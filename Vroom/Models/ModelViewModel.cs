using Vroom.Models.Contracts;

namespace Vroom.Models
{
    public class ModelViewModel : IStringIdable, IStringMakeIdable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string MakeId { get; set;}

        public MakeViewModel Make { get; set; }
    }
}
