using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Category
{
    public class UpdateCategoryResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
