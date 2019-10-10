using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MincePieRateV2.DAL.Managers
{
    public class FileSystemImageManager : IImageManager
    {
        public async Task<Guid> AddImageAsync(IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteImageAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<IFormFile> GetImageAsync(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
