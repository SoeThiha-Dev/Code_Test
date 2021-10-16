using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.BAL
{
    public interface IPurchaseBAL : IGenericBAL<Purchase>
    {
        Task<List<PurchaseSuccess>> CreatePurchase(PurchaseRequest request);
        List<PurchaseResponse> GetAllPurchase(bool status);
        string VerifyEVoucher(string code);
        Task<bool> UseEVoucher(int id);
    }
}
