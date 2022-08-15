using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Vroom.Models.Factories.NewFolder
{
    public interface ISelectedItemFactory
    {
        IEnumerable<SelectListItem> GetSelectList<T>(IEnumerable<T> ts, string selectedId = "0");
    }
}
