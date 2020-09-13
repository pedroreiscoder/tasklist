using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskListApi.Models
{
    public class TaskList
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
