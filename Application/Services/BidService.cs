using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BidService : IBidService
    {
        private readonly IMapper _mapper;
        private readonly IBidRepository _bidRepository;
        private readonly IUrlRepository _urlRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BidService(IMapper mapper, IBidRepository bidRepository, IUrlRepository urlRepository,  IProjectRepository projectRepository, IAppUserRepository appUserRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _bidRepository = bidRepository;
            _urlRepository = urlRepository;

            _projectRepository = projectRepository;
            _appUserRepository = appUserRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Add(BidDTO request)
        {
            var bid = _mapper.Map<Bid>(request);

            bid.ProjectId = request.ProjectId;
            bid.UserId = request.UserId;
            bid.Proposal = request.Proposal;
            bid.Duration = request.Duration;
            bid.Budget = request.Budget;
            bid.CreatedDate = DateTime.Now;
            bid.UpdatedDate = DateTime.Now;

            await _bidRepository.AddAsync(bid);
            //var urlRecord = bid.CreateUrlRecordAsync("du-an", bid.ProjectId.ToString());
            //await _urlRepository.AddAsync(urlRecord);
            return (int) bid.Id;
        }



        //public async Task<Pagination<BidDTO>> GetWithFilter(Expression<Func<Bid, bool>> filter, int pageIndex, int pageSize)
        //{
        //    var bids = await _bidRepository.GetAsync(filter, pageIndex, pageSize);

        //    var bidDTOs = _mapper.Map<Pagination<BidDTO>>(bids);
        //    return bidDTOs;
        //}

        public async Task<Pagination<BidDTO>> GetWithFilter(Expression<Func<Bid, bool>> filter, int pageIndex, int pageSize)
        {
            var bids = await _bidRepository.GetAsync(filter, pageIndex, pageSize);
            var bidDTOs = _mapper.Map<Pagination<BidDTO>>(bids);
            var updatedItems = new List<BidDTO>();

            foreach (var bid in bidDTOs.Items)
            {
                var bidDTO = _mapper.Map<BidDTO>(bid);


                var user = await _appUserRepository.GetByIdAsync(bid.UserId);
                bidDTO.AppUser = _mapper.Map<AppUserDTO>(user);

                var project = await _projectRepository.GetByIdAsync(bid.ProjectId);
                bidDTO.Project = _mapper.Map<ProjectDTO>(project);

                var category = await _categoryRepository.GetByIdAsync(bid.Project.CategoryId);
                bidDTO.Project.Category = _mapper.Map<CategoryDTO>(category);


                updatedItems.Add(bidDTO);
            }

            bidDTOs.Items = updatedItems;
            return bidDTOs;
        }

    }
}
