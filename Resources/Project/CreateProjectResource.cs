using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Project
{
    public class CreateProjectResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MaxLength(250)]
        public string Position { get; set; }

        [Required]
        [MaxLength(250)]
        public string Responsibilities { get; set; }

        [Required]
        public int TeamSize { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [MaxLength(250)]
        public string Technology { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
