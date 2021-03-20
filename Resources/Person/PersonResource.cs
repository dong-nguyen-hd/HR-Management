using HR_Management.Extensions;
using HR_Management.Extensions.Validation;
using HR_Management.Infrastructure;
using HR_Management.Resources.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Person
{
    public class PersonResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Staff Id")]
        public string StaffId { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [MaxLength(500)]
        public string Email { get; set; }

        public Uri Avatar { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Phone]
        [MaxLength(25)]
        public string Phone { get; set; }

        [Required]
        [DoB]
        [DataType(DataType.Date)]
        [JsonConverter(typeof(DateTimeConverter))]
        [Display(Name = "Year Of Birth")]
        public DateTime YearOfBirth { get; set; }

        [Required]
        [Gender]
        public eGender Gender { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Order Index")]
        public List<int> OrderIndex { get; set; }

        public LocationResource Location { get; set; }
    }
}
