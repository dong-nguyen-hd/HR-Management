using System;

namespace HR_Management.Resources
{
    public class EducationResource
    {
        public string CollegeName { get; set; }
        public string Major { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int OrderIndex { get; set; }
        public bool Status { get; set; }
    }
}
