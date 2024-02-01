using BE.LocalAccountabilitySystem.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace BE.LocalAccountabilitySystem.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Consumes("application/json")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly ILogger _log;
        private readonly IResetPasswordService _resetPasswordService;

        public ResetPasswordController(ILogger<ResetPasswordController> log, IResetPasswordService resetPasswordService)
        {
            _log = log;
            _resetPasswordService = resetPasswordService;
        }

        [HttpPost]
        [Route("user/reset")]
        public async Task<IActionResult> ResetPasswordEmail([FromBody] string email)
        {
            try
            {
                return await _resetPasswordService.SendEmail(email)
                    ? Ok("Password reset email sent successfully.")
                    : Ok();
            }
            catch (Exception ex) 
            {
                _log.LogError($"Error occured when sending email: {ex}");
                return Ok();
            }
        }

        [HttpGet]
        [Route("user/reset")]
        public async Task<IResult> GetResetPassword([FromQuery] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Results.Problem(statusCode: 401, title: "Token Validation Error", detail: "Token is empty");
                }

                var validationResponse = await _resetPasswordService.ValidateToken(token);

                if (validationResponse.IsValid)
                {
                    return Results.Ok(new { success = validationResponse.Message, email = validationResponse.Email });
                }
                else
                {
                    return Results.Problem(statusCode: 401, title: "Token Validation Error", detail: validationResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error occurred when trying to validate token: {ex}");

                return Results.Problem(statusCode: 500, title: "Token Validation Error", detail: "An error occurred during token validation" );
            }
        }


        [HttpPut]
        [Route("user/credentials")]
        public async Task<IResult> ResetPassword([FromBody] string newPassword, [FromQuery] string resetToken)
        {
            try
            {
                if (string.IsNullOrEmpty(newPassword))
                {
                    return Results.Problem(statusCode: 401, title: "Reset Password Error", detail: "New password is empty");
                }

                return await _resetPasswordService.ResetPassword(newPassword, resetToken)
                    ? Results.Ok(new { success = "Password reset was a success" })
                    : Results.Problem(statusCode: 401, title: "Reset Password Error", detail: "Password reset failed");

            }
            catch (Exception ex)
            {
                _log.LogError($"Error occurred while resetting password: {ex}");

                return Results.Problem(statusCode: 401, title: "Reset Password Error", detail: "An error occurred while resetting password");
            }
        }


    }
}
