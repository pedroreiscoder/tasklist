using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public class TaskTagsRepo : ITaskTagsRepo
    {
        private readonly AppDbContext _context;

        public TaskTagsRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskTag> GetTaskTagsByTask(long taskId)
        {
            return _context.TaskTags.Where(tt => tt.TaskId == taskId).ToList();
        }

        public void PostTaskTag(TaskTag taskTag)
        {
            _context.TaskTags.Add(taskTag);
        }

        public void PostTaskTags(IEnumerable<TaskTag> taskTags)
        {
            _context.TaskTags.AddRange(taskTags);
        }

        public void DeleteTaskTags(IEnumerable<TaskTag> taskTags)
        {
            _context.TaskTags.RemoveRange(taskTags);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
