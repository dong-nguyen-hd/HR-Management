using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.CategoryPerson
{
    public class CreateCategoryPersonResource
    {
        [Required]
        [MaxLength(250)]
        public string Technology { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
