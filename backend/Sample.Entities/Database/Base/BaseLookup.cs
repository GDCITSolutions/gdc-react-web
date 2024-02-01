using System.ComponentModel.DataAnnotations;

namespace BE.LocalAccountabilitySystem.Entities.Database.Base
{
    public class BaseLookup : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Value { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }
    }
}
