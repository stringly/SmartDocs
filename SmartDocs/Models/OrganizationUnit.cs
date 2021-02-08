using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    /// <summary>
    /// Entity Class that represents an Organizational Component
    /// </summary>
    public class OrganizationUnit
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Title { get; set; }

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
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the Unit's Division Name.
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// Gets or sets the Unit's Bureau Name.
        /// </summary>
        public string Bureau { get; set; }
    }
}
