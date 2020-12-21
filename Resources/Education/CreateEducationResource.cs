using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Education
{
    public class CreateEducationResource
    {
        [Required]
        [MaxLength(250)]
        public string CollegeName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Major { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
