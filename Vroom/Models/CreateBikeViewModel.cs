using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Vroom.Models
{
    public class CreateBikeViewModel
    {
        public BikeViewModel Bike { get; set; }


        public IEnumerable<SelectListItem> Makes { get; set; }

        public IEnumerable<SelectListItem> Models { get; set; }

        public IEnumerable<Currency> Currencies { get; set; }

        private List<Currency> cList = new List<Currency>();

        private List<Currency> CreateClist()
        {
            this.cList.Add(new Currency("USD", "USD"));
            this.cList.Add(new Currency("BGN", "BGN"));
            this.cList.Add(new Currency("EUR", "EUR"));
            return this.cList;
        }

        public CreateBikeViewModel()
        {
            this.Currencies = this.CreateClist();
        }
    }

    public class Currency
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Currency(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
