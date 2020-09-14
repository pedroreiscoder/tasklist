using AutoMapper;
using TaskListApi.Dtos;
using TaskListApi.Models;

namespace TaskListApi.Profiles
{
    public class TagsProfile : Profile
    {
        public TagsProfile()
        {
            CreateMap<Tag, TagReadDto>()
                .AfterMap((src, dest) => dest.Count = src.TaskTags != null ? src.TaskTags.Count : 0);
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagUpdateDto, Tag>();
        }
    }
}
