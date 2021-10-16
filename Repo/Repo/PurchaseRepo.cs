using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Model;

namespace WebAPI.Repo
{
    public class PurchaseRepo : GenericRepo<Purchase>, IPurchaseRepo
    {
        public PurchaseRepo(WebAPIDBContext dbContext)
                   : base(dbContext)
        {
        }
    }
}
