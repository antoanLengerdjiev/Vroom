namespace Vroom.Service.Models.Contracts
{
    public interface IModelServiceModel : IIntIdalbe
    { 

        public string Name { get; set; }

        public int MakeId { get; set; }

        public MakeServiceModel Make { get; set; }
    }
}
