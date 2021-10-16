using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Model;

namespace WebAPI.Repo
{
    public class EVoucherRepo : GenericRepo<EVoucher>, IEVoucherRepo
    {
        public EVoucherRepo(WebAPIDBContext dbContext)
             : base(dbContext)
        {
        }
    }
}
