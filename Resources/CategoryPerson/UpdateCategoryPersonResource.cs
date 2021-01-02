using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.CategoryPerson
{
    public class UpdateCategoryPersonResource
    {
        [Required]
        public string Technology { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
