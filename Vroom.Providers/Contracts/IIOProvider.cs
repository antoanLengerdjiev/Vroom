using Microsoft.AspNetCore.Http;


namespace Vroom.Providers.Contracts
{
    public interface IIOProvider
    {
        string SaveImage(int modelDbId, string wwrootPath, IFormFileCollection files);
    }
}
