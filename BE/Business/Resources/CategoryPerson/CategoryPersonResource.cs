using Business.Resources.Technology;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources.CategoryPerson
{
    public class CategoryPersonResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Order Index")]
        public int OrderIndex { get; set; }

        [Required]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        [Required]
        public List<TechnologyResource> Technology { get; set; } = new List<TechnologyResource>();
    }
}
