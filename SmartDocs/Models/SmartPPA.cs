using SmartDocs.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SmartDocs.Models
{
    public class SmartPPA
    {
        [Key]
        public int PPAId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public SmartUser Owner { get; set; }
        public SmartTemplate Template { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string DepartmentIdNumber { get; set; }
        public string PayrollIdNumber { get; set; }
        public string PositionNumber { get; set; }
        public string DepartmentDivision { get; set; }
        public string DepartmentDivisionCode { get; set; }
        public string WorkplaceAddress { get; set; }
        public string SupervisedByEmployee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AssessmentComments { get; set; }
        public string RecommendationComments { get; set; }
        public int? CategoryScore_1 { get; set; }
        public int? CategoryScore_2 { get; set; }
        public int? CategoryScore_3 { get; set; }
        public int? CategoryScore_4 { get; set; }
        public int? CategoryScore_5 { get; set; }
        public int? CategoryScore_6 { get; set; }
        public SmartJob Job { get; set;}
        public string DocumentName { get; set; }

        public SmartPPA()
        {
        }


    }
}
