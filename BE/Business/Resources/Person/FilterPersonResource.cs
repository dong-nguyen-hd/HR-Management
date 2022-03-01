using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Person
{
    public class FilterPersonResource
    {
        [MaxLength(25)]
        [Display(Name = "Staff Id")]
        public string StaffId { get; set; }

        [MaxLength(500)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Location Id")]
        public int? LocationId { get; set; }
    }
}
