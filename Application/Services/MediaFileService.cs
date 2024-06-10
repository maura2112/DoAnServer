using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MediaFileService : IMediaFileService
    {
        public readonly IMediaFileRepository _mediaFileRepository;
        public MediaFileService(IMediaFileRepository mediaFileRepository)
        {
            _mediaFileRepository = mediaFileRepository;
        }
        public async Task<MediaFileDTO> AddMediaFile(MediaFileDTO mediaFile)
        {
            var media = new MediaFile()
            {
                FileName = mediaFile.FileName,
                UserId = mediaFile.UserId,
                CreateAt = DateTime.Now,
                Description = mediaFile.Description,
                Title = mediaFile.Title,
            };
            await _mediaFileRepository.AddAsync(media);
            return mediaFile;
        }
    }
}
