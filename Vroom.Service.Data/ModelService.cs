using AutoMapper;
using Bytes2you.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Models;
using Vroom.Service.Contracts;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Service.Data
{
    public class ModelService : IModelService
    {
        private readonly IEfDbRepository<Model> modelRepository;
        private readonly IMapper mapper;
        private readonly IVroomDbContextSaveChanges vroomDbContextSaveChanges;
        public ModelService(IEfDbRepository<Model> modelRepository, IMapper mapper, IVroomDbContextSaveChanges vroomDbContextSaveChanges)
        {
            Guard.WhenArgument<IEfDbRepository<Model>>(modelRepository, GlobalConstants.ModelRepositoryNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IVroomDbContextSaveChanges>(vroomDbContextSaveChanges, GlobalConstants.DbContextSaveChangesNullExceptionMessege).IsNull().Throw();

            this.modelRepository = modelRepository;
            this.mapper = mapper;
            this.vroomDbContextSaveChanges = vroomDbContextSaveChanges;
        }
        public void Add(ModelServiceModel model)
        {
            Guard.WhenArgument<ModelServiceModel>(model, GlobalConstants.GetMemberName(() => model)).IsNull().Throw();

            var modelDb = this.mapper.Map<ModelServiceModel, Model>(model);
            this.modelRepository.Add(modelDb);
            this.vroomDbContextSaveChanges.SaveChanges();
        }

        public void Delete(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            var model = this.modelRepository.GetById(id);
            if(model == null)
            {
                return;
            }
            model.IsDeleted = true;
            this.vroomDbContextSaveChanges.SaveChanges();
        }

        public bool DoesExist(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            return this.modelRepository.All().Any(x => !x.IsDeleted && x.Id == id);
        }

        public IEnumerable<ModelServiceModel> GetAll()
        {
            return this.mapper.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(this.modelRepository.All().ToList());
        }

        public IEnumerable<ModelServiceModel> GetAll(int makeId)
        {
            Guard.WhenArgument<int>(makeId, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            return this.mapper.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(this.modelRepository.All().Where(x => x.MakeId == makeId).ToList());
        }

        public ModelServiceModel GetById(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();
            return this.mapper.Map<Model, ModelServiceModel>(this.modelRepository.GetById(id));
        }

        public void Update(int id, string name, int makeId)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();
            Guard.WhenArgument<string>(name, GlobalConstants.GetMemberName(() => name)).IsNullOrEmpty().Throw();
            Guard.WhenArgument<int>(makeId, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            var model = this.modelRepository.GetById(id);
            if(model == null)
            {
                return;
            }

            model.MakeId = makeId;
            model.Name = name;
            this.vroomDbContextSaveChanges.SaveChanges();
        }
    }
}
