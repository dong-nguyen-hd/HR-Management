using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Category
{
    public class CategoryResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
