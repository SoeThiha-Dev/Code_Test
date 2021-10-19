using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.BAL;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [ApiVersion("0.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("MyAllowSpecificOrigins")]
    [ApiController]
    [Authorize(Policy = "AuthorizationHeaderRequirement")]
    public class EVoucherController : ControllerBase
    {
        private readonly IEVoucherBAL _eVoucherBAL;
        private readonly IPaymentBAL _paymentBAL;
        private readonly IPurchaseBAL _purchaseBAL;
        public EVoucherController(IEVoucherBAL eVoucherBAL, IPaymentBAL paymentBAL, IPurchaseBAL purchaseBAL)
        {
            _eVoucherBAL = eVoucherBAL;
            _paymentBAL = paymentBAL;
            _purchaseBAL = purchaseBAL;
        }

        [HttpPost("CreateEVoucher"), MapToApiVersion("0.0")]
        public async Task<IActionResult> CreateEVoucher([FromBody] EVoucherRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _eVoucherBAL.CreateEVoucher(request);
                    if (result > 0)
                    {
                        return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = new { EVoucherID = result } });
                    }
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpPost("EditEVoucher"), MapToApiVersion("0.0")]
        public async Task<IActionResult> EditEVoucher([FromBody] EVoucherUpdate request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _eVoucherBAL.UpdateEVoucher(request);
                    if (result > 0)
                    {
                        return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = new { EVoucherID = result } });
                    }
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("SetInActive"), MapToApiVersion("0.0")]
        public async Task<IActionResult> SetInActive(int id)
        {
            try
            {
                var result = await _eVoucherBAL.SetInActive(id);
                if (result > 0)
                {
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = new { EVoucherID = result } });
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        //----------------------------------------------------------------------------------------------------//

        [HttpGet("GetAllEVoucher"), MapToApiVersion("0.0")]
        public IActionResult GetAllEVoucher()
        {
            try
            {
                var result = _eVoucherBAL.GetAllEVoucher();
                return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("GetEVoucherByID"), MapToApiVersion("0.0")]
        public IActionResult GetEVoucherByID(int id)
        {
            try
            {
                var result = _eVoucherBAL.GetEVoucherByID(id);
                return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpPost("CheckOut"), MapToApiVersion("0.0")]
        public async Task<IActionResult> CheckOut([FromBody] PurchaseRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _purchaseBAL.CreatePurchase(request);
                    if (result != null)
                    {
                        return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
                    }
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("GetAllPayment"), MapToApiVersion("0.0")]
        public IActionResult GetAllPayment()
        {
            try
            {
                var result = _paymentBAL.GetAllPayment();
                return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("VerifyPromoCode"), MapToApiVersion("0.0")]
        public IActionResult VerifyPromoCode(string code)
        {
            try
            {
                var result = _purchaseBAL.VerifyEVoucher(code);
                if (result == "Ok")
                {
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error, Data = result });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("PurchaseHistory"), MapToApiVersion("0.0")]
        public IActionResult PurchaseHistory(bool status)
        {
            try
            {
                var result = _purchaseBAL.GetAllPurchase(status);
                return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("UseEVoucher"), MapToApiVersion("0.0")]
        public async Task<IActionResult> UseEVoucher(int id)
        {
            try
            {
                var result = await _purchaseBAL.UseEVoucher(id);
                if (result)
                {
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull });
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }
    }
}
