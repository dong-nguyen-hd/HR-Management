using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HR_Management.Resources.Location
{
    public class LocationResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

        [Required]
        [MaxLength(250)]
        public string City { get; set; }

        [Required]
        [MaxLength(250)]
        public string Country { get; set; }
    }
}
