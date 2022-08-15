using System.Collections.Generic;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Service.Contracts
{
    public interface IModelService
    {
        IEnumerable<ModelServiceModel> GetAll();

        void Add(ModelServiceModel model);
        ModelServiceModel GetById(int id);
        bool DoesExist(int id);
        void Delete(int id);
        void Update(int id, string name, int makeId);
        IEnumerable<ModelServiceModel> GetAll(int makeId);
    }
}
