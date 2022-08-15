using System;
using System.Collections.Generic;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Service.Contracts
{
    public interface IMakeService
    {
        IEnumerable<MakeServiceModel> GetAll();

        void Add(MakeServiceModel model);
        IMakeServiceModel GetById(int id);
        void UpdateName(int id, string name);
        bool DoesExist(int id);
        void Delete(int id);
    }
}
