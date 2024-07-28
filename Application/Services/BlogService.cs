using Application.Common;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
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
            blogDTO.relateds = await RelatedBlogs(blogDTO.BlogId);
            return blogDTO;
        }

        public async Task<List<RelatedBLogDTO>> RelatedBlogs(int id)
        {
            var query = from b in _context.RelatedBlogs
                        join b2 in _context.Blogs on b.RelatedBlogId equals b2.Id
                        where b.BlogId == id
                        select new RelatedBLogDTO
                        {
                            BlogId = b2.Id,
                            BlogName = b2.Title,
                            DateString = DateTimeHelper.ToVietnameseDateString(b2.CreatedDate),
                        };

            var result = await query.ToListAsync();
            return result;
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

        public async Task<LaziLoadDTO<Blog>> GetOther(int blogId , string cursor, int limit)
        {
            var lastBlogId = cursor != null ? Convert.ToDateTime(cursor) : (DateTime?)null;
            var query = _context.Blogs.AsQueryable();

            if (lastBlogId.HasValue)
            {
                query = query.Where(b => b.CreatedDate < lastBlogId.Value);
            }
            query = query.Where(b => b.Id != blogId);
            var blogs = await query.OrderByDescending(b => b.CreatedDate)
                                   .Take(limit)
                                   .ToListAsync();
            var nextCursor = blogs.Any() ? blogs.Last().CreatedDate.ToString() : null;
            var result = new LaziLoadDTO<Blog>()
            {
                nextCursor = nextCursor,
                Items = blogs,
            };
            return result;
        }

        public async Task<bool> AddRelatedBlog(RelatedAdd related)
        {
            var relatedBlog =  _context.RelatedBlogs.Where(x => x.BlogId == related.BlogId).AsQueryable();
            if (relatedBlog.Any())
            {
                _context.RelatedBlogs.RemoveRange(relatedBlog);
                await _context.SaveChangesAsync();
            }
            var relatedBlogNews = new List<RelatedBlog>();
            foreach (int id in related.RelatedBlogId)
            {
                var blog = new RelatedBlog()
                {
                    BlogId = related.BlogId,
                    RelatedBlogId = id
                };
                relatedBlogNews.Add(blog);
            }
            if (relatedBlogNews.Any())
            {
                await _context.RelatedBlogs.AddRangeAsync(relatedBlog);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
