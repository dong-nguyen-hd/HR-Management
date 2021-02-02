using HR_Management.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Education
{
    public class EducationResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "College Name")]
        public string CollegeName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Major { get; set; }

        [Required]
        [JsonConverter(typeof(DateTimeConverter))]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Order Index")]
        public int OrderIndex { get; set; }
    }
}
