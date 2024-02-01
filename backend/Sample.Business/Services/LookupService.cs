using BE.LocalAccountabilitySystem.Common.Enum;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Database;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Business.Services
{
    /// <summary>
    /// Provide lookups from the database
    /// </summary>
    public interface ILookupService 
    {
        /// <summary>
        /// Get all active roles
        /// </summary>
        /// <returns></returns>
        Task<IList<Role>> GetRoles();

        /// <summary>
        /// Get system statuses
        /// </summary>
        /// <returns></returns>
        Task<IList<SystemStatus>> GetSystemStatuses();
    }

    public class LookupService : ILookupService
    {
        private readonly SampleContext _context;

        public LookupService(SampleContext context) 
        {
            Util.Guard.ArgumentsAreNotNull(context);

            _context = context;
        }

        public async Task<IList<Role>> GetRoles()
        {
            return await _context.Role
                .Where(x => x.SystemStatusId == SystemStatusEnum.Active.AsInt())
                .ToListAsync();
        }

        public async Task<IList<SystemStatus>> GetSystemStatuses()
        {
            return await _context.SystemStatus
                .ToListAsync();
        }
    }
}
