using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListApi.Dtos;
using TaskListApi.Models;

namespace TaskListApi.Profiles
{
    public class TaskListsProfile : Profile
    {
        public TaskListsProfile()
        {
            CreateMap<TaskList, TaskListReadDto>();
            CreateMap<TaskListCreateDto, TaskList>();
            CreateMap<TaskListUpdateDto, TaskList>();
        }
    }
}
