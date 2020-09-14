using System;
using System.Collections.Generic;
using System.Linq;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public class TaskListsRepo : ITaskListsRepo
    {
        private readonly AppDbContext _context;

        public TaskListsRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskList> GetTaskLists()
        {
            return _context.TaskLists.ToList();
        }

        public TaskList GetTaskListById(long id)
        {
            return _context.TaskLists.Find(id);
        }

        public void PostTaskList(TaskList taskList)
        {
            _context.TaskLists.Add(taskList);
        }

        public void DeleteTaskList(TaskList taskList)
        {
            _context.TaskLists.Remove(taskList);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
