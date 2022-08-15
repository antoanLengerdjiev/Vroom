using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Data.Models.Base;

namespace Vroom.Tests.Data.Mocks
{
    public class MockedModel : BaseModel<int>, IAuditInfo, IDeletableEntity
    {
        public string Name { get; set; }
    }
}
