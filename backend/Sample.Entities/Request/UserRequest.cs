using System.ComponentModel.DataAnnotations;

namespace BE.LocalAccountabilitySystem.Entities.Request
{
    public class UserRequest
    {
        public int Id { get; set; } = 0;

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public int SystemStatusId { get; set; }

        public IList<int> RoleIds { get; set; }
    }
}
