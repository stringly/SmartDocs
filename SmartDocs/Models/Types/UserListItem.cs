namespace SmartDocs.Models.Types
{
    /// <summary>
    /// Class used to populate a dropdown list of <see cref="T:SmartDocs.Models.SmartUser"/>
    /// </summary>
    public class UserListItem
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.Types.UserListItem"/> class.
        /// </summary>
        public UserListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.Types.UserListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor that takes a <see cref="T:SmartDocs.Models.SmartUser"/> parameter.
        /// </remarks>
        /// <param name="user">The user.</param>
        public UserListItem(SmartUser user)
        {
            UserId = user.UserId;
            DisplayName = user.DisplayName;
        }
    }
}
