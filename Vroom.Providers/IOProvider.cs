using Microsoft.AspNetCore.Http;
using System.IO;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class IOProvider : IIOProvider
    {
        public string SaveImage(int modelDbId, string wwrootPath, IFormFileCollection files)
        {
            var imgPath = @"images\bike\";
            var Extension = Path.GetExtension(files[0].FileName);
            var RelativeImagePath = imgPath + modelDbId + Extension;
            var AbsImagePath = Path.Combine(wwrootPath, RelativeImagePath);

            using (var filestream = new FileStream(AbsImagePath, FileMode.Create))
            {
                files[0].CopyTo(filestream);
            }

            return RelativeImagePath;
        }
    }
}
