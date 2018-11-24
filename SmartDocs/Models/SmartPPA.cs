using System;
using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that represents the data in a PPA document stored in the DB
    /// </summary>
    public class SmartPPA
    {
        /// <summary>
        /// Gets or sets the SmartPPA identifier.
        /// </summary>
        /// <value>
        /// The SmartPPA identifier.
        /// </value>
        [Key]
        public int PPAId { get; set; }

        /// <summary>
        /// Gets or sets the Date the record was created.
        /// </summary>
        /// <value>
        /// The date the record was created.
        /// </value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date the record was last modified.
        /// </summary>
        /// <value>
        /// The date the record was last modified.
        /// </value>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the SmartUser that authored the record.
        /// </summary>
        /// <value>
        /// The SmartUser listed as the record's author.
        /// </value>
        public SmartUser Owner { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:SmartDocs.Models.SmartTemplate"/> on which the document is built.
        /// </summary>
        /// <value>
        /// The <see cref="T:SmartDocs.Models.SmartTemplate"/> template.
        /// </value>
        public SmartTemplate Template { get; set; }

        /// <summary>
        /// Gets or sets the first name of the employee who is the subject of the PPA record.
        /// </summary>
        /// <value>
        /// The first name of the employee.
        /// </value>
        public string EmployeeFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the employee.
        /// </summary>
        /// <value>
        /// The last name of the employee.
        /// </value>
        public string EmployeeLastName { get; set; }

        /// <summary>
        /// Gets or sets the PGPD Department ID number for the subject employee.
        /// </summary>
        /// <value>
        /// The department identifier number.
        /// </value>
        public string DepartmentIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the payroll identifier number for the subject employee.
        /// </summary>
        /// <value>
        /// The payroll identifier number.
        /// </value>
        public string PayrollIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the subject employee's position number.
        /// </summary>
        /// <value>
        /// The position number.
        /// </value>
        public string PositionNumber { get; set; }

        /// <summary>
        /// Gets or sets the subject employee's Department/Division.
        /// </summary>
        /// <value>
        /// The subject employee's Department/Division.
        /// </value>
        public string DepartmentDivision { get; set; }

        /// <summary>
        /// Gets or sets the department division code.
        /// </summary>
        /// <value>
        /// The department division code.
        /// </value>
        public string DepartmentDivisionCode { get; set; }

        /// <summary>
        /// Gets or sets the subject employee's workplace address.
        /// </summary>
        /// <value>
        /// The workplace address of the subject employee.
        /// </value>
        public string WorkplaceAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of any component that is supervised by the subject employee.
        /// </summary>
        /// <value>
        /// The name of any component that the subject employee supervises.
        /// </value>
        public string SupervisedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the Start Date for the rating period documented in the PPA.
        /// </summary>
        /// <value>
        /// The rating period start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date for the rating period documented in the PPA.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the supervisor's assessment comments.
        /// </summary>
        /// <value>
        /// The assessment comments.
        /// </value>
        public string AssessmentComments { get; set; }

        /// <summary>
        /// Gets or sets the supervisor's recommendation comments.
        /// </summary>
        /// <value>
        /// The recommendation comments.
        /// </value>
        public string RecommendationComments { get; set; }

        /// <summary>
        /// Gets or sets the score assigned to Job Description Category 1.
        /// </summary>
        /// <value>
        /// The score assigned to Job Description Category 1.
        /// </value>
        public int? CategoryScore_1 { get; set; }

        /// <summary>
        /// Gets or sets the score assigned to Job Description Category 2.
        /// </summary>
        /// <value>
        /// The score assigned to Job Description Category 2.
        /// </value>
        public int? CategoryScore_2 { get; set; }

        /// <summary>
        /// Gets or sets the score assigned to Job Description Category 3.
        /// </summary>
        /// <value>
        /// The score assigned to Job Description Category 3.
        /// </value>
        public int? CategoryScore_3 { get; set; }

        /// <summary>
        /// Gets or sets the score assigned to Job Description Category 4.
        /// </summary>
        /// <value>
        /// The score assigned to Job Description Category 4.
        /// </value>
        public int? CategoryScore_4 { get; set; }

        /// <summary>
        /// Gets or sets the score assigned to Job Description Category 5.
        /// </summary>
        /// <value>
        /// The score assigned to Job Description Category 5.
        /// </value>
        public int? CategoryScore_5 { get; set; }

        /// <summary>
        /// Gets or sets the score assigned to Job Description Category 6.
        /// </summary>
        /// <value>
        /// The score assigned to Job Description Category 6.
        /// </value>
        public int? CategoryScore_6 { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:SmartDocs.Models.SmartJob"/> associated with this PPA record.
        /// </summary>
        /// <value>
        /// The associated <see cref="T:SmartDocs.Models.SmartJob"/>.
        /// </value>
        public SmartJob Job { get; set;}

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>
        /// The name of the document.
        /// </value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.SmartPPA"/> class.
        /// </summary>
        public SmartPPA()
        {
        }
    }
}
