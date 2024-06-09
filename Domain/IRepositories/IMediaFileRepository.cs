using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IMediaFileRepository : IGenericRepository<MediaFile>
    {
        Task<List<MediaFile>> GetByUserId(int UserId);
        Task<string> UploadAsync(IFormFile mediaFile);
    }
}
