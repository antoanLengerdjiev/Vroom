using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Models.Contracts
{
    public interface IStringModelIdable
    {
        public string ModelId { get; set; }
    }
}
