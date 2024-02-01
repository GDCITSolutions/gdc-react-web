using BE.LocalAccountabilitySystem.Business.Services;
using BE.LocalAccountabilitySystem.Common.Claims;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BE.LocalAccountabilitySystem.Api.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger _log;
        private readonly IUserService _service;

        public AuthenticationController(ILogger<AuthenticationController> log, IUserService service)
        {
            Util.Guard.ArgumentsAreNotNull(log, service);

            _log = log;
            _service = service;
        }

        [HttpPost, Route("api/login")]
        public async Task<IResult> Login(AuthenticationRequest request)
        {
            try
            {
                if (!_service.IsValidUser(request.EmailAddress, request.Password))
                    return Results.Problem(statusCode: 401, title: "Invalid Credentials", detail: "The provided credentials did not match any in the system.");

                var user = await _service.GetSessionUser(request.EmailAddress);

                var claims = new List<Claim> { 
                    new Claim(SessionClaims.UserEmail, user.EmailAddress),
                    new Claim(SessionClaims.UserId, user.Id.ToString()),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Could not get authenticated. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "Login Error", detail: "An error occurred when attempting to get authenticated");
            }
        }

        [Authorize, HttpPost, Route("api/logout")]
        public async Task<IResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Could not logout. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "Logout Error", detail: "An error occurred when attempting to logout.");
            }
        }
    }
}
