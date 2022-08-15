using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vroom.Data.Models.Base;

namespace Vroom.Models
{
    public class Model : BaseModel<int>, IAuditInfo, IDeletableEntity
    {

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey("Make")]
        public int MakeId { get; set;}
        public virtual Make Make { get; set; }
    }
}
