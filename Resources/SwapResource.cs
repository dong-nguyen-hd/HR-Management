using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HR_Management.Resources
{
    public sealed class SwapResource
    {
        [Required]
        public int CurrentId { get; set; }

        [Required]
        public int TurnedId { get; set; }
    }
}
