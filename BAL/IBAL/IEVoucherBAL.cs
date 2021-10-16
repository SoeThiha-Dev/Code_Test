using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.BAL
{
    public interface IEVoucherBAL : IGenericBAL<EVoucher>
    {
        Task<int> CreateEVoucher(EVoucherRequest request);
        List<EVoucherResponse> GetAllEVoucher();
        EVoucherDetail GetEVoucherByID(int id);
        Task<int> UpdateEVoucher(EVoucherUpdate request);
        Task<int> SetInActive(int id);
        Task<bool> ExpiredEVoucher(int id);
    }
}
