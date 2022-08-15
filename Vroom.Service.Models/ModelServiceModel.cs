using Vroom.Service.Models.Contracts;

namespace Vroom.Service.Models
{
    public class ModelServiceModel : IModelServiceModel, IIntMakeIdable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MakeId { get; set; }
        public MakeServiceModel Make { get; set; }
    }
}
