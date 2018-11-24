using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that represents a SmartDocs application user in the database
    /// </summary>
    public class SmartUser
    {
        /// <summary>
        /// Gets or sets the User identifier.
        /// </summary>
        /// <value>
        /// The User's identifier.
        /// </value>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's BlueDeck identifier.
        /// </summary>
        /// <value>
        /// The SmartUser's associated BlueDeck user id
        /// </value>
        /// <remarks>
        /// Not implemented as of SmartDocs v1.0
        /// </remarks>
        public int BlueDeckId { get; set; }

        /// <summary>
        /// Gets or sets the User's Logon name
        /// </summary>
        /// <value>
        /// The user's logon name
        /// </value>
        /// <remarks>
        /// This is the User's LDAP username
        /// </remarks>
        public string LogonName { get; set; }

        /// <summary>
        /// Gets or sets the user's in-application display name.
        /// </summary>
        /// <value>
        /// The user's in-application display name.
        /// </value>
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
    }
}
