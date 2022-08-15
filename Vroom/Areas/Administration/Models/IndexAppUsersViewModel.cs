
using System.Collections.Generic;

namespace Vroom.Areas.Administration.Models
{
    public class IndexAppUsersViewModel
    {
        public IEnumerable<AppUserViewModel> Admins { get; set; }

        public IEnumerable<AppUserViewModel> Users { get; set; }
    }
}
