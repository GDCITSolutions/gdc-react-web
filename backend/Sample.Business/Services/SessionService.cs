using BE.LocalAccountabilitySystem.Common.Claims;
using BE.LocalAccountabilitySystem.Common.Enum;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.AspNetCore.Http;

namespace BE.LocalAccountabilitySystem.Business.Services
{
    /// <summary>
    /// Manage all operations related to session claims.
    /// </summary>
    public interface ISessionService 
    {
        /// <summary>
        /// Get the requesting user's email address
        /// </summary>
        /// <returns></returns>
        string GetRequesterEmail();

        /// <summary>
        /// Get the requesting user's ID
        /// </summary>
        /// <returns></returns>
        int GetRequesterId();

        /// <summary>
        /// Determine if some requester has any of these roles
        /// </summary>
        /// <param name="allowedRoles"></param>
        void HasRole(IList<RoleEnum> allowedRoles);

        /// <summary>
        /// Determine if some request has this role
        /// </summary>
        /// <param name="allowedRole"></param>
        void HasRole(RoleEnum allowedRole);

        /// <summary>
        /// Verify that the requester is modifying their own user data.
        /// </summary>
        /// <param name="id">Identifier of data being manipulated.</param>
        void IsModifyingSelf(int id);
    }

    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _context;
        private readonly SampleContext _dbContext;

        public SessionService(IHttpContextAccessor context, SampleContext dbContext) 
        {
            Util.Guard.ArgumentsAreNotNull(context, dbContext);

            _context = context;
            _dbContext = dbContext;
        }

        public string GetRequesterEmail() 
        {
            return _context.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == SessionClaims.UserEmail).Value;
        }

        public int GetRequesterId()
        {
            string value = _context.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == SessionClaims.UserId).Value;

            bool success = int.TryParse(value, out int result);

            if (!success)
                throw new BadHttpRequestException("User ID claim is missing");

            return result;
        }

        public IList<RoleEnum> GetRequesterRoles()
        {
            var id = GetRequesterId();

            return _dbContext.UserToRole
                .Where(x => x.SystemStatusId == SystemStatusEnum.Active.AsInt())
                .Select(x => (RoleEnum)x.RoleId)
                .ToList();
        }

        public void HasRole(IList<RoleEnum> allowedRoles) 
        {
            var roles = GetRequesterRoles();

            if (!roles.Any(allowedRoles.Contains))
                throw new UnauthorizedAccessException("No permissions to utilize this endpoint");
        }

        public void HasRole(RoleEnum allowedRole)
        {
            var roles = GetRequesterRoles();

            if (!roles.Contains(allowedRole))
                throw new UnauthorizedAccessException("No permissions to utilize this endpoint");
        }

        public void IsModifyingSelf(int id)
        {
            if (id != GetRequesterId())
                throw new UnauthorizedAccessException("The requester does not have permission to modify the provided user data.");
        }
    }
}
