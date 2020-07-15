using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MincePieRateV2.CrossCutting.ImageHandling.Extensions;

namespace MincePieRateV2.DAL.Managers
{
    public class FileSystemImageManager : IImageManager
    {
        private readonly string imageOutputDirectory = @"wwwroot\images";
        public async Task<Guid> AddImageAsync(IFormFile formFile)
        {
            if (formFile == null || !formFile.IsImage())
            {
                throw new ArgumentException("Submitted file is either null or not an image");
            }
            
            Directory.CreateDirectory(imageOutputDirectory);

            var guid = Guid.NewGuid();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageOutputDirectory, guid.ToString() + formFile.GetFileExtension());

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return guid;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> GetImagePathAsync(Guid guid)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (guid != Guid.Empty)
            {
                var absoluteFilePath = Directory.EnumerateFiles(imageOutputDirectory, guid.ToString() + ".*").FirstOrDefault();
                if (!string.IsNullOrEmpty(absoluteFilePath))
                {
                    var fileName = Path.GetFileName(absoluteFilePath);
                    return Path.Combine(@"\images", fileName);
                }
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
