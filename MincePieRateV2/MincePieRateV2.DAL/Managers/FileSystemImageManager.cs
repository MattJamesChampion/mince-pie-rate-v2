using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MincePieRateV2.CrossCutting.ImageHandling.Extensions;

namespace MincePieRateV2.DAL.Managers
{
    public class FileSystemImageManager : IImageManager
    {
        public async Task<Guid> AddImageAsync(IFormFile formFile)
        {
            if (formFile == null || !formFile.IsImage())
            {
                return Guid.Empty;
            }

            var imageOutputDirectory = @"C:\Temp\MincePieRateV2";
            Directory.CreateDirectory(imageOutputDirectory);

            var guid = Guid.NewGuid();
            var filePath = Path.Combine(imageOutputDirectory, guid.ToString() + formFile.GetFileExtension());

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return guid;
        }

        public async Task<string> GetImagePathAsync(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
