using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITaskTagsRepo
    {
        void PostTaskTag(TaskTag taskTag);
        void DeleteTaskTags(IEnumerable<TaskTag> taskTags);
        void SaveChanges();
    }
}
