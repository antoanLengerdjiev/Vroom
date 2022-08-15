using Vroom.Service.Models;

namespace Vroom.Service.Contracts
{
    public interface IBikeService
    {
        PagedBikeServiceModel GetFilteredBikes(int pageNumber, int pageSize, string searchString, string sortOrder);
        BikeServiceModel GetById(int id);

        int Add(BikeServiceModel model);
        void UpdateImg(int newBikeId, string relativeImagePath);
        bool DoesExist(int id);
        void Delete(int id);
        void Update(BikeServiceModel bikeServiceModel);
    }
}
