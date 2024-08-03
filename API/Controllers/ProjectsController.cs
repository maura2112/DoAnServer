using API.Hubs;
using API.Utilities;
using Application.DTOs;
using Application.DTOs.Favorite;
using Application.Extensions;
using Application.IServices;

using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static API.Common.Url;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers
{

    public class ProjectsController : ApiControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISkillService _skillService;
        private readonly IProjectRepository _projectRepository;
        private readonly IBidRepository _bidRepository;
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;
        public ProjectsController(IProjectService projectService, ICurrentUserService currentUserService, ISkillService skillService, IProjectRepository projectRepository, IBidRepository bidRepository, INotificationService notificationService, INotificationRepository notificationRepository, ApplicationDbContext context, IHubContext<ChatHub> chatHubContext)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
            _skillService = skillService;
            _projectRepository = projectRepository;
            _bidRepository = bidRepository;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _context = context;
            _chatHubContext = chatHubContext;
        }

        [HttpGet]
        [Route(Common.Url.Project.GetAll)]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về BadRequest với ModelState khi ModelState không hợp lệ
            }

            if (pageIndex < 1 || pageSize < 1)
            {
                ModelState.AddModelError("", "Số trang hoặc kích cỡ trang lớn hơn 1"); // Thêm lỗi vào ModelState
                return BadRequest(ModelState); // Trả về BadRequest với ModelState đã thêm lỗi
            }
            else
            {
                return Ok(await _projectService.Get(pageIndex, pageSize));
            }
        }

        [HttpGet]
        [Route(Common.Url.Project.Search)]
        public async Task<IActionResult> Search([FromQuery] ProjectSearchDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            Expression<Func<Domain.Entities.Project, bool>> filter = PredicateBuilder.True<Domain.Entities.Project>();

            if (!string.IsNullOrWhiteSpace(projects.Keyword))
            {
                var keyword = projects.Keyword.ToLower().Trim();
                filter = filter.And(item => item.Title.Contains(keyword));
            }

            if (projects.CategoryId.HasValue && projects.CategoryId > 0)
            {
                filter = filter.And(item => item.CategoryId == projects.CategoryId.Value);
            }

            if (projects.Skills != null && projects.Skills.Any())
            {
                filter = filter.And(item => item.ProjectSkills.Any(skill => projects.Skills.Contains(skill.Skill.SkillName)));
            }

            if (projects.StatusId.HasValue && projects.StatusId != 0)
            {
                filter = filter.And(item => item.StatusId == projects.StatusId.Value);
            }

            if (projects.MinBudget.HasValue && projects.MinBudget > 0)
            {
                filter = filter.And(item => item.MinBudget >= projects.MinBudget);
            }
            if (projects.Duration.HasValue && projects.Duration > 0)
            {
                filter = filter.And(item => item.Duration == projects.Duration);
            }

            if (projects.MaxBudget.HasValue && projects.MaxBudget > 0)
            {
                filter = filter.And(item => item.MaxBudget <= projects.MaxBudget);
            }

            if (projects.CreatedFrom.HasValue)
            {
                filter = filter.And(item => item.CreatedDate >= projects.CreatedFrom);
            }

            if (projects.CreatedTo.HasValue)
            {
                filter = filter.And(item => item.CreatedDate <= projects.CreatedTo);
            }
            if (projects.IsDeleted.HasValue)
            {
                filter = filter.And(item => item.IsDeleted == projects.IsDeleted.Value);
            }

            var result = await _projectService.GetWithFilterRecruiter(filter, projects.PageIndex, projects.PageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route(Common.Url.Project.SearchHomePage)]
        public async Task<IActionResult> SearchHomePage([FromQuery] ProjectSearchDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var result = await _projectService.GetWithFilter( projects, projects.PageIndex, projects.PageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route(Common.Url.Project.SearchRecruiter)]
        public async Task<IActionResult> SearchRecruiter([FromQuery] ProjectSearchDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            Expression<Func<Domain.Entities.Project, bool>> filter = PredicateBuilder.True<Domain.Entities.Project>();

            if (!string.IsNullOrWhiteSpace(projects.Keyword))
            {
                var keyword = projects.Keyword.ToLower().Trim();
                filter = filter.And(item => item.Title.Contains(keyword));
            }

            if (projects.CategoryId.HasValue && projects.CategoryId > 0)
            {
                filter = filter.And(item => item.CategoryId == projects.CategoryId.Value);
            }
            if (projects.StatusId.HasValue && projects.StatusId != 0)
            {
                filter = filter.And(item => item.StatusId == projects.StatusId.Value);
            }

            if (projects.Skills != null && projects.Skills.Any())
            {
                filter = filter.And(item => item.ProjectSkills.Any(skill => projects.Skills.Contains(skill.Skill.SkillName)));
            }
            if (projects.Duration.HasValue && projects.Duration > 0)
            {
                filter = filter.And(item => item.Duration == projects.Duration);
            }

            if (projects.MinBudget.HasValue && projects.MinBudget > 0)
            {
                filter = filter.And(item => item.MinBudget >= projects.MinBudget);
            }

            if (projects.MaxBudget.HasValue && projects.MaxBudget > 0)
            {
                filter = filter.And(item => item.MaxBudget <= projects.MaxBudget);
            }

            if (projects.CreatedFrom.HasValue)
            {
                filter = filter.And(item => item.CreatedDate >= projects.CreatedFrom);
            }

            if (projects.CreatedTo.HasValue)
            {
                filter = filter.And(item => item.CreatedDate <= projects.CreatedTo);
            }
            
            filter = filter.And(item => item.IsDeleted == false);
            
            var result = await _projectService.GetWithFilterForRecruiter(filter, projects.PageIndex, projects.PageSize);
            return Ok(result);
        }


        [HttpGet]
        [Route(Common.Url.Project.Gets)]
        public async Task<IActionResult> Gets([FromQuery] ProjectSearchDTO projects)
        {
            var projectDTOs = await _projectService.GetProjectDTOs(projects);
            return Ok(projectDTOs); 
        }

        [HttpPost]
        [Route(Common.Url.Project.MakeDoneProject)]
        public async Task<IActionResult> MakeDoneProject([FromBody] int projectId)
        {
            var result = await _projectService.MakeDoneByRec(projectId);
            return Ok(result);
        }
        //đang dùng cái này
        [HttpPut]
        [Route(Common.Url.Project.UpdateStatus)]
        public async Task<IActionResult> UpdateStatus([FromBody] ProjectStatusUpdate update)
        {
            var userId = _currentUserService.UserId;
            var projectDTOs = await _projectService.UpdateProjectStatus(update);
            if(projectDTOs == null)
            {
                return BadRequest("Dự án của bạn đã bị từ chối quá nhiều lần. Hãy tạo dự án mới");
            }
            if(projectDTOs.StatusId == 5) {
                NotificationDto notificationDto = new NotificationDto()
                {
                    NotificationId = await _notificationRepository.GetNotificationMax() + 1,
                    SendId = userId,
                    SendUserName = "Hệ thống GoodJob",
                    ProjectName = projectDTOs.Title,//k can cx dc
                    RecieveId = projectDTOs.CreatedBy,
                    Description = "đã từ chối dự án của bạn",
                    Datetime = DateTime.Now,
                    NotificationType = 1,
                    IsRead = 0,
                    Link = "detail/" + projectDTOs.Id
                };
                bool x = await _notificationService.AddNotification(notificationDto);
                if (x)
                {
                    var hubConnections = await _context.HubConnections
                                .Where(con => con.userId == projectDTOs.CreatedBy).ToListAsync();
                    foreach (var hubConnection in hubConnections)
                    {
                        await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", notificationDto);
                    }
                }
            }

            if (projectDTOs.StatusId == 2)
            {
                NotificationDto notificationDto = new NotificationDto()
                {
                    NotificationId = await _notificationRepository.GetNotificationMax() + 1,
                    SendId = userId,
                    SendUserName = "Hệ thống GoodJob",
                    ProjectName = projectDTOs.Title,//k can cx dc
                    RecieveId = projectDTOs.CreatedBy,
                    Description = "đã duyệt dự án của bạn",
                    Datetime = DateTime.Now,
                    NotificationType = 1,
                    IsRead = 0,
                    Link = "detail/" + projectDTOs.Id
                };
                bool x = await _notificationService.AddNotification(notificationDto);
                if (x)
                {
                    var hubConnections = await _context.HubConnections
                                .Where(con => con.userId == projectDTOs.CreatedBy).ToListAsync();
                    foreach (var hubConnection in hubConnections)
                    {
                        await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", notificationDto);
                    }
                }
            }
            return Ok(projectDTOs);
        }

        [HttpGet]
        [Route(Common.Url.Project.AllStatus)]
        public async Task<IActionResult> AllStatus()
        {
            var DTOs = await _projectService.GetAllStatus();
            return Ok(DTOs);
        }


        [HttpPut]
        [Route(Common.Url.Project.AcceptBid)]
        public async Task<IActionResult> AcceptBid(long bidid)
        {
            var userId = _currentUserService.UserId;
            var bid = await _bidRepository.GetByIdAsync(bidid);
            if (bid == null && bid.AcceptedDate != null)
            {
                return BadRequest("Không có dự thầu");
            }
            var project = await _projectRepository.GetByIdAsync(bid.ProjectId);
            if (project.CreatedBy != userId || project.StatusId != (int)Application.Common.ProjectStatus.StatusId.Open)
            {
                return BadRequest("Bạn không thể chấp nhận dự thầu này");
            }
            bid.AcceptedDate = DateTime.Now;
            bid.UpdatedDate = DateTime.Now;
            _bidRepository.Update(bid);
            project.StatusId =(int) Application.Common.ProjectStatus.StatusId.Close; //
            _projectRepository.Update(project);
            var projectDTO = await _projectService.GetDetailProjectById(project.Id);
            return Ok(projectDTO);
        }

        [HttpGet]
        [Route(Common.Url.Project.GetByStatus)]
        public async Task<IActionResult> GetByStatus([FromQuery] ProjectStatusFilter statusFilter)
        {
            var userId = _currentUserService.UserId;
            statusFilter.userId = userId;
            var result = await _projectService.GetByStatus(statusFilter);
            return Ok(result);
        }

        [HttpGet]
        [Route(Common.Url.Project.GetProjectsByUserId)]
        public async Task<IActionResult> GetListByUserId([FromQuery] ProjectListDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var filter = PredicateBuilder.True<Domain.Entities.Project>();
            var userid =  _currentUserService.UserId;

            filter = filter.And(item => item.CreatedBy == userid);

            if (projects.StatusId != null)
            {
                filter = filter.And(item => item.StatusId == projects.StatusId);
            }
            return Ok(await _projectService.GetByUserId(filter, projects.PageIndex, projects.PageSize));
        }

        [HttpGet]
        [Route(Common.Url.Project.GetProjectDetails)]
        public async Task<IActionResult> GetDetailProject(int id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var projectDetail = await _projectService.GetDetailProjectById(id);
            if (projectDetail.IsDeleted == true)
            {
                return BadRequest(new { message = "Dự án không còn tồn tại nữa!" });
            }
            if (projectDetail == null)
            {
                    return NotFound();
            }
            return Ok(projectDetail);
        }


        [HttpPost]
        [Route(Common.Url.Project.Add)]
        public async Task<IActionResult> AddAsync(AddProjectDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var project = await _projectService.Add(DTOs);

            if (project == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProjectResponse { Success = false, Message = "Failed to create project." });
            }

            await _skillService.AddSkillForProject(DTOs.Skill, project.Id);

            var response = new ProjectResponse
            {
                Success = true,
                Message = "Bạn vừa tạo dự án thành công",
                Data = project
            };

            return Ok(response);
        }

        [HttpPost]
        [Route(Common.Url.Project.AddFavorite)]
        [RoleAuthorizeAttribute("Freelancer")]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteCreate dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            dto.UserId = _currentUserService.UserId;
            var result = await _projectService.CreateFavorite(dto);
            if (!result)
            {
                return Conflict("Thêm yêu thích không thành công");
            }
            return Ok(new
            {
                message = "Thêm yêu thích thành công",
                data = result
            });
        }

        [HttpDelete]
        [Route(Common.Url.Project.DeleteFavorite)]
        [RoleAuthorizeAttribute("Freelancer")]
        public async Task<IActionResult> DeleteFavorite([FromBody]FavoriteCreate dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            dto.UserId = _currentUserService.UserId;
            var result = await _projectService.DeleteFavorite(dto);
            if (result == 0)
            {
                return Conflict("Xóa yêu thích không thành công");
            }
            return Ok(new
            {
                message = "Xóa yêu thích thành công",
                data = result
            });
        }

        [HttpGet]
        [Route(Common.Url.Project.Favorite)]
        [RoleAuthorizeAttribute("Freelancer")]
        public async Task<IActionResult> Favorite([FromQuery]FavoriteSearch dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            dto.UserId = _currentUserService.UserId;
            var result = await _projectService.GetFavorites(dto);
            return Ok(result);
        }



        [HttpPut]
        [Route(Common.Url.Project.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateProjectDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fetchedProject = await _projectRepository.GetByIdAsync(DTOs.Id);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy dự án phù hợp!" });
            }

            var project = await _projectService.Update(DTOs);
            if (project == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProjectResponse { Success = false, Message = "Failed to update project." });
            }

            await _skillService.AddSkillForProject(DTOs.Skill, project.Id);

            var response = new ProjectResponse
            {
                Success = true,
                Message = "Bạn vừa cập nhật dự án thành công",
                Data = project
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route(Common.Url.Project.Delete)]
        public async Task<IActionResult> DeleteAsync(int projectId, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fetchedProject = await _projectRepository.GetByIdAsync(projectId);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy dự án phù hợp!" });
            }

            await _projectService.Delete(projectId);
            var userId = _currentUserService.UserId;


            NotificationDto notificationDto = new NotificationDto()
            {
                NotificationId = await _notificationRepository.GetNotificationMax() + 1,
                SendId = userId,
                SendUserName = "Hệ thống GoodJob",
                ProjectName = fetchedProject.Title,//k can cx dc
                RecieveId = fetchedProject.CreatedBy,
                Description = " đã xóa dự án của bạn vì 1 số lí do. Hãy liên hệ với chúng tôi !",
                Datetime = DateTime.Now,
                NotificationType = 1,
                IsRead = 0,
                Link = "#"
            };
            bool x = await _notificationService.AddNotification(notificationDto);
            if (x)
            {
                var hubConnections = await _context.HubConnections
                            .Where(con => con.userId == fetchedProject.CreatedBy).ToListAsync();
                foreach (var hubConnection in hubConnections)
                {
                    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", notificationDto);
                }
            }
            return Ok(new ProjectResponse
            {
                Success = true,
                Message = "Bạn vừa xóa dự án thành công"
            });
        }


        [HttpPost]
        [Route(Common.Url.Project.RejectTesting)]
        public async Task<IActionResult> RejectTesting([FromQuery]int projectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _projectService.RejectTesting(projectId);
            if (!result)
            {
                return BadRequest("Từ chối đợi kiệm thất bại");
            }
            return Ok("Từ chối hoàn thành dự án thành công");
        }
    }
}
