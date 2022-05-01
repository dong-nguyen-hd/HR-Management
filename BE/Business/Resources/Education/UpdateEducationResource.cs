﻿using Business.Extensions.Validation;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Education
{
    public class UpdateEducationResource
    {
        [Required]
        [MaxLength(250)]
        [Display(Name = "College Name")]
        public string CollegeName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Major { get; set; }

        [Required]
        [StartDate("EndDate")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
    }
}
