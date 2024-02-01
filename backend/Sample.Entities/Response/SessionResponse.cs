using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Database;

namespace BE.LocalAccountabilitySystem.Entities.Response
{
    public class SessionResponse
    {
        #region Constructors

        public SessionResponse() { }

        public SessionResponse(User user, IList<UserToRole> roles)
        {
            Util.Guard.ArgumentIsNotNull(user, "user");

            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            EmailAddress = user.EmailAddress;
            Roles = roles;
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }

        public int SchoolDistrictId { get; set; }

        public int SchoolId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public IList<UserToRole> Roles { get; set; }

        #endregion
    }
}
