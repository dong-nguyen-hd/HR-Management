using HR_Management.Extensions.Validation;
using HR_Management.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources
{
    public class ComponentResource
    {
        [MaxLength(50)]
        [ComponentOrder]
        [Display(Name = "Order Index")]
        public List<eOrder> OrderIndex { get; set; }
    }
}
