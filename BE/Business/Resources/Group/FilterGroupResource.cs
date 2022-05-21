using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Group
{
    public class FilterGroupResource
    {
        [MaxLength(25)]
        public string Name { get; set; }

        public bool Available { get; set; }
    }
}
