using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Model;

namespace WebAPI.BAL
{
    public interface IPaymentBAL : IGenericBAL<Payment>
    {
        List<PaymentResponse> GetAllPayment();
        List<PaymentResponse> GetSelectedPayment(List<int> id);
    }
}
