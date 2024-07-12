using Application.Common;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public BlogService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Blog> CreateBlog(BlogCreateDTO dto)
        {
            var blog = _mapper.Map<Blog>(dto);
            blog.CreatedDate = DateTime.Now;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<Pagination<BlogDTO>> GetBlogs(BlogSearch search )
        {
            var query = (from b in _context.Blogs
                        join c in _context.Categories on b.CategoryId equals c.Id
                        join u in _context.Users on b.CreatedBy equals u.Id
                        select new BlogDTO
                        {
                            Title = b.Title,
                            Description = b.Description,
                            CategoryId = b.CategoryId,
                            UserId = b.CreatedBy,
                            CategoryName = c.CategoryName,
                            Author = u.Name,
                            CreateDate = b.CreatedDate,
                            CreateTime = DateTimeHelper.ToVietnameseDateString(b.CreatedDate),
                        });

            if(search.AuthorName != null )
            {
                query = query.Where(x => x.Author.ToLower().Contains(search.AuthorName));
            }

            var filter = PredicateBuilder.True<BlogDTO>();
            if(search.CategoryId != null) {
                filter = filter.And(item => item.CategoryId == search.CategoryId);
            }
            if (search.Title != null)
            {
                filter = filter.And(item => item.Title.ToLower().Contains(search.Title.ToLower()));
            }
            if (search.Description != null)
            {
                filter = filter.And(item => item.Description.ToLower().Contains(search.Description.ToLower()));
            }
            query = query.Where(filter);
            var totalItem = await query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToListAsync();
            var result = new Pagination<BlogDTO>()
            {
                PageSize = search.PageSize,
                PageIndex = search.PageIndex,
                TotalItemsCount = query.Count(),
                Items = totalItem,
            };
            return result;
        }
    }
}
