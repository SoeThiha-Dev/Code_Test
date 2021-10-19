using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Authen
{
    public class AuthorizationHeaderRequirement : IAuthorizationRequirement
    {
    }
    public class AuthorizationHeaderHandler : AuthorizationHandler<AuthorizationHeaderRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationHeaderRequirement requirement)
        {
            var mobile = "";
            var role = "";

            if (_httpContextAccessor != null)
            {
                if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var Control = _httpContextAccessor.HttpContext.Request.Path;
                    string ControllerName = Control.HasValue ? Control.Value : "";
                    foreach (var item in context.User.Claims)
                    {
                        if (item.Type == "Mobile")
                        {
                            mobile = item.Value;
                        }
                        else if (item.Type == "Role")
                        {
                            role = item.Value;
                        }
                    }
                    bool FlagValidate = false;
                    switch (role)
                    {
                        case "11": //--- eShop
                            FlagValidate = EShopAuthen.ValidateToken(mobile, ControllerName);
                            break;
                        case "21": //--- User
                            FlagValidate = UserAuthen.ValidateToken(mobile, ControllerName);
                            break;
                    }
                    if (FlagValidate)
                    {
                        context.Succeed(requirement);
                    }
                    return;
                }
            }
            return;
        }
    }
}
