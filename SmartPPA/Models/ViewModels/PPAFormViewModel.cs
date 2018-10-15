using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.ViewModels
{
    public class PPAFormViewModel
    {
        [Display(Name = "First Name:"), StringLength(50), Required]
        public string EmployeeFirstName { get; set; }
        [Display(Name = "Last Name:"), StringLength(50), Required]
        public string EmployeeLastName { get; set; }
        [Display(Name = "ID#"), StringLength(5), Required]
        public string EmployeeDepartmentIdNumber { get; set; }
        [Display(Name = "Payroll ID#"), StringLength(10), Required]
        public string PayrollIdNumber { get; set; }
        [Display(Name = "Choose Class:"), StringLength(50), Required]
        public string ClassTitle { get; set; }
        [Display(Name = "Grade:"), StringLength(10), Required]
        public string JobGrade { get; set; }
        public int EmployeeRank { get; set; }
        [Display(Name = "Department/Division:"), StringLength(50), Required]
        public string DepartmentDivision {get;set;}
        [Display(Name = "D/D Code:"), StringLength(50), Required]
        public string DepartmentDivisionCode { get; set; }
        [Display(Name = "Work Location:"), StringLength(50), Required]
        public string WorkPlaceAddress { get; set; }
        [Display(Name = "Immediate Supervisor"), StringLength(50), Required]
        public string SupervisingEmployeeName { get; set; }
        [Display(Name = "Supervised by Employee:"), StringLength(50), Required]
        public string SupervisedByEmployeeName { get; set; }
        [Display(Name = "Start Date:"), Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date:"), StringLength(50), Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        public JobDescription Job { get; set; }        
        [MaxLength(5000), Required, Display(Name ="Performance Assessment:")]
        public string Comments { get; set; }
        [MaxLength(5000), Required, Display(Name = "Supervisor's Recommendations:")]
        public string Recommendation { get; set; }

        public PPAFormViewModel()
        {
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;
        }
        public PPAFormViewModel(JobDescription _job)
        {
            ClassTitle = _job.ClassTitle;
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;
            Job = _job;
        }
    }
}
