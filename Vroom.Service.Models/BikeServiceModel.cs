using Vroom.Service.Models.Contracts;

namespace Vroom.Service.Models
{
    public class BikeServiceModel : IIntIdalbe , IIntMakeIdable, IIntModelIdable
    {
        public int Id { get; set; }

        public int MakeId { get; set; }
        public MakeServiceModel Make { get; set; }

        public int ModelId { get; set; }

        public ModelServiceModel Model { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }
        public string Features { get; set; }


        public string SellerId { get; set; }
        public ApplicationUserServiceModel Seller { get; set; }

        public int Price { get; set; }

        public string Currency { get; set; }

        public string ImagePath { get; set; }
    }
}
