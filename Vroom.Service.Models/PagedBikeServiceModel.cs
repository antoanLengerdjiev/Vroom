using System.Collections.Generic;

namespace Vroom.Service.Models
{
    public class PagedBikeServiceModel
    {
        public IEnumerable<BikeServiceModel> Bikes { get; set; }

        public int TotalSize { get; set; }
    }
}
