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
            return _context.Tasks.Where(t => t.TaskListId == taskListId).ToList();
        }

        public Models.Task GetTaskById(long id)
        {
            return _context.Tasks.Find(id);
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
