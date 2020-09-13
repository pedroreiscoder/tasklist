using System.Collections.Generic;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITasksRepo
    {
        IEnumerable<Task> GetTasks(long taskListId);
        Task GetTaskById(long id);
        Task PostTask(Task task);
        void PutTask(long id, Task task);
        Task DeleteTask(long id);
    }
}
