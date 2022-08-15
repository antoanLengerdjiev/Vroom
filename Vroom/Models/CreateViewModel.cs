using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vroom.Models.Contracts;

namespace Vroom.Models
{
    public class CreateViewModel : IStringIdable, IStringMakeIdable
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string MakeId { get; set; }

        [Display(Name ="Makes")]
        public IEnumerable<SelectListItem> MakeViewModels { get; set; }

        public IEnumerable<SelectListItem> CSelectListItem(IEnumerable<MakeViewModel> makeViewModels)
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
                    Text = item.Name,
                    Value = item.Id
                });
            }

            return selectListItems;
        }
    }
}
