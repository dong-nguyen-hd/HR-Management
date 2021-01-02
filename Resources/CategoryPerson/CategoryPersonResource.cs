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
        public int OrderIndex { get; set; }

        [Required]
        public List<string> Technology { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
