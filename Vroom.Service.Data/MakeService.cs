using System;
using System.Collections.Generic;
using Vroom.Data.Contracts;
using AutoMapper;
using Vroom.Models;
using System.Linq;
using Vroom.Data.Common;
using Vroom.Service.Models.Contracts;
using Vroom.Service.Models;
using Vroom.Service.Contracts;
using Bytes2you.Validation;
using Vroom.Common;

namespace Vroom.Service.Data
{
    public class MakeService : IMakeService
    {
        private readonly IEfDbRepository<Make> makeRepository;
        private readonly IMapper mapper;
        private readonly IVroomDbContextSaveChanges vroomDbContextSaveChanges;
        public MakeService(IEfDbRepository<Make> makeRepository, IMapper mapper, IVroomDbContextSaveChanges vroomDbContextSaveChanges)
        {
            Guard.WhenArgument<IEfDbRepository<Make>>(makeRepository, GlobalConstants.MakeRepositoryNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IVroomDbContextSaveChanges>(vroomDbContextSaveChanges, GlobalConstants.DbContextSaveChangesNullExceptionMessege).IsNull().Throw();

            this.makeRepository = makeRepository;
            this.mapper = mapper;
            this.vroomDbContextSaveChanges = vroomDbContextSaveChanges;
        }
        public void Add(MakeServiceModel model)
        {
            Guard.WhenArgument<MakeServiceModel>(model, GlobalConstants.GetMemberName(() => model)).IsNull().Throw();

            var make = this.mapper.Map<MakeServiceModel, Make>(model);
            this.makeRepository.Add(make);
            this.vroomDbContextSaveChanges.SaveChanges();
        }

        public void Delete(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            var make = this.makeRepository.GetById(id);
            if (make == null)
            {
                return;
            }

            make.IsDeleted = true;
            this.vroomDbContextSaveChanges.SaveChanges();
        }

        public bool DoesExist(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            return this.makeRepository.All().Any(x => x.Id == id && !x.IsDeleted);
        }

        public IEnumerable<MakeServiceModel> GetAll()
        {
            return this.mapper.Map<IEnumerable<Make>, IEnumerable<MakeServiceModel>>(this.makeRepository.All().Where(x => !x.IsDeleted).ToList());
        }

        public IMakeServiceModel GetById(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            return this.mapper.Map<Make, MakeServiceModel>(this.makeRepository.GetById(id));
        }

        public void UpdateName(int id, string name)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();
            Guard.WhenArgument<string>(name, GlobalConstants.GetMemberName(() => name)).IsNullOrEmpty().Throw();


            var make = this.makeRepository.GetById(id);
            if (make == null)
            {
                return;
            }

            make.Name = name;
            this.vroomDbContextSaveChanges.SaveChanges();
        }
    }
}
