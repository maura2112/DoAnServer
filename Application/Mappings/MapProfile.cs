
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

        CreateMap<Bid, BidDTO>().ReverseMap();
        CreateMap<Pagination<Bid>, Pagination<BidDTO>>().ReverseMap();

        CreateMap<Category, CategoryDTO>().ReverseMap();

        CreateMap<AppUser, AppUserDTO>().ReverseMap();

        CreateMap<Skill, SkillDTO>().ReverseMap();
        CreateMap<Pagination<Skill>, Pagination<SkillDTO>>().ReverseMap();

        CreateMap<ProjectSkill, ProjectSkillDTO>().ReverseMap();
        CreateMap<Pagination<ProjectSkill>, Pagination<ProjectSkillDTO>>().ReverseMap();
        
        CreateMap<UserSkill, UserSkillDTO>().ReverseMap();
        CreateMap<Pagination<UserSkill>, Pagination<UserSkillDTO>>().ReverseMap();
    }
}
