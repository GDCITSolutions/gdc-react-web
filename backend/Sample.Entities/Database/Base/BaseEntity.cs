using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.LocalAccountabilitySystem.Entities.Database.Base
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("SystemStatus")]
        public int SystemStatusId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedDate { get; set; }

        #region Linked Entities

        public SystemStatus SystemStatus { get; set; }

        #endregion
    }
}
