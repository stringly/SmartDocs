using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.ViewModels
{
    public class MainFormViewModel
    {
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeDepartmentIdNumber { get; set; }
        public string PayrollIdNumber { get; set; }
        public string ClassTitle { get; set; }
        public string JobGrade { get; set; }
        public int EmployeeRank { get; set; }
        public string DepartmentDivision {get;set;}
        public string DepartmentDivisionCode { get; set; }
        public string WorkPlaceAddress { get; set; }
        public string SupervisingEmployeeName { get; set; }
        public string SupervisedByEmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public JobDescription Job { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Comments { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Recommendation { get; set; }

        public MainFormViewModel()
        {

        }
        public MainFormViewModel(JobDescription job)
        {
            ClassTitle = job.ClassTitle;

        }
    }
}
