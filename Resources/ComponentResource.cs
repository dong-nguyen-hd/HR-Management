using HR_Management.Extensions.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources
{
    public class ComponentResource
    {
        [MaxLength(50)]
        [ComponentOrder]
        [Display(Name = "Order Index")]
        public List<int> OrderIndex { get; set; }
    }
}
