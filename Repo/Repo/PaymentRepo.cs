using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Model;

namespace WebAPI.Repo
{
    public class PaymentRepo : GenericRepo<Payment>, IPaymentRepo
    {
        public PaymentRepo(WebAPIDBContext dbContext)
                : base(dbContext)
        {
        }
    }
}
