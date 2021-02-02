using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.CategoryPerson
{
    public class CategoryPersonResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Order Index")]
        public int OrderIndex { get; set; }

        [Required]
        public List<Dictionary<int, string>> Technology { get; set; } = new List<Dictionary<int, string>>();

        [Required]
        public string Category { get; set; }
    }
}
