using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.BAL;
using WebAPI.Model;

namespace Code_Test.Controllers
{
    [ApiVersion("0.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("MyAllowSpecificOrigins")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IOptions<AppSetting> options, IHttpContextAccessor httpContextAccessor)
        {
            _appSetting = options.Value;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpGet("LoginShop"), MapToApiVersion("0.0")]
        public IActionResult LoginShop(string mobile)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var result = TKmgr.GenerateToken(new Token() { Mobile = mobile, Role = 11 });
                if (!string.IsNullOrEmpty(result))
                {
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [HttpGet("LoginUser"), MapToApiVersion("0.0")]
        public IActionResult LoginUser(string mobile)
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var result = TKmgr.GenerateToken(new Token() { Mobile = mobile, Role = 21 });
                if (!string.IsNullOrEmpty(result))
                {
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
                }
                return Ok(new Response { Message = "Error", Status = APIStatus.Error });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = "SystemError", Status = APIStatus.SystemError, Data = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("RefreshToken"), MapToApiVersion("0.0")]
        public IActionResult RefreshToken()
        {
            try
            {
                TokenManager TKmgr = new TokenManager(_appSetting);
                var context = _httpContextAccessor.HttpContext;
                Token claim = TKmgr.GetClaimToken(context);
                var result = TKmgr.GenerateToken(claim);
                if (!string.IsNullOrEmpty(result))
                {
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = result });
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
