using System.Collections.Generic;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITasksRepo
    {
        IEnumerable<Task> GetTasks(long taskListId);
        Task GetTaskById(long id);
        void PostTask(Task task);
        void DeleteTask(Task task);
        void SaveChanges();
    }
}
