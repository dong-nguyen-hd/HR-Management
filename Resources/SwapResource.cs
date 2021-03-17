using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources
{
    public sealed class SwapResource
    {
        [Required]
        [Display(Name = "Current Id")]
        public int CurrentId { get; set; }

        [Required]
        [Display(Name = "Turned Id")]
        public int TurnedId { get; set; }
    }
}
