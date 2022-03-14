using Business.Data;
using Business.Extensions.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Business.Resources.Person
{
    public class CreatePersonResource
    {
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

        [MaxLength(500)]
        public string Description { get; set; }

        [Phone]
        [MaxLength(25)]
        public string Phone { get; set; }

        [Required]
        [DoB]
        [DataType(DataType.Date)]
        [Display(Name = "Year Of Birth")]
        public DateTime YearOfBirth { get; set; }

        [Required]
        [Gender]
        public eGender Gender { get; set; }

        [Display(Name = "Location Id")]
        public int OfficeId { get; set; }

        [JsonIgnore]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
    }
}
