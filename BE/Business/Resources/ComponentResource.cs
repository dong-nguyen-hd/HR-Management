using Business.Extensions.Validation;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources
{
    public class ComponentResource
    {
        [MaxLength(50)]
        [ComponentOrder]
        [Display(Name = "Order Index")]
        public List<int> OrderIndex { get; set; }
    }
}
