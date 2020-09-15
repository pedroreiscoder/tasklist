using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListApi.Dtos;

namespace TaskListApi.Profiles
{
    public class TasksProfile : Profile
    {
        public TasksProfile()
        {
            CreateMap<Models.Task, TaskReadDto>();
            CreateMap<TaskCreateDto, Models.Task>();
            CreateMap<TaskUpdateDto, Models.Task>().ReverseMap();
            CreateMap<TaskListTaskCreateDto, Models.Task>();
        }
    }
}
