using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.BAL
{
    public class PromoCodeClass
    {
        private Random random = new Random();
        public string GeneratePromoCode()
        {
            const string nums = "0123456789";
            string numStr = new string(Enumerable.Repeat(nums, 6).Select(s => s[random.Next(10)]).ToArray());
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            string charStr = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(26)]).ToArray());
            string result = numStr + charStr;
            return result;
        }
    }
}
