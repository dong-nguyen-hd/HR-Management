﻿using HR_Management.Extensions.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Certificate
{
    public class UpdateCertificateResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Provider { get; set; }

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
