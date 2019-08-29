using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    public class ServeJobDescriptionViewModel
    {
        public int DocumentId { get;set;}
        [Required(ErrorMessage = "You must enter the employee's first name")]
        public string EmployeeFirstName { get; set; }
        [Required(ErrorMessage = "You must enter the employee's last name")]
        public string EmployeeLastName { get; set;}
        [Required(ErrorMessage = "You must select the employee's current Division")]
        public string DepartmentDivisionName { get; set; }
        [Required(ErrorMessage = "You must provide the employee's current Division Code")]
        public string DepartmentDivisionCode { get; set; }
        [Required(ErrorMessage = "You must provide the employee's Position Number")]
        public string PositionNumber { get; set; }
        [Required(ErrorMessage = "You must provide the employee's Class Title")]
        public string ClassTitle { get; set; }
        [Required(ErrorMessage = "You must provide the employee's Grade")]
        public string Grade { get; set; }
        [Required(ErrorMessage = "You must provide the employee's Working Title")]
        public string WorkingTitle { get; set; }
        [Required(ErrorMessage = "You must provide the employee's current work address")]
        public string PlaceOfWork { get; set; }
        [Required(ErrorMessage = "You must provide the employee's current work hours")]
        public string WorkingHours { get; set; }
        [Required(ErrorMessage = "You must provide the employee's current supervisor")]
        public string Supervisor { get; set; }
        public string Supervises { get; set; }
        [Required(ErrorMessage = "You must select a Job Description")]
        public int JobId { get;set;}
        public List<SelectListItem> Jobs { get;set;}
        public List<SelectListItem> Components { get;set;}
        
    }
}
