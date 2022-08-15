using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectItemsList<T>(this IEnumerable<T> makeViewModels, string id = "0")
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "----Select----",
                    Value = "0"
                }
            };

            foreach (var item in makeViewModels)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Text = item.GetType().GetProperty("Name").GetValue(item, null).ToString(),
                    Value = item.GetType().GetProperty("Id").GetValue(item, null).ToString(),
                    Selected = item.GetType().GetProperty("Id").GetValue(item, null).ToString() == id
                });
            }

            return selectListItems;
        }
    }
}
