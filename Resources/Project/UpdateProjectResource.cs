using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Project
{
    public class UpdateProjectResource
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
        [Display(Name = "Team Size")]
        public int TeamSize { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        public List<int> Technology { get; set; }
    }
}
