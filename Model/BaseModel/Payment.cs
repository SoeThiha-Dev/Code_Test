using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public class Payment : CommonEntity
    {
        public string PaymentName { get; set; }
    }

    public class PaymentResponse
    {
        public int ID { get; set; }
        public string PaymentName { get; set; }
    }
}
