using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vroom.Data.Models.Base
{
    public abstract class BaseModel<TKey> : IAuditInfo, IDeletableEntity
    {

        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
