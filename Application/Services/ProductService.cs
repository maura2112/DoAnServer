using Application.DTOs;
using Application.DTOs;
using Application.DTOs.Favorite;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Azure.Core;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Application.Common.ProjectStatus;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper ;
        private readonly IProductRepository _productRepository;
        private readonly IUrlRepository _urlRepository;
        private readonly ApplicationDbContext _context;
        public ProductService(IMapper mapper, IProductRepository productRepository, IUrlRepository urlRepository, ApplicationDbContext context)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _urlRepository = urlRepository;
            _context = context;
        }

        public async Task<int> Add(ProductDTO request)
        {
            var product = _mapper.Map<Product>(request);
            product.DateCreated = DateTime.UtcNow;
            product.DateUpdated = DateTime.UtcNow;
            await _productRepository.AddAsync(product);
            var urlRecord = product.CreateUrlRecordAsync("san-pham" , product.Title);
            await _urlRepository.AddAsync(urlRecord);
            return product.Id;
        }

        public async Task<int> Delete(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id.ToString().Equals(id));
            _productRepository.Delete(product);
            return product.Id;
        }






        public async Task<Pagination<ProductDTO>> Get(int pageIndex, int pageSize)
        {
            var products = await _productRepository.ToPagination(pageIndex, pageSize);
            var productDTOs = _mapper.Map<Pagination<ProductDTO>>(products);
            return productDTOs;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id.ToString().Equals(id));
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }



        public async Task<Pagination<ProductDTO>> GetWithFilter(Expression<Func<Product, bool>> filter, int pageIndex, int pageSize)
        {
            var products = await _productRepository.GetAsync(filter, pageIndex, pageSize);

            var productDTOs =  _mapper.Map<Pagination<ProductDTO>>(products);
            return productDTOs;
        }

        public async Task<int> Update(ProductDTO request)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id.ToString().Equals(request.Id));
            _productRepository.Update(product);
            return product.Id;
        }
    }
}
