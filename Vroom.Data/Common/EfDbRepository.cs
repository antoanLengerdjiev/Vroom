using Bytes2you.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Vroom.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models.Base;

namespace Vroom.Data.Common
{
    public class EfDbRepository<T> : IEfDbRepository<T> where T : class, IAuditInfo, IDeletableEntity
    {
        
        public EfDbRepository(IVroomDbContext context)
        {
            Guard.WhenArgument<IVroomDbContext>(context, GlobalConstants.DbContextNullExceptionMessege)
                 .IsNull()
                 .Throw();

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        private DbSet<T> DbSet { get; }

        private IVroomDbContext Context { get; }

        public IQueryable<T> All()
        {
            return this.DbSet.Where(x => !x.IsDeleted);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return this.DbSet;
        }

        public T GetById(object id)
        {
            if (id == null)
            {
                return null;
            }

            var item = this.DbSet.Find(id);
            if (item.IsDeleted)
            {
                return null;
            }

            return item;
        }

        public void Add(T entity)
        {
            Guard.WhenArgument<T>(entity, GlobalConstants.EntityToAddNullExceptionMessage).IsNull().Throw();

            this.DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            Guard.WhenArgument<T>(entity, GlobalConstants.EntityToDeleteNullExceptionMessage).IsNull().Throw();
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
        }

        public void HardDelete(T entity)
        {
            Guard.WhenArgument<T>(entity, GlobalConstants.EntityToHardDeleteNullExceptionMessage).IsNull().Throw();
            this.DbSet.Remove(entity);
        }
    }
}
