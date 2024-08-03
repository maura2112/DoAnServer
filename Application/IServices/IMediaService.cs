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

        public Task<MediaFileDTO> AddMediaFile(MediaFileDTO mediaFile);

        public Task<MediaFileDTO> GetById(long id);
        public Task<MediaFileDTO> UpdateMediaFile(MediaFileDTO mediaFile);

        public Task<long> DeleteMediaFile(long id);

    }
}
