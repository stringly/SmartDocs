using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    /// <summary>
    /// Entity Class that represents an Organizational Component
    /// </summary>
    public class OrganizationComponent
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        [Key]
        public int ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the department code.
        /// </summary>
        /// <value>
        /// The department code.
        /// </value>
        public string DepartmentCode { get; set; }
    }
}
