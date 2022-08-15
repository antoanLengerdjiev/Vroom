using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Extensions
{
    public class YearRangeTillDateAttribute : RangeAttribute
    {
        public YearRangeTillDateAttribute(int startYear) : base(startYear, DateTime.Today.Year)
        {
        }

    }
}
