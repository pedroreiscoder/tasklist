using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskListApi.Data
{
    public class TasksRepo : ITasksRepo
    {
        private readonly AppDbContext _context;

        public TasksRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Task> GetTasks(long taskListId)
        {
            return _context.Tasks
                .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
                .Where(t => t.TaskListId == taskListId)
                .ToList();
        }

        public Models.Task GetTaskById(long id)
        {
            return _context.Tasks
                .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefault(t => t.Id == id);
        }

        public void PostTask(Models.Task task)
        {
            _context.Tasks.Add(task);
        }

        public void DeleteTask(Models.Task task)
        {
            _context.Tasks.Remove(task);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
