using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.Model;
using WebAPI.Repo;

namespace WebAPI.BAL
{
    public class PaymentBAL : GenericBAL<Payment>, IPaymentBAL
    {
        private readonly IGenericRepo<Payment> _paymentRepo;

        public PaymentBAL(IGenericRepo<Payment> paymentRepo)
            : base(paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public List<PaymentResponse> GetAllPayment()
        {
            try
            {
                List<Payment> payments = _paymentRepo.GetAll().ToList();
                List<PaymentResponse> result = (from p in payments
                                                where p.Flag
                                                select new PaymentResponse()
                                                {
                                                    ID = p.ID,
                                                    PaymentName = p.PaymentName
                                                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PaymentResponse> GetSelectedPayment(List<int> id)
        {
            try
            {
                List<Payment> payments = _paymentRepo.GetAll().ToList();
                List<PaymentResponse> result = (from p in payments
                                                join i in id on p.ID equals i
                                                where p.Flag
                                                select new PaymentResponse()
                                                {
                                                    ID = p.ID,
                                                    PaymentName = p.PaymentName
                                                }).ToList();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
