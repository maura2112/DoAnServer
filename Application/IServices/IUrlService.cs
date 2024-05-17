using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IUrlService
    {
        Task<string> Add(UrlRecord url);
        Task<UrlRecord> GetByPath(string path);
        Task<List<UrlRecord>> GetUrlRecords();
    }
}
