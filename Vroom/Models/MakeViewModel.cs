using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vroom.Models.Contracts;

namespace Vroom.Models
{
    public class MakeViewModel : IStringIdable
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
