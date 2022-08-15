using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vroom.Data.Models.Base;

namespace Vroom.Models
{
    public class Make : BaseModel<int>, IAuditInfo, IDeletableEntity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
