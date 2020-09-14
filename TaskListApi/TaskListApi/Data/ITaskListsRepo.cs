using System.Collections.Generic;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITaskListsRepo
    {
        IEnumerable<TaskList> GetTaskLists();
        TaskList GetTaskListById(long id);
        void PostTaskList(TaskList taskList);
        void DeleteTaskList(TaskList taskList);
        void SaveChanges();
    }
}
