using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskListApi.Dtos
{
    public class TaskListReadDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<TaskReadDto> Tasks { get; set; }
    }
}
