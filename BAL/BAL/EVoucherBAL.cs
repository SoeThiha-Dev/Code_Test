using Hangfire;
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
    public class EVoucherBAL : GenericBAL<EVoucher>, IEVoucherBAL
    {
        private readonly IGenericRepo<EVoucher> _eVoucherRepo;
        private readonly IPaymentBAL _paymentBAL;
        private readonly AppSetting _appSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EVoucherBAL(IGenericRepo<EVoucher> eVoucherRepo, IPaymentBAL paymentBAL, IOptions<AppSetting> options, IHttpContextAccessor httpContextAccessor)
            : base(eVoucherRepo)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _appSetting = options.Value;
            _eVoucherRepo = eVoucherRepo;
            _paymentBAL = paymentBAL;
        }

        public async Task<int> CreateEVoucher(EVoucherRequest request)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 11)
                {
                    EVoucher eVoucher = new EVoucher()
                    {
                        Title = request.Title,
                        Description = request.Description,
                        ExpiryDate = request.ExpiryDate,
                        Image = request.Image,
                        Amount = request.Amount,
                        PaymentMethod = string.Join(", ", request.PaymentMethod),
                        DiscountPayment = request.DiscountPayment,
                        DiscountPercent = request.DiscountPercent,
                        Quantity = request.Quantity,
                        BuyType = request.BuyType,
                        MaxBuy = request.MaxBuy,
                        MaxGift = request.BuyType == 2 ? request.MaxGift : null,
                        Status = true,
                        CreateDate = DateTime.UtcNow,
                        Flag = true
                    };
                    bool obj = await _eVoucherRepo.Create(eVoucher);
                    if (obj)
                    {
                        return eVoucher.ID;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateEVoucher(EVoucherUpdate request)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 11)
                {
                    EVoucher eVoucher = _eVoucherRepo.GetById(request.ID);
                    if (eVoucher != null && eVoucher.Flag)
                    {
                        eVoucher.Title = request.Title;
                        eVoucher.Description = request.Description;
                        eVoucher.Image = request.Image;
                        bool obj = await _eVoucherRepo.Update(eVoucher);
                        if (obj)
                        {
                            return eVoucher.ID;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SetInActive(int id)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile) && claim.Role == 11)
                {
                    EVoucher eVoucher = _eVoucherRepo.GetById(id);
                    if (eVoucher != null && eVoucher.Flag)
                    {
                        eVoucher.Status = false;
                        bool obj = await _eVoucherRepo.Update(eVoucher);
                        if (obj)
                        {
                            return eVoucher.ID;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVoucherResponse> GetAllEVoucher()
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile))
                {
                    List<EVoucher> eVouchers = _eVoucherRepo.GetAll().ToList();
                    List<Payment> payments = _paymentBAL.GetAll().ToList();
                    List<EVoucherResponse> result = (from e in eVouchers
                                                     where e.Flag
                                                     orderby e.ID descending
                                                     select new EVoucherResponse()
                                                     {
                                                         ID = e.ID,
                                                         Title = e.Title,
                                                         Description = e.Description,
                                                         ExpiryDate = e.ExpiryDate,
                                                         Image = e.Image,
                                                         Amount = e.Amount,
                                                         Quantity = e.Quantity,
                                                         Status = e.Status ? "Active" : "InActive"
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

        public EVoucherDetail GetEVoucherByID(int id)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                if (!string.IsNullOrEmpty(claim.Mobile))
                {
                    EVoucher eVoucher = _eVoucherRepo.GetById(id);
                    if (eVoucher != null && eVoucher.Flag)
                    {
                        List<PaymentResponse> payments = _paymentBAL.GetAllPayment().ToList();
                        EVoucherDetail result = new EVoucherDetail()
                        {
                            ID = eVoucher.ID,
                            Title = eVoucher.Title,
                            Description = eVoucher.Description,
                            ExpiryDate = eVoucher.ExpiryDate,
                            Image = eVoucher.Image,
                            Amount = eVoucher.Amount,
                            Quantity = eVoucher.Quantity,
                            PaymentMethod = !string.IsNullOrEmpty(eVoucher.PaymentMethod) ? _paymentBAL.GetSelectedPayment(eVoucher.PaymentMethod.Split(',').Select(Int32.Parse).ToList()) : null,
                            DiscountPayment = eVoucher.DiscountPayment,
                            DiscountPercent = eVoucher.DiscountPercent,
                            BuyType = eVoucher.BuyType == 1 ? "only me usage" : "gift to others",
                            MaxBuy = eVoucher.MaxBuy,
                            MaxGift = eVoucher.MaxGift,
                            Status = eVoucher.Status ? "Active" : "InActive",
                        };
                        return result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExpiredEVoucher(int id)
        {
            try
            {
                EVoucher eVoucher = _eVoucherRepo.GetById(id);
                if(eVoucher != null && eVoucher.Flag)
                {
                    eVoucher.Status = false;
                    return await _eVoucherRepo.Update(eVoucher);
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
