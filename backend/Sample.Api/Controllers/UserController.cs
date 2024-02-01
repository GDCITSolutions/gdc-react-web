using BE.LocalAccountabilitySystem.Business.Services;
using BE.LocalAccountabilitySystem.Common;
using BE.LocalAccountabilitySystem.Common.Enum;
using BE.LocalAccountabilitySystem.Common.Exceptions;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Request;
using BE.LocalAccountabilitySystem.Entities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.LocalAccountabilitySystem.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly IUserService _userService;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly ISessionService _sessionService;

        public UserController(ILogger<UserController> log, IUserService userService, IResetPasswordService resetPasswordService, ISessionService sessionService)
        {
            Util.Guard.ArgumentsAreNotNull(log, userService, resetPasswordService, sessionService);

            _log = log;
            _userService = userService;
            _resetPasswordService = resetPasswordService;
            _sessionService = sessionService;
        }

        [Authorize, HttpGet, Route("session")]
        public async Task<IResult> GetSession()
        {
            try
            {
                var emailAddress = _sessionService.GetRequesterEmail();
                var sessionUser = await _userService.GetSessionUser(emailAddress);

                return Results.Ok(sessionUser);
            }
            catch (Exception ex)
            {
                _log.LogError("Could not get session user information. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "Session User Error", detail: "An error occurred when attempting get session user information.");
            }
        }

        [Authorize, HttpGet]
        public async Task<IResult> GetByDistrict()
        {
            try
            {
                _sessionService.HasRole(RoleEnum.DistrictAdmin);

                var users = await _userService.GetAll();

                return Results.Ok(users);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                _log.LogError("Could not get user information. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "User Information Error", detail: "An error occurred when attempting get district user information.");
            }
        }

        [Authorize, HttpPost]
        public async Task<IResult> Post(UserRequest request)
        {
            try
            {
                _sessionService.HasRole(RoleEnum.DistrictAdmin);

                await _userService.Create(request);

                return Results.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (UserException ex) 
            {
                return Results.Ok(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _log.LogError("Could not create user. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "User Creation Error", detail: "An error occurred when creating a new user");
            }
        }

        [Authorize, HttpPut]
        public async Task<IResult> Put(UserRequest request)
        {
            try
            {
                _sessionService.HasRole(RoleEnum.DistrictAdmin);

                var requesterId = _sessionService.GetRequesterId();

                await _userService.Update(request, requesterId);

                return Results.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (UserException ex)
            {
                return Results.Ok(new ErrorResponse(ex.Message));
            }
            catch (LockedEntityException ex)
            {
                return Results.Ok(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _log.LogError("Could not update user. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "User Update Error", detail: $"An error occurred when updating user {request.Id}");
            }
        }

        [Authorize, HttpPost, Route("bulk")]
        public async Task<IResult> BulkPost([FromForm(Name = "file")]IFormFile request)
        {
            try
            {
                _sessionService.HasRole(RoleEnum.DistrictAdmin);

                if (request == null) return Results.BadRequest();

                await _userService.BulkCreation(request);

                return Results.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (BulkImportException ex)
            {
                _log.LogError("Could not bulk create users. {ex}", ex);

                var response = ex.ExceptionMessages != null 
                    ? new ErrorResponse(ex.ExceptionMessages) 
                    : new ErrorResponse(ex.Message);

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError("Could not bulk create users. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "User Bulk Creation Error", detail: "An error occurred when bulk creating users");
            }
        }

        [Authorize, HttpDelete, Route("{id}")]
        public async Task<IResult> Delete(int id)
        {
            try
            {
                _sessionService.HasRole(RoleEnum.DistrictAdmin);

                var requesterId = _sessionService.GetRequesterId();
                var user = await _userService.GetById(id);

                await _userService.Delete(id, requesterId);

                return Results.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (LockedEntityException ex)
            {
                return Results.Ok(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _log.LogError("Could not delete user. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "User Delete Error", detail: "An error occurred when deleting user");
            }
        }

        [Authorize, HttpPut, Route("toggle-lock/{id}")]
        public async Task<IResult> ToggleLock(int id)
        {
            try
            {
                _sessionService.HasRole(RoleEnum.DistrictAdmin);

                var requesterId = _sessionService.GetRequesterId();
                var user = await _userService.GetById(id);

                var result = await _userService.ToggleLock(id, requesterId);

                return Results.Ok(new LockResponse(result));
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (LockedEntityException ex)
            {
                return Results.Ok(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _log.LogError("Could not lock user. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "User Lock Error", detail: "An error occurred when locking a user");
            }
        }

        [HttpPost, Route("self-registration")]
        public async Task<IResult> SelfRegister(SelfRegistrationRequest request)
        {
            try
            {
                await _userService.AddUser(request);
                await _resetPasswordService.SendEmail(request.EmailAddress);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Could not self register new user. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "Self Registration Error", detail: "An error occurred while self registering user.");
            }
        }

        [Authorize, HttpPut, Route("profile")]
        public async Task<IResult> UpdateProfile(UserProfileRequest request)
        {
            try
            {
                _sessionService.IsModifyingSelf(request.Id);
                await _userService.Update(request);

                return Results.Ok();
            }
            catch(Exception ex)
            {
                _log.LogError("Could not update user profile. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "Profile Update Error", detail: "An error occurred while updating the user profile.");
            }
        }
    }
}
