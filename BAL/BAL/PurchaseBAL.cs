using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Repo;

namespace WebAPI.BAL
{
    public class PurchaseBAL : GenericBAL<Purchase>, IPurchaseBAL
    {
        private readonly IGenericRepo<Purchase> _purchaseRepo;
        private readonly IEVoucherRepo _eVoucherRepo;
        private readonly ITranscationBAL _transcationBAL;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSetting _appSetting;

        public PurchaseBAL(IGenericRepo<Purchase> purchaseRepo, IEVoucherRepo eVoucherRepo, ITranscationBAL transcationBAL, IOptions<AppSetting> options, IHttpContextAccessor httpContextAccessor)
            : base(purchaseRepo)
        {
            _appSetting = options.Value;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _purchaseRepo = purchaseRepo;
            _eVoucherRepo = eVoucherRepo;
            _transcationBAL = transcationBAL;
        }

        public async Task<List<PurchaseSuccess>> CreatePurchase(PurchaseRequest request)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 21)
                {

                    EVoucher eVoucher = _eVoucherRepo.GetById(request.EVoucherID);
                    if(eVoucher != null && eVoucher.Flag && eVoucher.Status && eVoucher.ExpiryDate > DateTime.UtcNow)
                    {

                        List<Purchase> purchases = _purchaseRepo.GetByExp(x => x.EVoucherID == eVoucher.ID && x.Flag).ToList();
                        if(purchases.Count() + request.Quantity > eVoucher.Quantity)
                        {
                            return null;
                        }
                        else if(request.Quantity > eVoucher.MaxBuy)
                        {
                            return null;
                        }
                        else if (purchases.Where(x=>x.PurchaseMobile == claim.Mobile).Count() + request.Quantity > eVoucher.MaxBuy)
                        {
                            return null;
                        }
                        else if(eVoucher.BuyType == 2 && request.Quantity > eVoucher.MaxGift)
                        {
                            return null;
                        }
                        else if(eVoucher.BuyType == 2 && purchases.Where(x=>x.PurchaseMobile == claim.Mobile && x.UserMobile == request.UserMobile).Count() + request.Quantity > eVoucher.MaxGift)
                        {
                            return null;
                        }

                        bool obj = await _transcationBAL.CreateTranscation(new TranscationRequest()
                        {
                            EVoucherID = eVoucher.ID,
                            PaymentID = request.PaymentID,
                            CardDetail = request.CardDetail,
                            Amount = eVoucher.Amount * request.Quantity,
                            PurchaseMobile = claim.Mobile
                        });
                        if (!obj)
                        {
                            return null;
                        }

                        PromoCodeClass generate = new PromoCodeClass();
                        purchases = new List<Purchase>();
                        for(int i = 0; i < request.Quantity; i++)
                        {
                            string code = null;
                            do
                            {
                                code = generate.GeneratePromoCode();
                            }
                            while (_purchaseRepo.GetByExp(x => x.Code == code).FirstOrDefault() != null || purchases.Exists(x => x.Code == code));
                            if (!string.IsNullOrEmpty(code))
                            {
                                purchases.Add(new Purchase()
                                {
                                    Code = code,
                                    EVoucherID = request.EVoucherID,
                                    UserName = eVoucher.BuyType == 2 ? request.UserName : request.PurchaseName,
                                    UserMobile = eVoucher.BuyType == 2 ? request.UserMobile : request.PurchaseMobile,
                                    PurchaseName = request.PurchaseName,
                                    PurchaseMobile = request.PurchaseMobile,
                                    Status = false,
                                    CreateDate = DateTime.UtcNow,
                                    Flag = true
                                });
                            }
                        }
                        obj = await _purchaseRepo.CreateRange(purchases);
                        if (obj)
                        {
                            QRCodeClass qrCode = new QRCodeClass();
                            return purchases.Select(x => new PurchaseSuccess() { ID = x.ID, PromoCode = x.Code, QRCode = qrCode.GenerateQRCode(x.Code) }).ToList();
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PurchaseResponse> GetAllPurchase(bool status)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 11)
                {
                    QRCodeClass qrCode = new QRCodeClass();
                    List<Purchase> purchases = _purchaseRepo.GetAll().ToList();
                    List<PurchaseResponse> result = (from p in purchases
                                                     where p.Flag && p.Status == status
                                                     orderby p.ID descending
                                                     select new PurchaseResponse()
                                                     {
                                                         ID = p.ID,
                                                         UserName = p.UserName,
                                                         UserMobile = p.UserMobile,
                                                         PromoCode = !status ? null : p.Code,
                                                         QRCode = !status ? null : qrCode.GenerateQRCode(p.Code)
                                                     }).ToList();
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string VerifyEVoucher(string code)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 21)
                {
                    Purchase purchase = _purchaseRepo.GetByExp(x => x.UserMobile == claim.Mobile && x.Code == code && x.Flag).FirstOrDefault();
                    if (purchase != null)
                    {
                        if (purchase.Status)
                        {
                            return "eVoucher already used!";
                        }
                        else
                        {
                            EVoucher eVoucher = _eVoucherRepo.GetByExp(x => x.ID == purchase.EVoucherID).FirstOrDefault();
                            if (eVoucher != null)
                            {
                                if (!eVoucher.Flag || DateTime.UtcNow > eVoucher.ExpiryDate)
                                {
                                    return "eVoucher expired!";
                                }
                                else if (!eVoucher.Status)
                                {
                                    return "InActive eVoucher!";
                                }
                                return "Ok";
                            }
                        }
                    }
                    return "Wrong eVoucher code!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UseEVoucher(int id)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 21)
                {
                    Purchase purchase = _purchaseRepo.GetById(id);
                    if (purchase != null && purchase.UserMobile == claim.Mobile && purchase.Flag && !purchase.Status)
                    {
                        purchase.Status = true;
                        purchase.UsedDate = DateTime.UtcNow;
                        bool obj = await _purchaseRepo.Update(purchase);
                        return obj;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
