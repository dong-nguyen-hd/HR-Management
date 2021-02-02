using HR_Management.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Project
{
    public class ProjectResource
    {
        [Required]
        public int Id { get; set; }

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
        [JsonConverter(typeof(DateTimeConverter))]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Order Index")]
        public int OrderIndex { get; set; }

        [Required]
        public List<Dictionary<int, string>> Technology { get; set; } = new List<Dictionary<int, string>>();
    }
}
