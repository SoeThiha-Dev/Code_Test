using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Authen
{
    public class UserAuthen
    {
        public static bool ValidateToken(string mobile, string ControllerName)
        {
            var AllCanUse = GetAllCanUse();
            return AllCanUse.Contains(ControllerName.ToUpper());
        }
        private static List<string> GetAllCanUse()
        {
            return new List<string>() {

                "/api/v0.0/Login/RefreshToken".ToUpper(),

                "/api/v0.0/EVoucher/GetAllEVoucher".ToUpper(),
                "/api/v0.0/EVoucher/GetEVoucherByID".ToUpper(),
                "/api/v0.0/EVoucher/CheckOut".ToUpper(),
                "/api/v0.0/EVoucher/GetAllPayment".ToUpper(),
                "/api/v0.0/EVoucher/VerifyPromoCode".ToUpper(),
                "/api/v0.0/EVoucher/UseEVoucher".ToUpper(),
            };
        }
    }
}
