using Application.DTOs;
using Application.DTOs;
using Application.IServices;
using Application.Repositories;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaFileRepository _mediaRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public MediaService(IMediaFileRepository mediaRepository, ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _mediaRepository = mediaRepository;
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<MediaFileDTO>> GetByUserId(int uid)
        {
            var mediaDTOs = new List<MediaFileDTO>();
            var medias = await _mediaRepository.GetByUserId(uid);
            if (medias.Any())
            {
                mediaDTOs = _mapper.Map<List<MediaFileDTO>>(medias);
            }
            return mediaDTOs;
        }


        public Task<string> UploadAsync(IFormFile request)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaFileDTO> AddMediaFile(MediaFileDTO mediaFile)
        {
            var userId = _currentUserService.UserId;
            var media = new MediaFile()
            {
                FileName = mediaFile.FileName,
                UserId = userId,
                CreateAt = DateTime.Now,
                Description = mediaFile.Description,
                Title = mediaFile.Title,
            };
            await _mediaRepository.AddAsync(media);
            return mediaFile;
        }

        public async Task<MediaFileDTO> UpdateMediaFile(MediaFileDTO mediaFile)
        {
            var userId = _currentUserService.UserId;
            var media = await _mediaRepository.GetByIdAsync(mediaFile.Id);
            if(userId != media.UserId)
            {
                return null;
            }
            media.Description = mediaFile.Description;
            media.Title = mediaFile.Title;
            media.FileName = mediaFile.FileName;
            _mediaRepository.Update(media);
            return mediaFile;
        }

        public async Task<long> DeleteMediaFile(long id)
        {
             await _mediaRepository.Delete(id);
            return id;
        }

        public async Task<MediaFileDTO> GetById(long id)
        {
            var media = await _mediaRepository.GetByIdAsync(id);
            if ( media == null)
            {
                return null;
            }
           var mediaDTO = _mapper.Map<MediaFileDTO>(media);
            return mediaDTO;
        }
    }
}
