using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.CategoryPerson
{
    public class CreateCategoryPersonResource
    {
        [Required]
        public List<int> Technology { get; set; }

        [Required]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Person Id")]
        public int PersonId { get; set; }
    }
}
