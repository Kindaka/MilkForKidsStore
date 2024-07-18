using AutoMapper;
using MilkStore_BAL.ModelViews.PaymentDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaymentDtoResponse> CreatePayment(PaymentDtoRequest paymentRequest)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var existedOrder = await _unitOfWork.OrderRepository.GetByIDAsync(int.Parse(paymentRequest.vnp_TxnRef));
                    if (existedOrder != null)
                    {
                        var existedPayment = await _unitOfWork.PaymentRepository.GetAsync(p => p.OrderId == existedOrder.OrderId);
                        if(existedPayment.Any())
                        {
                            return null;
                        }
                        var payment = new Payment()
                        {
                            PaymentMethod = "VNPay",
                            BankCode = paymentRequest.vnp_BankCode,
                            BankTranNo = paymentRequest.vnp_BankTranNo,
                            CardType = paymentRequest.vnp_CardType,
                            PaymentInfo = paymentRequest.vnp_OrderInfo,
                            PayDate = DateTime.ParseExact(paymentRequest.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                            TransactionNo = paymentRequest.vnp_TransactionNo,
                            TransactionStatus = int.Parse(paymentRequest.vnp_TransactionStatus),
                            PaymentAmount = decimal.Parse(paymentRequest.vnp_Amount) / 100,
                            OrderId = int.Parse(paymentRequest.vnp_TxnRef)
                        };
                        await _unitOfWork.PaymentRepository.AddAsync(payment);
                        await _unitOfWork.SaveAsync();

                        // Update Order's status is Paid
                        existedOrder.Status = 1;
                        await _unitOfWork.OrderRepository.UpdateAsync(existedOrder);
                        await _unitOfWork.SaveAsync();

                        // accumulate points customer point
                        var customer = await _unitOfWork.CustomerRepository.GetByIDAsync(existedOrder.CustomerId);
                        customer.Point += (int)((double) payment.PaymentAmount * 0.02); // 2% per successful order
                        await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                        await _unitOfWork.SaveAsync();

                        await transaction.CommitAsync();
                        return _mapper.Map<PaymentDtoResponse>(payment);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
