using System.Linq;
using Vroom.Data.Models.Base;

namespace Vroom.Data.Contracts
{
    public interface IEfDbRepository<T> where T : class, IAuditInfo, IDeletableEntity
    {
        IQueryable<T> All();

        IQueryable<T> AllWithDeleted();

        T GetById(object id);

        void Add(T entity);

        void Delete(T entity);

        void HardDelete(T entity);
    }
}
