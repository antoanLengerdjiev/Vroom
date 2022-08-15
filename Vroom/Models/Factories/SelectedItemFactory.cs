using Bytes2you.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Vroom.Common;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;

namespace Vroom.Models.Factories
{
    public class SelectedItemFactory : ISelectedItemFactory
    {
        private readonly IEncryptionProvider encryptionProvider;
        public SelectedItemFactory(IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            this.encryptionProvider = encryptionProvider;
        }
        public IEnumerable<SelectListItem> GetSelectList<T>(IEnumerable<T> ts, string selectedId = "0")
        {
            var realId = this.encryptionProvider.Decrypt(selectedId);

            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "----Select----",
                    Value = "0"
                }
            };
            int itemListId;
            bool isSelected;
            foreach (var item in ts)
            {
                itemListId = this.encryptionProvider.Decrypt(item.GetType().GetProperty("Id").GetValue(item, null).ToString());
                isSelected = itemListId == realId;
                selectListItems.Add(new SelectListItem()
                {
                    Text = item.GetType().GetProperty("Name").GetValue(item, null).ToString(),
                    Value = isSelected? selectedId:item.GetType().GetProperty("Id").GetValue(item, null).ToString(),
                    Selected = isSelected
                });
            }

            return selectListItems;
        }
    }
}

