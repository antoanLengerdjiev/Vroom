using System.Collections.Generic;

namespace Vroom.Service.Models
{
    public class PagedUserServiceModel
    {
        public IEnumerable<ApplicationUserServiceModel> Users { get; set; }

        public int TotalSize { get; set; }
    }
}
