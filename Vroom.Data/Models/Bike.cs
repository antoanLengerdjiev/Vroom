using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Data.Models.Base;
using Vroom.Models;

namespace Vroom.Data.Models
{
    public class Bike : BaseModel<int>, IAuditInfo, IDeletableEntity
    {

        public int ModelId { get; set; }
        public virtual Model Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Mileage { get; set; }

        public string Features { get; set; }

        public string SellerId { get; set; }

        public virtual ApplicationUser Seller { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Currency { get; set; }

        public string ImagePath { get; set; }

    }


}
