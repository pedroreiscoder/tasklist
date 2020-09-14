using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskListApi.Dtos
{
    public class TaskReadDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        public string Notes { get; set; }
        
        public int Priority { get; set; }

        public DateTime? RemindMeOn { get; set; }
        
        public string ActivityType { get; set; }
        
        public string Status { get; set; }
        
        public long TaskListId { get; set; }

        public List<TagReadDto> Tags { get; set; }
    }
}
