using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public class EVoucher : CommonEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int? DiscountPayment { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public byte BuyType { get; set; }
        public int MaxBuy { get; set; }
        public int? MaxGift { get; set; }
        public bool Status { get; set; }
    }

    public class EVoucherRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public List<int> PaymentMethod { get; set; }
        public int? DiscountPayment { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public byte BuyType { get; set; }
        public int MaxBuy { get; set; }
        public int? MaxGift { get; set; }
    }

    public class EVoucherResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class EVoucherResult
    {
        public int ID { get; set; }
        public List<string> Codes { get; set; }
    }

    public class EVoucherUpdate
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class EVoucherDetail
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public List<PaymentResponse> PaymentMethod { get; set; }
        public int? DiscountPayment { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public string BuyType { get; set; }
        public int MaxBuy { get; set; }
        public int? MaxGift { get; set; }
        public string Status { get; set; }
    }
}
