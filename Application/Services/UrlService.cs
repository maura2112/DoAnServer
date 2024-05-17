using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }
        public async Task<string> Add(UrlRecord url)
        {
            await _urlRepository.AddAsync(url);
            return url.Slug;
        }

        public async Task<UrlRecord> GetByPath(string path)
        {
            var urlRecords = await GetUrlRecords();
            var url = urlRecords.FirstOrDefault(x=>x.Slug.Equals(path));
            return url;
        }

        public async Task<List<UrlRecord>> GetUrlRecords()
        {
            var urlRecords = await _urlRepository.GetAll();
            return urlRecords;
        }
    }
}
