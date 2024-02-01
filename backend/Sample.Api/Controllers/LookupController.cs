using BE.LocalAccountabilitySystem.Business.Services;
using BE.LocalAccountabilitySystem.Common.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.LocalAccountabilitySystem.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize]
    public class LookupController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly ILookupService _service;

        public LookupController(ILogger<UserController> log, ILookupService service)
        {
            Util.Guard.ArgumentsAreNotNull(log, service);

            _log = log;
            _service = service;
        }

        [HttpGet, Route("roles")]
        public async Task<IResult> GetRoles()
        {
            try
            {
                var roles = await _service.GetRoles();

                return Results.Ok(roles);
            }
            catch (Exception ex)
            {
                _log.LogError("Could not get roles. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "Role Error", detail: "An error occurred when attempting get roles.");
            }
        }

        [HttpGet, Route("system-statuses")]
        public async Task<IResult> GetSystemStatuses()
        {
            try
            {
                var statuses = await _service.GetSystemStatuses();

                return Results.Ok(statuses);
            }
            catch (Exception ex)
            {
                _log.LogError("Could not get system statuses. {ex}", ex);
                return Results.Problem(statusCode: 500, title: "System Status Error", detail: "An error occurred when attempting get system statuses.");
            }
        }
    }
}
