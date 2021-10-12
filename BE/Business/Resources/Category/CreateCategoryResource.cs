using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Category
{
    public class CreateCategoryResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
