using HR_Management.Resources.CategoryPerson;
using HR_Management.Resources.Certificate;
using HR_Management.Resources.Education;
using HR_Management.Resources.Person;
using HR_Management.Resources.Project;
using HR_Management.Resources.WorkHistory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Information
{
    public class InformationResource
    {
        public PersonResource Person { get; set; }

        [Display(Name = "Work History")]
        public List<WorkHistoryResource> WorkHistory { get; set; } = new List<WorkHistoryResource>();

        [Display(Name = "Category Person")]
        public List<CategoryPersonResource> CategoryPerson { get; set; } = new List<CategoryPersonResource>();
        public List<EducationResource> Education { get; set; } = new List<EducationResource>();
        public List<CertificateResource> Certificate { get; set; } = new List<CertificateResource>();
        public List<ProjectResource> Project { get; set; } = new List<ProjectResource>();
    }
}
