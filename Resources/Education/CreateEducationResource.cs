using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Education
{
    public class CreateEducationResource
    {
        [Required]
        [MaxLength(255)]
        public string CollegeName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Major { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public int PersonId { get; set; }
    }
}
