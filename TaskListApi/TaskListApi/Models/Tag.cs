using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskListApi.Models
{
    public class Tag
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<TaskTag> TaskTags { get; set; }
    }
}
