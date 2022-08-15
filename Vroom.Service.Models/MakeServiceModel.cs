using Vroom.Service.Models.Contracts;

namespace Vroom.Service.Models
{
    public class MakeServiceModel : IMakeServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
