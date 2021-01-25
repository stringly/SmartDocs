using System;
using System.Collections.Generic;

namespace SmartDocs.OldModels
{
    public partial class Ppas
    {
        public int Ppaid { get; set; }
        public int? OwnerUserId { get; set; }
        public int? TemplateId { get; set; }
        public int? JobId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string DocumentName { get; set; }
        public string AssessmentComments { get; set; }
        public int? CategoryScore1 { get; set; }
        public int? CategoryScore2 { get; set; }
        public int? CategoryScore3 { get; set; }
        public int? CategoryScore4 { get; set; }
        public int? CategoryScore5 { get; set; }
        public int? CategoryScore6 { get; set; }
        public string DepartmentDivision { get; set; }
        public string DepartmentDivisionCode { get; set; }
        public string DepartmentIdNumber { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime EndDate { get; set; }
        public string PayrollIdNumber { get; set; }
        public string PositionNumber { get; set; }
        public string RecommendationComments { get; set; }
        public DateTime StartDate { get; set; }
        public string SupervisedByEmployee { get; set; }
        public string WorkplaceAddress { get; set; }

        public Jobs Job { get; set; }
        public Users OwnerUser { get; set; }
        public Templates Template { get; set; }
    }
}
