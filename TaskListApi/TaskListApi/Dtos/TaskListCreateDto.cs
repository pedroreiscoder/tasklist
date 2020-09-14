using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskListApi.Dtos
{
    public class TaskListCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<TaskListTaskCreateDto> Tasks { get; set; }
    }
}
