using BE.LocalAccountabilitySystem.Entities.Database.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.LocalAccountabilitySystem.Entities.Database
{
    public class SystemStatus
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Value { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedDate { get; set; }
    }
}
