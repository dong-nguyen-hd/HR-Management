using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Location
{
    public class UpdateLocationResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
    }
}
