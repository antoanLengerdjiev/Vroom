using System.ComponentModel.DataAnnotations;
using Vroom.Extensions;
using Vroom.Models.Contracts;

namespace Vroom.Models
{
    public class BikeViewModel : IStringIdable, IStringMakeIdable, IStringModelIdable
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Provide Make")]
        //[RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Select Make")]
        public string MakeId { get; set; }
        public MakeViewModel Make { get; set; }

        [Required(ErrorMessage = "Provide Model!")]
        //[RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Select Model")]
        public string ModelId { get; set; }

        public ModelViewModel Model { get; set; }

        [Required(ErrorMessage = "Provide Year!")]
        [YearRangeTillDate(2000, ErrorMessage = "Year must be between {1} and {2}")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Provide Mileage!")]
        [Range(1,int.MaxValue, ErrorMessage = "Mileage must be between {1} and {2}")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Provide Features!")]
        public string Features { get; set; }


        public string SellerId { get; set; }
        public ApplicationUserViewModel Seller { get; set; }

        [Required(ErrorMessage = "Provide  Price!")]
        [Range(100, int.MaxValue, ErrorMessage = "Mileage must be between {1} and {2}")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Provide Currency!")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Select Currency!")]
        public string Currency { get; set; }
        
        public string ImagePath { get; set; }
    }
}
