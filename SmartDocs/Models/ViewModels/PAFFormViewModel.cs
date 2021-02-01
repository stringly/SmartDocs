using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// View Model used to Create/Edit A PAF Document
    /// </summary>
    public class PAFFormViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PAFFormViewModel"/> class.
        /// <remarks>
        /// This constructor is used when a new PAF is being created.
        /// </remarks>
        /// </summary>
        public PAFFormViewModel(int authorUserId)
        {
            // default the dates to today and a year ago from today
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;

            AuthorUserId = authorUserId;
            PAFTypeChoices = new List<string>
            {
                "Periodic Performance Assessment",
                "Probationary Midpoint",
                "Rating Justification"
            };
            SelectedPAFType = "Periodic Performance Assessment";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PAFFormViewModel"/> class.
        /// <remarks>
        /// This constructor is used when a PAF is being edited and form data will be populated by <see cref="SmartDocumentClasses.SmartPAFFactory.GetViewModelFromXML"/>.
        /// </remarks>
        /// </summary>
        public PAFFormViewModel()
        {
            // default the dates to today and a year ago from today
            PAFTypeChoices = new List<string>
            {
                "Periodic Performance Assessment",
                "Probationary Midpoint",
                "Rating Justification"
            };
        }

        /// <summary>
        /// Gets or sets the PAF's document Id
        /// </summary>
        /// <remarks>
        /// This is the Id of the <see cref="Models.SmartDocument"/> from which the View Model is built.
        /// </remarks>
        /// <value>
        /// The PAF identifier.
        /// </value>
        public int DocumentId { get; set; }
        /// <summary>
        /// The type of PAF being preparted.
        /// </summary>
        [Display(Name = "Assessment Form Type"), Required]
        /// <summary>
        /// The type of the PAF being prepared, selected from one of three options in the PAFTypeChoices list.
        /// </summary>
        public string SelectedPAFType { get; set; }
        /// <summary>
        /// Gets or sets the First Name of the employee who is the subject of the PAF.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter a First Name.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name of the employee who is the subject of the PAF.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter a Last Name.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Payroll Identifier Number (EIS) of the employee who is the subject of the PAF.
        /// </summary>
        /// <value>
        /// The payroll identifier number.
        /// </value>
        [Display(Name = "Payroll ID#"), StringLength(10, ErrorMessage = "Must be 10 characters or fewer."), Required(ErrorMessage = "A Payroll number is required.")]
        public string PayrollIdNumber { get; set; }
        /// <summary>
        /// Gets or sets the Departmental ID# (EIS) of the employee who is the subject of the PAF.
        /// </summary>
        /// <value>
        /// The employee's departmental Id.
        /// </value>
        [Display(Name = "ID#"), StringLength(10, ErrorMessage = "Must be 10 characters or fewer."), Required(ErrorMessage = "A Payroll number is required.")]
        public string DepartmentIdNumber { get; set; }
        /// <summary>
        /// Gets or sets the Start Date for the rating period of the PAF.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Display(Name = "Start Date:"), Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date for the rating period of the PAF.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = "End Date:"), Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
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
        /// Gets or sets the identifier for the <see cref="SmartJob"/> associated with the <see cref="SmartDocument.SmartDocumentType.PAF"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        [Display(Name = "Job Title"), Required(ErrorMessage = "Please choose a Job Description.")]
        public int JobId { get; set; }
        /// <summary>
        /// Gets or sets the id of the <see cref="T:SmartDocs.Models.SmartUser"/> who is the author of the <see cref="SmartDocument.SmartDocumentType.PAF"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The author user identifier.
        /// </value>
        [Display(Name = "Immediate Supervisor"), Required(ErrorMessage = "Please select your name from the list.")]
        public int AuthorUserId { get; set; }
        /// <summary>
        /// Gets or sets the Assessment narrative field text.
        /// </summary>
        /// <value>
        /// The assessment.
        /// </value>
        [MaxLength(5000, ErrorMessage = "This field is limited to 5000 characters."), Display(Name = "Performance Assessment:")]
        [DataType(DataType.MultilineText)]
        public string Assessment { get; set; }

        /// <summary>
        /// Gets or sets the Recommendation narrative field text.
        /// </summary>
        /// <value>
        /// The recommendation.
        /// </value>
        [MaxLength(5000, ErrorMessage = "This field is limited to 5000 characters."), Display(Name = "Supervisor's Recommendations:")]
        [DataType(DataType.MultilineText)]
        public string Recommendation { get; set; }

        /// <summary>
        /// List of PAF Types used to populate a selection menu
        /// </summary>
        public List<string> PAFTypeChoices { get; set; }

        /// <summary>
        /// Gets or sets the list of Job Descriptions available in the database.
        /// </summary>
        /// <remarks>
        /// This builds a list of <see cref="JobDescriptionListItem"/> from the available <see cref="SmartJob"/>s.
        /// </remarks>
        /// <value>
        /// The job list.
        /// </value>
        public List<JobDescriptionListItem> JobList { get; set; }

        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <remarkds>
        /// This shows a list of <see cref="OrganizationComponent"/> in the database.
        /// </remarkds>
        /// <value>
        /// The components.
        /// </value>
        public List<OrganizationComponent> Components { get; set; }

        /// <summary>
        /// Gets or sets the Users list.
        /// </summary>
        /// <remarks>
        /// This assembles a List of <see cref="UserListItem"/> from the available <see cref="SmartUser"/>s.
        /// </remarks>
        /// <value>
        /// The users.
        /// </value>
        public List<UserListItem> Users { get; set; }

        public void HydrateLists(List<JobDescriptionListItem> jobList, List<OrganizationComponent> components, List<UserListItem> users)
        {
            JobList = jobList;
            Components = components;
            Users = users;
        }
    }
}
