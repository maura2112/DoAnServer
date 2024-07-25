using Application.Common;
using Application.DTOs;
using Application.DTOs.Favorite;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Azure.Core;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Application.Common.ProjectStatus;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUrlRepository _urlRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectSkillRepository _projectSkillRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly PaginationService<ProjectDTO> _paginationService;
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;


        public ProjectService(IMapper mapper, IProjectRepository projectRepository, IUrlRepository urlRepository, IAppUserRepository appUserRepository, ICategoryRepository categoryRepository, IProjectSkillRepository projectSkillRepository, ICurrentUserService currentUserService, IAddressRepository addressRepository, IStatusRepository statusRepository, PaginationService<ProjectDTO> paginationService, ApplicationDbContext context, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _urlRepository = urlRepository;
            _appUserRepository = appUserRepository;
            _categoryRepository = categoryRepository;
            _projectSkillRepository = projectSkillRepository;
            _currentUserService = currentUserService;
            _addressRepository = addressRepository;
            _statusRepository = statusRepository;
            _paginationService = paginationService;
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task<ProjectDTO> Add(AddProjectDTO request)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var project = _mapper.Map<Project>(request);
            var existedCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (existedCategory == null)
            {
                return null;
            }
            project.CategoryId = request.CategoryId;
            project.MinBudget = request.MinBudget;
            project.MaxBudget = request.MaxBudget;
            project.Duration = request.Duration;
            // createdBy
            project.CreatedBy = userId;
            project.CreatedDate = DateTime.UtcNow;
            project.UpdatedDate = DateTime.UtcNow;
            project.StatusId = 1;
            project.IsDeleted = false;
            project.Description = request.Description;

            try
            {
                await _projectRepository.AddAsync(project);
            }
            catch (Exception ex)
            {
                throw new Exception("Tạo dự án mới thất bại", ex);
            }

            //var urlRecord = project.CreateUrlRecordAsync("tao-du-an", project.Title);
            //await _urlRepository.AddAsync(urlRecord);

            var projectDto = _mapper.Map<ProjectDTO>(project);
            //smthing ưởng here
            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDto.AppUser = _mapper.Map<AppUserDTO>(user);

            //var address = await _addressRepository.GetAddressByUserId(userId);
            //projectDto.AppUser.Address = _mapper.Map<AddressDTO>(address);

            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDto.Category = _mapper.Map<CategoryDTO>(category);

            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDto.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            foreach (var skill in listSkills)
            {
                projectDto.Skill.Add(skill.SkillName);
            }

            projectDto.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDto.CreatedDate);
            projectDto.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.CreatedDate);
            projectDto.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.UpdatedDate);

            return projectDto;
        }


        public async Task<ProjectDTO> Delete(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new Exception($"Project with ID {id} not found.");
            }
            project.IsDeleted = true;
            _projectRepository.Update(project);
            var projectDto = _mapper.Map<ProjectDTO>(project);
            return projectDto;
        }

        public async Task<Pagination<ProjectDTO>> Get(int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.ProjectToPagination(pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            var updatedItems = new List<ProjectDTO>();

            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);

                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = user != null ? _mapper.Map<AppUserDTO>(user) : null;

                var address = user != null ? await _addressRepository.GetAddressByUserId((int)x.CreatedBy) : null;
                if (model.AppUser != null && address != null)
                {
                    model.AppUser.Address = _mapper.Map<AddressDTO>(address);
                }

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = category != null ? _mapper.Map<CategoryDTO>(category) : null;

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = status != null ? _mapper.Map<ProjectStatusDTO>(status) : null;

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                if (listSkills != null)
                {
                    foreach (var skill in listSkills)
                    {
                        model.Skill.Add(skill.SkillName);
                    }
                }

                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }

            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<Pagination<ProjectDTO>> GetWithFilterRecruiter(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.GetAsync(filter, pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            var updatedItems = new List<ProjectDTO>();

            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);

                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = _mapper.Map<AppUserDTO>(user);
                var address = await _addressRepository.GetAddressByUserId((int)x.CreatedBy);
                model.AppUser.Address = _mapper.Map<AddressDTO>(address);

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = _mapper.Map<CategoryDTO>(category);

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                foreach (var skill in listSkills)
                {
                    model.Skill.Add(skill.SkillName);
                }
                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }

            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<Pagination<ProjectDTO>> GetWithFilterForRecruiter(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var projects = await _projectRepository.GetAsyncForRecruiter(filter, userId, pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            var updatedItems = new List<ProjectDTO>();

            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);

                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = _mapper.Map<AppUserDTO>(user);
                var address = await _addressRepository.GetAddressByUserId((int)x.CreatedBy);
                model.AppUser.Address = _mapper.Map<AddressDTO>(address);

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = _mapper.Map<CategoryDTO>(category);

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                foreach (var skill in listSkills)
                {
                    model.Skill.Add(skill.SkillName);
                }
                model.CanMakeDone = (model.StatusId == 3) ? true : false;
                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }

            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<ProjectDTO> GetDetailProjectById(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return null;
            }

            if (project.IsDeleted == true || project.StatusId == 1 || project.StatusId == 5 || project.StatusId == 6)
            {

                if (project.CreatedBy != _currentUserService.UserId)
                {
                    return null;
                }
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);

            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDTO.AppUser2 = _mapper.Map<AppUserDTO2>(user);


            var totalCompleteProject = await _context.RateTransactions.CountAsync(x => x.BidUserId == projectDTO.Id || x.ProjectUserId == projectDTO.Id);
            var totalRate = await _context.Ratings.CountAsync(x => x.RateToUserId == user.Id);
            decimal avgRate;
            if (totalRate != 0)
            {
                float sumStars = await _context.Ratings.Where(x => x.RateToUserId == user.Id).SumAsync(x => x.Star);
                avgRate = Math.Round((decimal)sumStars / totalRate, 1);
            }
            else
            {
                avgRate = 0;
            }
            projectDTO.AppUser2.CreatedDate = user.CreatedDate;
            projectDTO.AppUser2.EmailConfirmed = user.EmailConfirmed;
            projectDTO.AppUser2.AvgRate = (float)avgRate;
            projectDTO.AppUser2.TotalRate = totalRate;
            projectDTO.AppUser2.TotalCompleteProject = totalCompleteProject;
            var address = await _addressRepository.GetAddressByUserId((int)project.CreatedBy);
            if (address != null)
            {
                projectDTO.AppUser2.Country = address.Country;
                projectDTO.AppUser2.City = address.City;
            }
            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDTO.Category = _mapper.Map<CategoryDTO>(category);
            var userId = _currentUserService.UserIdCan0;
            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDTO.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);
            projectDTO.IsFavorite = await IsFavorite(userId, id);
            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            projectDTO.Skill = listSkills.Select(x => x.SkillName).ToList();
            projectDTO.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDTO.CreatedDate);
            projectDTO.AverageBudget = await _projectRepository.GetAverageBudget(projectDTO.Id);
            projectDTO.TotalBids = await _projectRepository.GetTotalBids(projectDTO.Id);
            projectDTO.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.CreatedDate);
            projectDTO.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.UpdatedDate);
            return projectDTO;
        }

        //not check delete
        public async Task<ProjectDTO> GetDetailProjectForId(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return null;
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);

            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDTO.AppUser2 = _mapper.Map<AppUserDTO2>(user);


            var totalCompleteProject = await _context.RateTransactions.CountAsync(x => x.BidUserId == user.Id || x.ProjectUserId == user.Id);
            var totalRate = await _context.Ratings.CountAsync(x => x.RateToUserId == user.Id);
            int avgRate;
            if (totalRate != 0)
            {
                avgRate = await _context.Ratings.Where(x => x.RateToUserId == user.Id).SumAsync(x => x.Star) /
                          totalRate;
            }
            else
            {
                avgRate = 0;
            }
            projectDTO.AppUser2.CreatedDate = user.CreatedDate;
            projectDTO.AppUser2.EmailConfirmed = user.EmailConfirmed;
            projectDTO.AppUser2.AvgRate = avgRate;
            projectDTO.AppUser2.TotalRate = totalRate;
            projectDTO.AppUser2.TotalCompleteProject = totalCompleteProject;
            var address = await _addressRepository.GetAddressByUserId((int)project.CreatedBy);
            if (address != null)
            {
                projectDTO.AppUser2.Country = address.Country;
                projectDTO.AppUser2.City = address.City;
            }
            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDTO.Category = _mapper.Map<CategoryDTO>(category);

            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDTO.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            projectDTO.Skill = listSkills.Select(x => x.SkillName).ToList();
            projectDTO.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDTO.CreatedDate);
            projectDTO.AverageBudget = await _projectRepository.GetAverageBudget(projectDTO.Id);
            projectDTO.TotalBids = await _projectRepository.GetTotalBids(projectDTO.Id);
            projectDTO.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.CreatedDate);
            projectDTO.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.UpdatedDate);
            return projectDTO;
        }

        public async Task<Pagination<ProjectDTO>> GetProjectDTOs(ProjectSearchDTO search)
        {
            var projects = await _projectRepository.GetAll();
            var res = projects.AsQueryable();
            var projectDTOs = projects.Select(project => ProcessProjectAsync(project).Result).ToList();

            var projectDTOList = projectDTOs.AsQueryable();
            if (search.Keyword != null)
            {
                projectDTOList = projectDTOList.Where(x => x.Title.ToLower().Contains(search.Keyword.ToLower()) || x.Description.ToLower().Contains(search.Keyword.ToLower()));
            }
            if (search.Skills != null)
            {
                foreach (var skill in search.Skills)
                {
                    projectDTOList = projectDTOList.Where(x => x.Skill.Contains(skill));
                }
            }
            if (search.StatusId != null)
            {
                projectDTOList = projectDTOList.Where(x => x.StatusId == search.StatusId);
            }
            if (search.MinBudget != null)
            {
                projectDTOList = projectDTOList.Where(x => x.MinBudget >= search.MinBudget);
            }
            if (search.MaxBudget != null)
            {
                projectDTOList = projectDTOList.Where(x => x.MaxBudget <= search.MaxBudget);
            }
            if (search.CategoryId != null)
            {
                projectDTOList = projectDTOList.Where(x => x.CategoryId == search.CategoryId);
            }
            if (search.CreatedFrom != null)
            {
                projectDTOList = projectDTOList.Where(x => x.CreatedDate >= search.CreatedFrom);
            }
            if (search.CreatedTo != null)
            {
                projectDTOList = projectDTOList.Where(x => x.CreatedDate <= search.CreatedTo);
            }
            return await _paginationService.ToPagination(projectDTOList.OrderByDescending(x=>x.CreatedDate).ToList(), search.PageIndex, search.PageSize);
        }

        public async Task<ProjectDTO> ProcessProjectAsync(Project project)
        {
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            //category
            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDTO.Category = _mapper.Map<CategoryDTO>(category);
            //user
            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDTO.AppUser = _mapper.Map<AppUserDTO>(user);
            //address
            var address = user != null ? await _addressRepository.GetAddressByUserId((int)projectDTO.CreatedBy) : null;
            if (projectDTO.AppUser != null && address != null)
            {
                projectDTO.AppUser.Address = _mapper.Map<AddressDTO>(address);
            }
            //status
            var status = await _statusRepository.GetByIdAsync(projectDTO.StatusId);
            projectDTO.ProjectStatus = status != null ? _mapper.Map<ProjectStatusDTO>(status) : null;
            //Skill
            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(projectDTO.Id);
            projectDTO.Skill = listSkills.Select(x => x.SkillName).ToList();

            projectDTO.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDTO.CreatedDate);
            projectDTO.AverageBudget = await _projectRepository.GetAverageBudget(projectDTO.Id);
            projectDTO.TotalBids = await _projectRepository.GetTotalBids(projectDTO.Id);
            projectDTO.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.CreatedDate);
            projectDTO.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.UpdatedDate);
            return projectDTO;
        }

        public async Task<List<ProjectStatusDTO>> GetAllStatus()
        {
            var statuses = await _statusRepository.GetAll();
            var statuseDTO = _mapper.Map<List<ProjectStatusDTO>>(statuses);
            return statuseDTO;
        }

        public async Task<ProjectDTO> UpdateProjectStatus(ProjectStatusUpdate update)
        {
            var project = await _projectRepository.GetByIdAsync(update.ProjectId);
            
            
            var userId = _currentUserService.UserId;
            if (update.StatusId == 5 && project.StatusId == 1)
            {
                project.RejectReason = update.RejectReason;
            } // reject 
            else if(update.StatusId == 9)
            {
                var BidAccepted = await _context.Bids.FirstOrDefaultAsync(x => x.ProjectId == update.ProjectId && x.AcceptedDate != null);
                if(BidAccepted.UserId == userId && update.BidId == BidAccepted.Id)
                {
                    project.UpdatedDate = DateTime.Now;
                    project.StatusId = update.StatusId;
                    _projectRepository.Update(project);
                    var DTOAbout9 = _mapper.Map<ProjectDTO>(project);
                    var statusAbout9 = await _statusRepository.GetByIdAsync(DTOAbout9.StatusId);
                    DTOAbout9.ProjectStatus = statusAbout9 != null ? _mapper.Map<ProjectStatusDTO>(statusAbout9) : null;
                    var result = await MakeDone((int)update.BidId);
                    return DTOAbout9;
                }
            }
            project.UpdatedDate = DateTime.Now;
            project.StatusId = update.StatusId;
            _projectRepository.Update(project);
            var DTO = _mapper.Map<ProjectDTO>(project);
            var status = await _statusRepository.GetByIdAsync(DTO.StatusId);
            DTO.ProjectStatus = status != null ? _mapper.Map<ProjectStatusDTO>(status) : null;
            return DTO;
        }

        public async Task<bool> MakeDone(int id)
        {
            var userId = _currentUserService.UserId;
            var bid = await _context.Bids.FirstOrDefaultAsync(x=>x.Id == id);
            if (bid == null || bid.UserId != userId || bid.AcceptedDate != null)
            {
                return false;
            }
            var transaction = await _context.RateTransactions.FirstOrDefaultAsync(x => x.BidUserId == bid.UserId && x.ProjectId == bid.ProjectId);
            if (transaction != null)
            {
                return false;
            }
            var transactionNew = new RateTransaction()
            {
                ProjectId = bid.ProjectId,
                BidUserId = userId,
                BidCompletedDate = DateTime.Now,
                IsDeleted = false,
            };
            _context.RateTransactions.Add(transactionNew);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MakeDoneByRec(int projectId)
        {
            var userId = _currentUserService.UserId;
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project.CreatedBy == userId && (project.StatusId == 3 || project.StatusId == 9))
            {
                var bid = await _context.Bids.FirstOrDefaultAsync(x => x.AcceptedDate != null && projectId == x.ProjectId);
                if (bid == null)
                {
                    return false;
                }
                var transaction = await _context.RateTransactions.FirstOrDefaultAsync(x => x.ProjectId == projectId);
                if (transaction != null)
                {
                    transaction.ProjectAcceptedDate = DateTime.Now;
                    _context.RateTransactions.Update(transaction);
                    await _context.SaveChangesAsync();
                    return false;
                }
                transaction = new RateTransaction()
                {
                    ProjectId = projectId,
                    IsDeleted = false,
                    ProjectUserId = project.CreatedBy,
                    BidUserId = bid.UserId,
                    Rated = false,
                    BidCompletedDate = DateTime.Now,
                    ProjectAcceptedDate = DateTime.Now,
                };
                await _context.RateTransactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                project.StatusId = 6;
                project.UpdatedDate = DateTime.UtcNow;
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Pagination<ProjectDTO>> GetWithFilter( ProjectSearchDTO dto, int pageIndex, int pageSize)
        {
            var query = from p in _context.Projects
                        join ps in _context.ProjectSkills on p.Id equals ps.ProjectId into psGroup
                        from ps in psGroup.DefaultIfEmpty()
                        join s in _context.Skills on ps.SkillId equals s.Id into sGroup
                        from s in sGroup.DefaultIfEmpty()
                        join c in _context.Categories on p.CategoryId equals c.Id into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        group new { p, s, c } by p into g
                        where g.Key.StatusId == 2 && g.Key.IsDeleted != true
                        orderby g.Key.CreatedDate descending
                        select new
                        {
                            Project = g.Key,
                            Title = g.Key.Title,
                            Description = g.Key.Description,
                            SkillNames = g.Select(x => x.s != null ? x.s.SkillName : (string?)null).Where(skillName => skillName != null).ToList(),
                            Skills = string.Join(", ", g.Select(x => x.s != null ? x.s.SkillName : null).Where(skillName => skillName != null)),
                            CategoryName = g.Select(x => x.c != null ? x.c.CategoryName : null).FirstOrDefault()
                        };

            var queryFilter = query.AsEnumerable(); // Chuyển phần còn lại của truy vấn sang client-side

            if (dto.Keyword != null)
            {
                queryFilter = queryFilter.Where(x => x.Title.ToLower().Contains(dto.Keyword.ToLower()) || x.Description.ToLower().Contains(dto.Keyword.ToLower()) || x.Skills.ToLower().Contains(dto.Keyword.ToLower()) || x.CategoryName.ToLower().Contains(dto.Keyword.ToLower()));
            }
            if (dto.CategoryId != null)
            {
                queryFilter = queryFilter.Where(x => x.Project.CategoryId == dto.CategoryId);
            }
            if (dto.Skills != null && dto.Skills.Any())
            {
                queryFilter = queryFilter.Where(x => x.SkillNames.Any(skill => dto.Skills.Contains(skill)));
            }
            if (dto.MinBudget != null)
            {
                queryFilter = queryFilter.Where(x => x.Project.MinBudget >= dto.MinBudget);
            }
            if (dto.MaxBudget != null)
            {
                queryFilter = queryFilter.Where(x => x.Project.MaxBudget <= dto.MaxBudget);
            }

            var selectProject = queryFilter.Select(x => x.Project);
            var totalItem = selectProject.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToList();

            var result = new Pagination<Project>()
            {
                PageSize = dto.PageSize,
                PageIndex = dto.PageIndex,
                TotalItemsCount = selectProject.Count(),
                Items = totalItem,
            };
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(result);
            var updatedItems = new List<ProjectDTO>();
            var userid = _currentUserService.UserIdCan0;
            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);

                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = _mapper.Map<AppUserDTO>(user);
                var address = await _addressRepository.GetAddressByUserId((int)x.CreatedBy);
                model.AppUser.Address = _mapper.Map<AddressDTO>(address);

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = _mapper.Map<CategoryDTO>(category);

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                foreach (var skill in listSkills)
                {
                    model.Skill.Add(skill.SkillName);
                }

                model.IsFavorite = await IsFavorite(userid, model.Id);
                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }

            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<ProjectDTO> Update(UpdateProjectDTO request)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);

            // Update the project's properties
            project.Title = request.Title;
            project.CategoryId = request.CategoryId;
            project.MinBudget = request.MinBudget;
            project.MaxBudget = request.MaxBudget;
            project.Duration = request.Duration;
            project.UpdatedDate = DateTime.Now; // update the updated date
            //project.CreatedBy = request.CreatedBy;
            project.Description = request.Description;
            //mediafile

            // Update the project in the repository
            _projectRepository.Update(project);

            // Handle URL record update
            //var urlRecord = project.CreateUrlRecordAsync("chinh-sua-du-an", project.Title);
            //await _urlRepository.AddAsync(urlRecord); // assuming there's a method for updating URLs

            // Map the updated project back to a DTO
            var projectDto = _mapper.Map<ProjectDTO>(project);

            // Retrieve and map the user who created the project
            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDto.AppUser = _mapper.Map<AppUserDTO>(user);

            var address = await _addressRepository.GetAddressByUserId((int)project.CreatedBy);
            projectDto.AppUser.Address = _mapper.Map<AddressDTO>(address);

            // Retrieve and map the category of the project
            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDto.Category = _mapper.Map<CategoryDTO>(category);

            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDto.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

            await _projectSkillRepository.DeleteProjectSkill(project.Id);

            // Retrieve and map the skills associated with the project
            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            foreach (var skill in listSkills)
            {
                projectDto.Skill.Add(skill.SkillName);
            }
            projectDto.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDto.CreatedDate);
            projectDto.AverageBudget = await _projectRepository.GetAverageBudget(projectDto.Id);
            projectDto.TotalBids = await _projectRepository.GetTotalBids(projectDto.Id);
            projectDto.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.CreatedDate);
            projectDto.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.UpdatedDate);
            return projectDto;
        }

        public async Task<ProjectDTO> UpdateStatus(int projectId, int statusId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new Exception("Project not found");
            }
            project.StatusId = statusId;



            var projectDto = _mapper.Map<ProjectDTO>(project);
            _projectRepository.Update(project);

            return projectDto;
        }

        // Project in sig cho Freelancer
        public async Task<Pagination<ProjectBidDTO>> GetByStatus(ProjectStatusFilter search)
        {
            var query = from b in _context.Bids
                        join p in _context.Projects on b.ProjectId equals p.Id
                        join s in _context.ProjectStatus on p.StatusId equals s.Id
                        join u in _context.Users on p.CreatedBy equals u.Id
                        where b.UserId == search.userId
                        select new ProjectBidDTO
                        {
                            ProjectName = p.Title,
                            ProjectId = b.ProjectId,
                            BidBudget = b.Budget,
                            BidId = b.Id,
                            StatusId = s.Id,
                            Status = s.StatusName,
                            ProjectOwner = u.Name,
                            ProjectOwnerId = u.Id,
                            TimeBid = b.CreatedDate,
                            Duration = b.Duration,
                            Deadline = (p.EstimateStartDate != null) ? (DateTime)p.EstimateStartDate + TimeSpan.FromDays(p.Duration) : null,
                            CanMakeDone = (p.StatusId == (int)Application.Common.ProjectStatus.StatusId.Close) ? true : false,
                        };
            if (search.statusId.HasValue)
            {
                query = query.Where(x => x.StatusId == search.statusId.Value);
            }
            var totalItem = await query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToListAsync();
            var result = new Pagination<ProjectBidDTO>()
            {
                PageSize = search.PageSize,
                PageIndex = search.PageIndex,
                TotalItemsCount = query.Count(),
                Items = totalItem,
            };

            return result;
        }

        public async Task<bool> CreateFavorite(FavoriteCreate create)
        {
            var favorite = await _context.FavoriteProjects.FirstOrDefaultAsync(x => x.AppUserId == create.UserId && x.ProjectId == create.ProjectId);
            if (favorite != null)
            {
                return false;
            }
            var fa = new FavoriteProject()
            {
                AppUserId = (int)create.UserId,
                ProjectId = create.ProjectId,
                SavedDate = DateTime.Now,
            };
            await _context.FavoriteProjects.AddAsync(fa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteFavorite(FavoriteCreate create)
        {
            var favorite = await _context.FavoriteProjects.FirstOrDefaultAsync(x => x.AppUserId == create.UserId && x.ProjectId == create.ProjectId);
            if (favorite == null)
            {
                return 0;
            }
            var favoriteId = favorite.Id;
            _context.FavoriteProjects.Remove(favorite);
            await _context.SaveChangesAsync();
            return favoriteId;
        }

        public async Task<Pagination<FavoriteDTO>> GetFavorites(FavoriteSearch search)
        {
            var query = from f in _context.FavoriteProjects
                        join p in _context.Projects on f.ProjectId equals p.Id
                        join u in _context.Users on f.AppUserId equals u.Id
                        join s in _context.ProjectStatus on p.StatusId equals s.Id
                        where u.Id == search.UserId
                        select new FavoriteDTO
                        {
                            Id = f.Id,
                            ProjectId = f.ProjectId,
                            UserId = u.Id,
                            MinBudget = p.MinBudget,
                            MaxBudget = p.MaxBudget,
                            Duration = p.Duration,
                            ProjectName = p.Title,
                            Description = p.Description,
                            Status = s.StatusName,
                            StatusColor = s.StatusColor,
                            StatusId = s.Id,
                            CreatedProject = DateTimeHelper.ToVietnameseDateString(p.CreatedDate),
                            SavedTime = DateTimeHelper.ToVietnameseDateString(f.SavedDate),
                        };
            if (search.StatusId != null)
            {
                query = query.Where(x => x.StatusId == search.StatusId);
            }
            var totalItem = await query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToListAsync();
            var result = new Pagination<FavoriteDTO>()
            {
                PageSize = search.PageSize,
                PageIndex = search.PageIndex,
                TotalItemsCount = query.Count(),
                Items = totalItem,
            };
            return result;
        }

        public async Task<FavoriteDTO> GetFavoriteById(int uid, int pid)
        {
            var query = await (from f in _context.FavoriteProjects
                               join p in _context.Projects on f.ProjectId equals p.Id
                               join u in _context.Users on f.AppUserId equals u.Id
                               join s in _context.ProjectStatus on p.StatusId equals s.Id
                               where u.Id == uid && p.Id == pid
                               select new FavoriteDTO
                               {
                                   Id = f.Id,
                                   ProjectId = f.ProjectId,
                                   UserId = u.Id,
                                   ProjectName = p.Title,
                                   MinBudget = p.MinBudget,
                                   MaxBudget = p.MaxBudget,
                                   Duration = p.Duration,
                                   Description = p.Description,
                                   Status = s.StatusName,
                                   StatusColor = s.StatusColor,
                                   CreatedProject = DateTimeHelper.ToVietnameseDateString(p.CreatedDate),
                                   SavedTime = DateTimeHelper.ToVietnameseDateString(f.SavedDate),
                               }).FirstOrDefaultAsync();
            return query;
        }

        public async Task<Pagination<ProjectDTO>> GetByUserId(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.RecruiterGetAsync(filter, pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            var updatedItems = new List<ProjectDTO>();

            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);
                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = _mapper.Map<AppUserDTO>(user);
                var address = await _addressRepository.GetAddressByUserId((int)x.CreatedBy);
                model.AppUser.Address = _mapper.Map<AddressDTO>(address);

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = _mapper.Map<CategoryDTO>(category);

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                foreach (var skill in listSkills)
                {
                    model.Skill.Add(skill.SkillName);
                }
                model.CanMakeDone = (model.StatusId== 3 || model.StatusId == 9) ?true:false;
                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }
            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<bool?> IsFavorite(int userId, int projectId)
        {
            if(userId == 0)
            {
                return null;
            }
            var favorite = await _context.FavoriteProjects.FirstOrDefaultAsync(x => x.AppUserId == userId && projectId == x.ProjectId);

            if(favorite == null)
            {
                return false;
            }
            return true;
        }
    }
}
