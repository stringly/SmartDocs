using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// View Model class used to display a list of <see cref="T:SmartDocs.Models.SmartUser"/>
    /// </summary>
    public class UserIndexListViewModel
    {
        /// <summary>
        /// Gets or sets the list of <see cref="T:SmartDocs.Models.SmartUser"/>.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IEnumerable<SmartUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the current filter string being applied to the list.
        /// </summary>
        /// <value>
        /// The current filter.
        /// </value>
        public string CurrentFilter { get; set; }

        /// <summary>
        /// Gets or sets the current sort string being applied to the list.
        /// </summary>
        /// <value>
        /// The current sort.
        /// </value>
        public string CurrentSort { get; set; }

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
