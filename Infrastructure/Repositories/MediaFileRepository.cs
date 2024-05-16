
using Application.Repositories;
using Azure.Core;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class MediaFileRepository : GenericRepository<MediaFile>, IMediaFileRepository
    {
        private readonly ApplicationDbContext _context;
        public MediaFileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<string> UploadAsync(IFormFile fileMedia)
        {
            var extension = Path.GetExtension(fileMedia.FileName);
            var folder = "";
            if (Media.extensionFiles.Contains(extension))
            {
                folder = "FilesFolder";
            }
            else if (Media.extensionImages.Contains(extension))
            {
                folder = "ImageFolder";
            }
            else
            {
                return null;
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), folder, fileMedia.FileName);

            using (var file = new FileStream(path, FileMode.Create))
            {
                await fileMedia.CopyToAsync(file);
            }
            return fileMedia.FileName;
        }
    }
}
