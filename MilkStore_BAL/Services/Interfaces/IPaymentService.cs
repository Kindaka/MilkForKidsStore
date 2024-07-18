using MilkStore_BAL.ModelViews.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDtoResponse> CreatePayment(PaymentDtoRequest paymentRequest);
    }
}
