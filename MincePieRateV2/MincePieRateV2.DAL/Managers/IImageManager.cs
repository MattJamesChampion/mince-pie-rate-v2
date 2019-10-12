using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MincePieRateV2.DAL.Managers
{
    public interface IImageManager
    {
        Task<Guid> AddImageAsync(IFormFile formFile);
        Task<IFormFile> GetImageAsync(Guid guid);
    }
}
