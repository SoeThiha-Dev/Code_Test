using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public class Transcation : CommonEntity
    {
        public int EVoucherID { get; set; }
        public int PaymentID { get; set; }
        public string TranscationID { get; set; }
        public decimal Amount { get; set; }
        public string PurchaseMobile { get; set; }
    }

    public class TranscationRequest
    {
        public int EVoucherID { get; set; }
        public int PaymentID { get; set; }
        public CardInfo CardDetail { get; set; }
        public decimal Amount { get; set; }
        public string PurchaseMobile { get; set; }
    }

    public class CardInfo
    {
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string ExpiryYear { get; set; }
        public string ExpiryMonth { get; set; }
        public string CVV { get; set; }
    }
}
