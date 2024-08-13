
using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Entities;


namespace Application.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Pagination<Product>, Pagination<ProductDTO>>().ReverseMap();

        CreateMap<Project, ProjectDTO>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate != null ? src.CreatedDate.ToLocalTime() : DateTime.MinValue))
    .ReverseMap(); ;
        CreateMap<Pagination<Project>, Pagination<ProjectDTO>>().ReverseMap();
        //add project
        CreateMap<Project, AddProjectDTO>().ReverseMap();
        CreateMap<ProjectDTO, AddProjectDTO>().ReverseMap();
        //UpdateProject 
        CreateMap<Project, UpdateProjectDTO>().ReverseMap();
        CreateMap<ProjectDTO, UpdateProjectDTO>().ReverseMap();
        //Notification
        CreateMap<Notification, NotificationDto>()
    .ForMember(dest => dest.Datetime, opt => opt.MapFrom(src => src.Datetime.HasValue ? src.Datetime.Value.ToLocalTime() : DateTime.MinValue))
    .ReverseMap();
        CreateMap<Message, ChatDto>()
            .ForMember(dest => dest.SendDate, opt => opt.MapFrom(src => src.SendDate.HasValue ? src.SendDate.Value.ToLocalTime() : DateTime.MinValue))
            .ReverseMap();
        CreateMap<Pagination<Message>, Pagination<ChatDto>>().ReverseMap();

        CreateMap<Bid, BidDTO>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate != null ? src.CreatedDate.ToLocalTime() : DateTime.MinValue)).ReverseMap();
        CreateMap<Pagination<Bid>, Pagination<BidDTO>>().ReverseMap();
        //add bidding
        CreateMap<Bid, BiddingDTO>().ReverseMap();
        CreateMap<BidDTO, BiddingDTO>().ReverseMap();
        //update bidding
        CreateMap<Bid, UpdateBidDTO>().ReverseMap();
        CreateMap<BidDTO, UpdateBidDTO>().ReverseMap();

        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
        CreateMap<Pagination<Category>, Pagination<CategoryDTO>>().ReverseMap();

        CreateMap<AppUser, AppUserDTO>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate != null ? src.CreatedDate.ToLocalTime() : DateTime.MinValue)).ReverseMap();
        CreateMap<AppUser, AppUserDTO2>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate != null ? src.CreatedDate.ToLocalTime() : DateTime.MinValue)).ReverseMap();

        CreateMap<Rating, RatingDTO>().ReverseMap();

        CreateMap<string, List<Experience>>().ReverseMap();
        CreateMap<string, List<Education>>().ReverseMap();
        CreateMap<string, List<Qualification>>().ReverseMap();

        //Blog
        CreateMap<BlogCreateDTO, Blog>().ReverseMap();
        //Meida File
        CreateMap<MediaFile, MediaFileDTO>().ReverseMap();
        //Report
        CreateMap<UserReport, ReportDTO>().ReverseMap();
        CreateMap<UserReport, ReportCreateDTO>().ReverseMap();

        CreateMap<Transaction, TransactionDTO>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<Skill, SkillDTO>().ReverseMap();
        CreateMap<Pagination<Skill>, Pagination<SkillDTO>>().ReverseMap();

        CreateMap<ProjectSkill, ProjectSkillDTO>().ReverseMap();
        CreateMap<Pagination<ProjectSkill>, Pagination<ProjectSkillDTO>>().ReverseMap();
        
        CreateMap<UserSkill, UserSkillDTO>().ReverseMap();
        CreateMap<Pagination<UserSkill>, Pagination<UserSkillDTO>>().ReverseMap();

        CreateMap<AppUser, UserDTO>().ForMember(dest => dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd.HasValue ? src.LockoutEnd.Value.UtcDateTime : (DateTime?)null)).ReverseMap();

        CreateMap<Address, AddressDTO>().ReverseMap();
        CreateMap<Pagination<Address>, Pagination<AddressDTO>>().ReverseMap();

        CreateMap<Domain.Entities.ProjectStatus, ProjectStatusDTO>().ReverseMap();
        CreateMap<Pagination<Domain.Entities.ProjectStatus>, Pagination<ProjectStatusDTO>>().ReverseMap();
    }
}
