using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITaskTagsRepo
    {
        IEnumerable<TaskTag> GetTaskTagsByTask(long taskId);
        void PostTaskTag(TaskTag taskTag);
        void PostTaskTags(IEnumerable<TaskTag> taskTags);
        void DeleteTaskTags(IEnumerable<TaskTag> taskTags);
        void SaveChanges();
    }
}
