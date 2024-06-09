using Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IMediaService
    {
        Task<string> UploadAsync(IFormFile request, CancellationToken token);

        Task<List<MediaFileDTO>> GetByUserId( int uid);
    }
}
