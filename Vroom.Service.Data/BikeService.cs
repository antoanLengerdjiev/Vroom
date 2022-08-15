using AutoMapper;
using Bytes2you.Validation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Service.Contracts;
using Vroom.Service.Models;

namespace Vroom.Service.Data
{
    public class BikeService : IBikeService
    {
        private readonly IEfDbRepository<Bike> bikeRepository;
        private readonly IMapper mapper;
        private readonly IVroomDbContextSaveChanges vroomDbContextSaveChanges;
        public BikeService(IEfDbRepository<Bike> bikeRepository, IMapper mapper, IVroomDbContextSaveChanges vroomDbContextSaveChanges)
        {
            Guard.WhenArgument<IEfDbRepository<Bike>>(bikeRepository, GlobalConstants.BikeRepositoryNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IVroomDbContextSaveChanges>(vroomDbContextSaveChanges, GlobalConstants.DbContextSaveChangesNullExceptionMessege).IsNull().Throw();

            this.bikeRepository = bikeRepository;
            this.mapper = mapper;
            this.vroomDbContextSaveChanges = vroomDbContextSaveChanges;
        }

        public int Add(BikeServiceModel model)
        {
            Guard.WhenArgument<BikeServiceModel>(model, GlobalConstants.GetMemberName(() => model)).IsNull().Throw();

            var modelDb = this.mapper.Map<BikeServiceModel, Bike>(model);
            this.bikeRepository.Add(modelDb);
            this.vroomDbContextSaveChanges.SaveChanges();

            return modelDb.Id;
        }

        public void Delete(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();
            var model = this.bikeRepository.GetById(id);
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

            return this.bikeRepository.All().Any(x => x.Id == id);
        }

        public BikeServiceModel GetById(int id)
        {
            Guard.WhenArgument<int>(id, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();

            return this.mapper.Map<Bike,BikeServiceModel>(this.bikeRepository.GetById(id));
        }

        public PagedBikeServiceModel GetFilteredBikes(int pageNumber, int pageSize, string searchString, string sortOrder)
        {
            int excludeRecords = (pageNumber * pageSize) - pageSize;
            var bikes = bikeRepository.All();

            if (!string.IsNullOrEmpty(searchString))
            {
                bikes = bikes.Where(x => x.Model.Make.Name.ToLower().Contains(searchString.ToLower()));
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                bikes = bikes.OrderBy(x => x.Price);
            }
            else
            {
                bikes = bikes.OrderByDescending(x => x.Price);
            }

            var result = new PagedBikeServiceModel { TotalSize = bikes.Count() };
            result.Bikes = this.mapper.Map<IEnumerable<Bike>, IEnumerable<BikeServiceModel>>(bikes.Skip(excludeRecords).Take(pageSize).ToList());

            return result;
        }

        public void Update(BikeServiceModel bikeServiceModel)
        {
            Guard.WhenArgument<BikeServiceModel>(bikeServiceModel, GlobalConstants.GetMemberName(() => bikeServiceModel)).IsNull().Throw();

            var dbModel = this.bikeRepository.GetById(bikeServiceModel.Id);

            dbModel.Currency = bikeServiceModel.Currency;
            dbModel.ImagePath = bikeServiceModel.ImagePath;
            dbModel.Mileage = bikeServiceModel.Mileage;
            dbModel.ModelId = bikeServiceModel.ModelId;
            dbModel.Price = bikeServiceModel.Price;
            dbModel.SellerId = bikeServiceModel.SellerId;
            dbModel.Year = bikeServiceModel.Year;
            dbModel.Features = bikeServiceModel.Features;
            this.vroomDbContextSaveChanges.SaveChanges();
        }

        public void UpdateImg(int newBikeId, string relativeImagePath)
        {
            Guard.WhenArgument<int>(newBikeId, GlobalConstants.CannotBeZero).IsLessThanOrEqual(GlobalConstants.Zero).Throw();
            Guard.WhenArgument<string>(relativeImagePath, GlobalConstants.GetMemberName(() => relativeImagePath)).IsNullOrEmpty().Throw();

            var bike = this.bikeRepository.GetById(newBikeId);
            bike.ImagePath = relativeImagePath;
            this.vroomDbContextSaveChanges.SaveChanges();
        }
    }
}
