using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Person
{
    public class FilterPersonResource
    {
        [MaxLength(25)]
        [Display(Name = "Staff Id")]
        public string StaffId { get; set; }

        [MaxLength(500)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Office Id")]
        public int? OfficeId { get; set; }

        public bool Available { get; set; }

        [Display(Name = "Technology Id")]
        [MaxLength(3)]
        public List<int> TechnologyId { get; set; }
    }
}
