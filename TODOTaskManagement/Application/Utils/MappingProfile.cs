using Application.DTOs;
using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskEntity, TaskDTO>().ReverseMap();
            CreateMap<CreateTaskCommand, TaskEntity>().ReverseMap();
        }
    }
}
