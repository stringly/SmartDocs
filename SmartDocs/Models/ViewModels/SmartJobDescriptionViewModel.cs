using SmartDocs.Models.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models.ViewModels
{
    public class SmartJobDescriptionViewModel
    {
        /// <summary>
        /// The SmartDocumentID of the Document from which the model is built
        /// </summary>
        public int DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="T:SmartDocs.Models.SmartUser"/> who is the author.
        /// </summary>
        /// <value>
        /// The author user identifier.
        /// </value>
        [Display(Name = "Immediate Supervisor"), Required(ErrorMessage = "Please select your name from the list.")]
        public int AuthorUserId { get; set; }
        /// <summary>
        /// The First Name of the Employee
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name ="First Name:"), StringLength(50, ErrorMessage = "Must be less than 50 characters."), Required(ErrorMessage = "Please enter a First Name.")]
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the Last Name of the employee.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter a Last Name.")]
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the PGPD Id Number (Badge Number) of the employee who is the subject of the PPA
        /// </summary>
        /// <value>
        /// The department identifier number.
        /// </value>
        [Display(Name = "ID#"), StringLength(5, ErrorMessage = "Must be 5 characters or fewer."), Required(ErrorMessage = "Enter employees ID Number")]
        public string DepartmentIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the Position Number of the employee who is the subject of the PPA.
        /// </summary>
        /// <value>
        /// The position number.
        /// </value>
        [Display(Name = "Position Number"), StringLength(10, ErrorMessage = "Must be 10 characters or fewer."), Required(ErrorMessage = "Enter the employee's Position Number.")]
        public string PositionNumber { get; set; }
        /// <summary>
        /// Gets or sets the Department or Division to which the subject employee is assigned.
        /// </summary>
        /// <value>
        /// The department division.
        /// </value>
        [Display(Name = "Department/Division:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please select the employee's assigned unit.")]
        public string DepartmentDivision { get; set; }

        /// <summary>
        /// Gets or sets the Code for the Department or Division to which the subject employee is assigned.
        /// </summary>
        /// <value>
        /// The department division code.
        /// </value>
        [Display(Name = "Department/Division Code:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter the Department Code.")]
        public string DepartmentDivisionCode { get; set; }

        /// <summary>
        /// Gets or sets the subject employee's work place address.
        /// </summary>
        /// <value>
        /// The work place address.
        /// </value>
        [Display(Name = "Work Location:"), StringLength(100, ErrorMessage = "Must be 100 characters or fewer."), Required(ErrorMessage = "Enter the employee's work address.")]
        public string WorkPlaceAddress { get; set; }

        /// <summary>
        /// Gets or sets the component for which the subject employee is a supervisor.
        /// </summary>
        /// <value>
        /// The supervised by employee.
        /// </value>
        [Display(Name = "Supervised by Employee:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer.")]
        public string SupervisedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the <see cref="T:SmartDocs.Models.SmartJob"/> associated with the <see cref="T:SmartDocs.Models.SmartPPA"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        [Display(Name = "Job Title"), Required(ErrorMessage = "Please choose a Job Description.")]
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:SmartDocs.Models.JobDescription"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public JobDescription job { get; set; }

        /// <summary>
        /// Gets or sets the list of Job Descriptions available in the database.
        /// </summary>
        /// <remarks>
        /// This builds a list of <see cref="T:SmartDocs.Models.Types.JobDescriptionListItem"/> from the available <see cref="T:SmartDocs.Models.SmartJob"/>s.
        /// </remarks>
        /// <value>
        /// The job list.
        /// </value>
        public List<JobDescriptionListItem> JobList { get; set; }

        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <remarkds>
        /// This shows a list of <see cref="T:SmartDocs.Models.OrganizationComponent"/> in the database.
        /// </remarkds>
        /// <value>
        /// The components.
        /// </value>
        public List<OrganizationComponent> Components { get; set; }

        /// <summary>
        /// Gets or sets the Users list.
        /// </summary>
        /// <remarks>
        /// This assembles a List of <see cref="T:SmartDocs.Models.Types.UserListItem"/> from the available <see cref="T:SmartDocs.Models.SmartUser"/>s.
        /// </remarks>
        /// <value>
        /// The users.
        /// </value>
        public List<UserListItem> Users { get; set; }

        public SmartJobDescriptionViewModel()
        {
            Components = new List<OrganizationComponent>();
            Users = new List<UserListItem>();
            JobList = new List<JobDescriptionListItem>();
        }
    }
}
