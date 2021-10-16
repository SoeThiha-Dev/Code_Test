using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public class Purchase : CommonEntity
    {
        public string Code { get; set; }
        public int EVoucherID { get; set; }
        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public string PurchaseName { get; set; }
        public string PurchaseMobile { get; set; }
        public bool Status { get; set; }
        public DateTime? UsedDate { get; set; }
    }

    public class PurchaseRequest
    {
        public int EVoucherID { get; set; }
        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public string PurchaseName { get; set; }
        public string PurchaseMobile { get; set; }
        public int PaymentID { get; set; }
        public CardInfo CardDetail { get; set; }
        public int Quantity { get; set; }
    }

    public class PurchaseResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public string PromoCode { get; set; }
        public string QRCode { get; set; }
    }

    public class PurchaseSuccess
    {
        public int ID { get; set; }
        public string PromoCode { get; set; }
        public string QRCode { get; set; }
    }
}
