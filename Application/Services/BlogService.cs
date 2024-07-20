using Application.Common;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Migrations;
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
                        where b.IsDeleted != true
                        select new BlogDTO
                        {
                            BlogId = b.Id,
                            Title = b.Title,
                            ShortDesction = b.ShortDescription,
                            Description = b.Description,
                            CategoryId = b.CategoryId,
                            UserId = b.CreatedBy,
                            CategoryName = c.CategoryName,
                            Author = u.Name,
                            IsHomePage = b.IsHomePage,
                            IsHot = b.IsHot,
                            IsPublished = b.IsPublished,
                            BlogImage = b.BlogImage,
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

        public async Task<BlogDTO> GetBlogDTOAsync(int id)
        {
            var blogDTO = await (from b in _context.Blogs
                         join c in _context.Categories on b.CategoryId equals c.Id
                         join u in _context.Users on b.CreatedBy equals u.Id
                         where b.Id == id
                         select new BlogDTO
                         {
                             BlogId = b.Id,
                             Title = b.Title,
                             ShortDesction = b.ShortDescription,
                             Description = b.Description,
                             CategoryId = b.CategoryId,
                             UserId = b.CreatedBy,
                             IsPublished = b.IsPublished,
                             IsHomePage = b.IsHomePage,
                             IsHot= b.IsHot,
                             CategoryName = c.CategoryName,
                             Author = u.Name,
                             BlogImage = b.BlogImage,
                             CreateDate = b.CreatedDate,
                             CreateTime = DateTimeHelper.ToVietnameseDateString(b.CreatedDate),
                         }).FirstOrDefaultAsync();
            return blogDTO;
        }

        public async Task<BlogCreateDTO> UpdateBlog(BlogCreateDTO dto)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x=>dto.Id == x.Id);
            if(blog == null)
            {
                return null;
            }
            blog.UpdatedDate = DateTime.UtcNow;
            blog.Title = dto.Title;
            blog.BlogImage = dto.BlogImage;
            blog.IsPublished = dto.IsPublished;
            blog.Description = dto.Description;
            blog.ShortDescription = dto.ShortDescription;
            blog.IsHomePage = dto.IsHomePage;
            blog.IsHot = dto.IsHot;
            blog.CategoryId = dto.CategoryId;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteBlog(int id)
        {
            var blog =await _context.Blogs.FirstOrDefaultAsync(x=>x.Id == id && x.IsDeleted != true);
            if(blog == null)
            {
                return false;
            }
            blog.IsDeleted = true;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> PublishBlog(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x=>x.Id == id);
            if(blog == null)
            {
                return 0;
            }
            blog.IsPublished = !blog.IsPublished;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return blog.Id;
        }

        public async Task<List<BlogDTO>> GetBlogList(BlogFilter filter)
        {
            var query = (from b in _context.Blogs
                         join c in _context.Categories on b.CategoryId equals c.Id
                         join u in _context.Users on b.CreatedBy equals u.Id
                         where b.IsDeleted != true && b.IsPublished == true
                         select new BlogDTO
                         {
                             BlogId = b.Id,
                             Title = b.Title,
                             Description = b.Description,
                             ShortDesction = b.ShortDescription,
                             CategoryId = b.CategoryId,
                             UserId = b.CreatedBy,
                             CategoryName = c.CategoryName,
                             Author = u.Name,
                             IsPublished = b.IsPublished,
                             IsHomePage = b.IsHomePage,
                             IsHot = b.IsHot,
                             BlogImage = b.BlogImage,
                             CreateDate = b.CreatedDate,
                             CreateTime = DateTimeHelper.ToVietnameseDateString(b.CreatedDate),
                         });
            if(filter.IsHot != null)
            {
                query = query.Where(x=>x.IsHot == filter.IsHot);
            }

            if (filter.IsHomePage != null)
            {
                query = query.Where(x => x.IsHomePage == filter.IsHomePage);
            }

            query = query.OrderByDescending(x=>x.CreateDate).Take(filter.Top);
            var list = await query.ToListAsync();
            return list;
        }
    }
}
