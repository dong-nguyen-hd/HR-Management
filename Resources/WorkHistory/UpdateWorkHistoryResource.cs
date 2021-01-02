using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.WorkHistory
{
    public class UpdateWorkHistoryResource
    {
        [Required]
        [MaxLength(250)]
        public string Position { get; set; }

        [Required]
        [MaxLength(250)]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}
