using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Category
{
    public class CreateCategoryResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
