
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

        CreateMap<Project, ProjectDTO>().ReverseMap();
        CreateMap<Pagination<Project>, Pagination<ProjectDTO>>().ReverseMap();
        //add project
        CreateMap<Project, AddProjectDTO>().ReverseMap();
        CreateMap<ProjectDTO, AddProjectDTO>().ReverseMap();
        //UpdateProject 
        CreateMap<Project, UpdateProjectDTO>().ReverseMap();
        CreateMap<ProjectDTO, UpdateProjectDTO>().ReverseMap();
        //Notification
        CreateMap<Notification, NotificationDto>().ReverseMap();
        CreateMap<Message, ChatDto>().ReverseMap();

        CreateMap<Bid, BidDTO>().ReverseMap();
        CreateMap<Pagination<Bid>, Pagination<BidDTO>>().ReverseMap();
        //add bidding
        CreateMap<Bid, BiddingDTO>().ReverseMap();
        CreateMap<BidDTO, BiddingDTO>().ReverseMap();
        //update bidding
        CreateMap<Bid, UpdateBidDTO>().ReverseMap();
        CreateMap<BidDTO, UpdateBidDTO>().ReverseMap();

        CreateMap<Category, CategoryDTO>().ReverseMap();

        CreateMap<AppUser, AppUserDTO>().ReverseMap();

        CreateMap<Rating, RatingDTO>().ReverseMap();

        CreateMap<string, List<Experience>>().ReverseMap();
        CreateMap<string, List<Education>>().ReverseMap();
        CreateMap<string, List<Qualification>>().ReverseMap();

        //Meida File
        CreateMap<MediaFile, MediaFileDTO>().ReverseMap();
        //Report
        CreateMap<UserReport, ReportDTO>().ReverseMap();
        CreateMap<UserReport, ReportCreateDTO>().ReverseMap();

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
