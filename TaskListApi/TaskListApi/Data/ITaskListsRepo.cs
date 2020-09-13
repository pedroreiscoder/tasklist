using System.Collections.Generic;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITaskListsRepo
    {
        IEnumerable<TaskList> GetTaskLists();
        TaskList GetTaskListById(long id);
        TaskList PostTaskList(TaskList taskList);
        void PutTaskList(long id, TaskList taskList);
        TaskList DeleteTaskList(long id);
    }
}
