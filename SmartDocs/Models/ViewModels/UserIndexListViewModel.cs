using System.Collections.Generic;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// View Model class used to display a list of <see cref="T:SmartDocs.Models.SmartUser"/>
    /// </summary>
    public class UserIndexListViewModel : IndexViewModelBase
    {
        /// <summary>
        /// Gets or sets the list of <see cref="T:SmartDocs.Models.SmartUser"/>.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IEnumerable<SmartUser> Users { get; set; }
        /// <summary>
        /// Optional string that sorts the list by the <see cref="SmartUser.BlueDeckId"/>
        /// </summary>
        public string BlueDeckIdSort { get; set; }
        /// <summary>
        /// Optional string that sorts the list by the <see cref="SmartUser.UserId"/>
        /// </summary>
        public string UserIdSort { get; set; }
        /// <summary>
        /// Optional string that sorts the list by the <see cref="SmartUser.DisplayName"/>
        /// </summary>
        public string DisplayNameSort { get; set; }
        /// <summary>
        /// Optional string that sorts the list by the <see cref="SmartUser.LDAPName"/>
        /// </summary>
        public string LDAPNameSort { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewModels.UserIndexListViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless class constructor
        /// </remarks>
        public UserIndexListViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewModels.UserIndexListViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Class constructor that requires an IEnumerable list of <see cref="T:SmartDocs.Models.SmartUser"/> as a parameter.
        /// </remarks>
        /// <param name="users">An IEnumerable list of <see cref="T:SmartDocs.Models.SmartUser"/>s.</param>
        public UserIndexListViewModel(IEnumerable<SmartUser> users)
        {
            Users = users;
        }
    }
}
