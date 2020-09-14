using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskListApi.Dtos
{
    public class TaskUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        [Required]
        public int Priority { get; set; }

        public DateTime? RemindMeOn { get; set; }

        [MaxLength(10)]
        public string ActivityType { get; set; }

        [Required]
        [MaxLength(4)]
        public string Status { get; set; }

        public List<TagUpdateDto> Tags { get; set; }
    }
}
