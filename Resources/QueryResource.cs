using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources
{
    public class QueryResource
    {
        [Required]
        public int Page { get; set; }

        [Required]
        [Display(Name = "Items Per Page")]
        public int ItemsPerPage { get; set; }
    }
}
