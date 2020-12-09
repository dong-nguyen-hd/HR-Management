using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources
{
    public class CreateEducationResource
    {
        [Required]
        public string CollegeName { get; set; }
        [Required]
        public string Major { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
