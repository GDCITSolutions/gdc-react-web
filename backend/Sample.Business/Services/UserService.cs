using BE.LocalAccountabilitySystem.Business.Managers;
using BE.LocalAccountabilitySystem.Common.Enum;
using BE.LocalAccountabilitySystem.Common.Security;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Database;
using BE.LocalAccountabilitySystem.Entities.Request;
using BE.LocalAccountabilitySystem.Entities.Response;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Business.Services
{
    /// <summary>
    /// Manage operations surrounding users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds a user who registered themselves in the system.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddUser(SelfRegistrationRequest request);

        /// <summary>
        /// Gets user data by email address.
        /// </summary>
        /// <param name="emailAddress">Email address of user to get data for.</param>
        /// <returns>Session response object containing user data.</returns>
        Task<SessionResponse> GetSessionUser(string emailAddress);

        /// <summary>
        /// Determines if the provided email address and password.
        /// </summary>
        /// <param name="emailAddress">Email address of user.</param>
        /// <param name="password">Passsword associated with the user.</param>
        /// <returns></returns>
        bool IsValidUser(string emailAddress, string password);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        Task<IList<UserManagementResponse>> GetAll();

        /// <summary>
        /// Get a user by their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetById(int id);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Create(UserRequest request);

        /// <summary>
        /// Consume an excel/csv document to create a set of users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task BulkCreation(IFormFile request);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Update(UserRequest request, int requesterId);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Update(UserProfileRequest request);

        /// <summary>
        /// Soft-delete a user by setting their system status id to inactive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id, int requesterId);

        /// <summary>
        /// Lock a user by setting their system status id to locked
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Whether or not the user was locked or unlocked</returns>
        Task<bool> ToggleLock(int id, int requesterId);
    }

    public class UserService : IUserService
    {
        #region Variables

        private readonly SampleContext _context;
        private readonly IUserManager _userManager;

        #endregion

        #region Constructors

        public UserService(SampleContext context, IUserManager userManager)
        {
            Util.Guard.ArgumentsAreNotNull(context, userManager);

            _context = context;
            _userManager = userManager;
        }

        #endregion

        #region Public Methods

        public async Task AddUser(SelfRegistrationRequest request)
        {
            Util.Guard.ArgumentsAreNotNull(request);

            var user = new User(1, request.EmailAddress);

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var role = new UserToRole() { UserId = user.Id, RoleId = request.RoleId, SystemStatusId = SystemStatusEnum.Active.AsInt() };

            await _context.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task<SessionResponse> GetSessionUser(string emailAddress)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(x => x.EmailAddress == emailAddress);

            var userRoles = await _context.UserToRole
                .Include(x => x.Role)
                .Where(x => x.UserId == user.Id && x.SystemStatusId == SystemStatusEnum.Active.AsInt())
                .ToListAsync();

            userRoles.ForEach(x => x.User = null);

            return new SessionResponse(user, userRoles);
        }

        public bool IsValidUser(string emailAddress, string password)
        {
            Util.Guard.ArgumentsAreNotNull(emailAddress, password);

            var user = _context.User.Where(x => x.EmailAddress.ToLower() == emailAddress.ToLower())
                .FirstOrDefault(x => x.SystemStatusId != SystemStatusEnum.Removed.AsInt());

            if (user == null)
                return false;

            if (!SecurityUtil.VerifyHash(password, user.Salt, user.Password))
                return false;

            return true;
        }

        public async Task Create(UserRequest request) 
        {
            await _userManager.Create(request);
        }

        public async Task Update(UserRequest request, int requesterId)
        {
            await _userManager.Update(request, requesterId);
        }

        public async Task Update(UserProfileRequest request)
        {
            await _userManager.Update(request);
        }

        public async Task BulkCreation(IFormFile request)
        {
            await _userManager.BulkCreation(request);
        }

        public async Task Delete(int id, int requesterId)
        {
            await _userManager.Delete(id, requesterId);
        }

        public async Task<bool> ToggleLock(int id, int requesterId)
        {
            return await _userManager.ToggleLock(id, requesterId);
        }

        public async Task<IList<UserManagementResponse>> GetAll() 
        {
            return await _context.User
                .Select(x => new UserManagementResponse() 
                {
                    Id = x.Id,
                    EmailAddress = x.EmailAddress,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Roles = _context.UserToRole
                        .Where(r => r.UserId == x.Id && r.SystemStatusId == SystemStatusEnum.Active.AsInt())
                        .Select(r => new UserManagementRoleResponse()
                        {
                            Id = r.RoleId,
                            Name = r.Role.Value
                        })
                        .ToList(),
                    LockedByUserId = x.LockedByUserId,
                    SystemStatusName = x.SystemStatus.Value,
                    SystemStatusId = x.SystemStatusId   
                })
                .ToListAsync();
        }

        public async Task<User> GetById(int id) 
        {
            return await _context.User
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        #endregion
    }
}
