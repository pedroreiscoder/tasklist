using System.ComponentModel.DataAnnotations;

namespace TaskListApi.Dtos
{
    public class TagCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
