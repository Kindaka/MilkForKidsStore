using AutoMapper;
using Microsoft.Extensions.Configuration;
using MilkStore_BAL.ModelViews.OrderDetailDTOs;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_BAL.VNPay;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> CheckVoucher(int voucherId)
        {
            try
            {
                var voucher = await _unitOfWork.VoucherOfShopRepository.GetByIDAsync(voucherId);
                if (voucher == null) { 
                    return false;
                }
                if(voucher.VoucherQuantity <= 0)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CreateOrder(List<OrderProductDto> cartItems, int? voucherId, int exchangedPoint)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    decimal totalPrice = 0;
                    int customerId = cartItems[0].customerId;
                    List<OrderDetailDtoRequest> orderProducts = new List<OrderDetailDtoRequest>();
                    foreach (var cartItem in cartItems)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIDAsync(cartItem.productId);
                        totalPrice += (product.ProductPrice * cartItem.quantity);
                        var orderProduct = new OrderDetailDtoRequest
                        {
                            ProductId = product.ProductId,
                            ProductPrice = product.ProductPrice,
                            OrderQuantity = cartItem.quantity
                        };
                        orderProducts.Add(orderProduct);
                    }

                    // minus voucher quantity and discount if exist
                    if (voucherId != null)
                    {
                        var voucher = await _unitOfWork.VoucherOfShopRepository.GetByIDAsync(voucherId);
                        voucher.VoucherQuantity--;
                        await _unitOfWork.VoucherOfShopRepository.UpdateAsync(voucher);
                        await _unitOfWork.SaveAsync();

                        totalPrice -= (totalPrice * (decimal) voucher.VoucherValue / 100);
                    }

                    // minus point and discount if exchangedPoint > 0
                    if (exchangedPoint > 0)
                    {
                        if ((totalPrice / 2) < exchangedPoint)
                        {
                            exchangedPoint = (int) totalPrice / 2;
                        }
                        var customer = await _unitOfWork.CustomerRepository.GetByIDAsync(customerId);
                        customer.Point = customer.Point - exchangedPoint;
                        await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                        await _unitOfWork.SaveAsync();

                        totalPrice -= exchangedPoint;
                    }

                    // create order
                    var order = new Order
                    {
                        CustomerId = customerId,
                        TotalPrice = totalPrice,
                        ExchangedPoint = exchangedPoint,
                        VoucherId = (voucherId == null ? null : voucherId),
                        Status = 0,
                        OrderDate = DateTime.Now,
                    };
                    await _unitOfWork.OrderRepository.AddAsync(order);
                    await _unitOfWork.SaveAsync();

                    // create order detail
                    foreach (var orderProduct in orderProducts)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = orderProduct.ProductId,
                            ProductPrice = (double) orderProduct.ProductPrice,
                            OrderQuantity = orderProduct.OrderQuantity,
                            Status = true
                        };
                        await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
                        await _unitOfWork.SaveAsync();
                    }

                    // minus quantity of product
                    foreach (var orderProduct in orderProducts)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIDAsync(orderProduct.ProductId);
                        if (product.ProductQuatity < orderProduct.OrderQuantity)
                        {
                            throw new Exception("Not enough product in stock");
                        }
                        product.ProductQuatity = product.ProductQuatity - orderProduct.OrderQuantity;
                        await _unitOfWork.ProductRepository.UpdateAsync(product);
                        await _unitOfWork.SaveAsync();
                    }

                    // delete items in cart
                    foreach (var cartItem in cartItems)
                    {
                        var item = await _unitOfWork.CartRepository.GetByIDAsync(cartItem.cartId);
                        await _unitOfWork.CartRepository.DeleteAsync(item);
                        await _unitOfWork.SaveAsync();
                    }

                    var paymentUrl = CreateVnpayLink(order);
                    await transaction.CommitAsync();
                    return paymentUrl;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> ValidateExchangedPoint(int exchangedPoint, int customerId)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIDAsync(customerId);
                if(customer == null)
                {
                    throw new Exception("Customer not found");
                }
                if(customer.Point < exchangedPoint)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ValidateItemInCart(List<OrderProductDto> cartItems)
        {
            try
            {
                foreach (var cartItem in cartItems)
                {
                    var item = (await _unitOfWork.CartRepository.GetAsync(filter: c => c.CartId == cartItem.cartId, includeProperties: "Product")).FirstOrDefault();
                    if (item == null)
                    {
                        return -1;
                    }
                    else
                    {
                        if (item.Product.ProductQuatity == 0)
                        {
                            return -2;
                        }
                        else
                        {
                            if (cartItem.quantity > item.Product.ProductQuatity)
                            {
                                return -3;
                            }
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string CreateVnpayLink(Order order)
        {
            var paymentUrl = string.Empty;

            var vpnRequest = new VNPayRequest(_configuration["VNpay:Version"], _configuration["VNpay:tmnCode"],
                order.OrderDate, "https://localhost:7223", (decimal)order.TotalPrice, "VND", "other",
                $"Thanh toan don hang {order.OrderId}", _configuration["VNpay:ReturnUrl"],
                $"{order.OrderId}");

            paymentUrl = vpnRequest.GetLink(_configuration["VNpay:PaymentUrl"],
                _configuration["VNpay:HashSecret"]);

            return paymentUrl;
        }
    }
}
