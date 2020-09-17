using AutoMapper;
using TaskListApi.Profiles;

namespace TaskListApiTests
{
    public static class Mapper
    {
        public static MapperConfiguration Configuration = new MapperConfiguration(cfg => {
            cfg.AddProfile<TagsProfile>();
            cfg.AddProfile<TasksProfile>();
            cfg.AddProfile<TaskListsProfile>();
        });
    }
}
