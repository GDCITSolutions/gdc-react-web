using BE.LocalAccountabilitySystem.Business.Spreadsheets;
using BE.LocalAccountabilitySystem.Common;
using BE.LocalAccountabilitySystem.Common.Enum;
using BE.LocalAccountabilitySystem.Common.Exceptions;
using BE.LocalAccountabilitySystem.Common.Security;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Database;
using BE.LocalAccountabilitySystem.Entities.Request;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BE.LocalAccountabilitySystem.Business.Managers
{
    /// <summary>
    /// Faciliate User operations related to create, update, and delete
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Create(UserRequest request);

        /// <summary>
        /// Consume an excel/csv document to create a set of users
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        Task BulkCreation(IFormFile request);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requesterId"></param>
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
        /// <param name="requesterId"></param>
        /// <returns></returns>
        Task Delete(int id, int requesterId);

        /// <summary>
        /// Lock a user by setting their system status id to locked.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requesterId"></param>
        /// <returns>Whether or not the user was locked or unlocked</returns>
        Task<bool> ToggleLock(int id, int requesterId);
    }

    public class UserManager : IUserManager
    {
        #region Variables
        private readonly SampleContext _context;
        private readonly IUserSpreadsheetParser _spreadsheetParser;
        #endregion

        #region Constructor
        public UserManager(SampleContext context, IUserSpreadsheetParser spreadsheetParser) 
        {
            Util.Guard.ArgumentsAreNotNull(context, spreadsheetParser);

            _context = context;
            _spreadsheetParser = spreadsheetParser;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Verify that there is not a duplicate email
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="userId"></param>
        /// <exception cref="UserException"></exception>
        private void ValidateEmailAddress(string emailAddress, int userId = 0)
        {
            if (_context.User.Any(x => x.EmailAddress == emailAddress && x.Id != userId))
                throw new UserException("Email address is already in use");
        }

        /// <summary>
        /// Verify that this entity is not locked by someone else
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requesterId"></param>
        /// <exception cref="LockedEntityException"></exception>
        private void GuardLocked(int userId, int requesterId)
        {
            if (_context.User.Any(x => x.Id == userId && x.LockedByUserId != null && x.LockedByUserId != requesterId))
                throw new LockedEntityException($"User {userId} is locked");
        }

        /// <summary>
        /// Validate a bulk creation request
        /// </summary>
        /// <param name="users"></param>
        /// <exception cref="BulkImportException"></exception>
        private void ValidateBulkCreation(IList<UserRequest> users) 
        {
            var emails = users.Select(x => x.EmailAddress).ToList();
            var duplicateEmails = _context.User.Where(x => emails.Contains(x.EmailAddress)).ToList();

            if (duplicateEmails.Count > 0)
                throw new BulkImportException($"Emails {string.Join(", ", duplicateEmails.Select(x => x.EmailAddress))} are already in use");

            var duplicatesInFile = emails
                .GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            if (duplicatesInFile.Count > 0)
                throw new BulkImportException($"Emails {string.Join(", ", duplicatesInFile)} are duplicated in the file");
        }

        /// <summary>
        /// Create/Remove user roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task ResolveRoles(User user, UserRequest request) 
        {
            var currentRoles = _context.UserToRole
                .Where(x => x.UserId == user.Id && x.SystemStatusId == SystemStatusEnum.Active.AsInt())
                .ToList();
            var toDeleteRoles = currentRoles.Where(x => !request.RoleIds.Contains(x.RoleId));
            var toAddRoles = request.RoleIds.Where(x => !currentRoles.Any(y => y.RoleId == x));

            foreach (var role in toAddRoles)
                _context.UserToRole.Add(new UserToRole() { RoleId = role, UserId = user.Id, SystemStatusId = SystemStatusEnum.Active.AsInt() });
            foreach (var role in toDeleteRoles)
                role.SystemStatusId = SystemStatusEnum.Removed.AsInt();

            await _context.SaveChangesAsync();
        }

        #endregion

        public async Task Create(UserRequest request)
        {
            ValidateEmailAddress(request.EmailAddress);

            using (var transaction = _context.Database.BeginTransaction()) 
            {
                try
                {
                    var user = new User()
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        EmailAddress = request.EmailAddress,
                        Password = new byte[5], // no password at this point
                        Salt = new byte[5],
                        SystemStatusId = SystemStatusEnum.Active.AsInt()
                    };

                    await _context.User.AddAsync(user);
                    await _context.SaveChangesAsync();

                    foreach (var role in request.RoleIds)
                        _context.UserToRole.Add(new UserToRole() { RoleId = role, UserId = user.Id, SystemStatusId = SystemStatusEnum.Active.AsInt() });

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception) 
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task Update(UserRequest request, int requesterId)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("User does not exist");

            GuardLocked(request.Id, requesterId);

            ValidateEmailAddress(request.EmailAddress, request.Id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.EmailAddress = request.EmailAddress;
                user.SystemStatusId = request.SystemStatusId;

                if (request.SystemStatusId == SystemStatusEnum.Locked.AsInt())
                    user.LockedByUserId = requesterId;
                else
                    user.LockedByUserId = null;

                await _context.SaveChangesAsync();

                await ResolveRoles(user, request);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Update(UserProfileRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("User does not exist");

            ValidateEmailAddress(request.EmailAddress, request.Id);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.EmailAddress = request.EmailAddress;

                    if (!string.IsNullOrEmpty(request.NewPassword))
                    {
                        user.Salt = SecurityUtil.GenerateSalt();
                        user.Password = SecurityUtil.HashPassword(request.NewPassword, user.Salt);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task BulkCreation(IFormFile request)
        {
            var users = await _spreadsheetParser.ParseUserImport(request);

            ValidateBulkCreation(users);

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                foreach (var u in users)
                {
                    var user = new User()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        EmailAddress = u.EmailAddress,
                        Password = new byte[5], // no password at this point
                        Salt = new byte[5],
                        SystemStatusId = u.SystemStatusId
                    };

                    await _context.User.AddAsync(user);
                    await _context.SaveChangesAsync();

                    foreach (var role in u.RoleIds)
                        _context.UserToRole.Add(new UserToRole() { RoleId = role, UserId = user.Id, SystemStatusId = SystemStatusEnum.Active.AsInt() });

                    await _context.SaveChangesAsync();
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Delete(int id, int requesterId)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("User does not exist");

            GuardLocked(id, requesterId);

            user.SystemStatusId = SystemStatusEnum.Removed.AsInt();

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ToggleLock(int id, int requesterId)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("User does not exist");

            GuardLocked(id, requesterId);

            bool isLocked = user.SystemStatusId == SystemStatusEnum.Locked.AsInt();

            user.SystemStatusId = isLocked
                ? SystemStatusEnum.Active.AsInt() 
                : SystemStatusEnum.Locked.AsInt();
            user.LockedByUserId = isLocked ? null : requesterId;

            await _context.SaveChangesAsync();

            return !isLocked;
        }
    }
}
