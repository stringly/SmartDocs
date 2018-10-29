using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    public class OrganizationComponent
    {
        [Key]
        public int ComponentId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DepartmentCode { get; set; }
    }
}
