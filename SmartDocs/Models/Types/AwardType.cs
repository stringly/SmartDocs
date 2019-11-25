using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.Types
{
    public class AwardType
    {
        [Required, Display(Name = "Award Class")]
        public string AwardClass { get; set; }
        [Required, Display(Name = "Award Name")]
        public string Name { get; set; }
        public string ComponentViewName { get; set;}
        public string Description { get;set; }
        public bool HasRibbon { get; set; }
    }

    public class GoodConductAward: AwardType
    {
        [Display(Name ="Date Eligibility Confirmed by IAD"), Required]
        public DateTime EligibilityConfirmationDate { get; set; }

        public GoodConductAward()
        {
            EligibilityConfirmationDate = DateTime.Now;
            ComponentViewName = "GoodConduct";
            AwardClass = "Special Achievement Award";
            Name = "Good Conduct Award";
            HasRibbon = true;
            Description = "To be eligible for the Good Conduct Award, the employee must have received an overall rating of at least 'Exceeds Satisfactory' on the last two appraisals, and has not received any sustained disciplinary action within the last 24 months. Documentation confirming the employee's eligibility must be obtained from IAD and included with the Nomination Form.";
        }
    }
    public class OutstandingPerformanceAward: AwardType
    {
        [Display(Name = "Start Date of Eligibility Period"), Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date of Eligibility Period"), Required]
        public DateTime EndDate { get; set; }
        [Required, Display(Name = "Select the type of Outstanding Performance Award")]
        public int SelectedAwardType { get;set;}
        public List<SelectListItem> AwardTypes { get;set;}

        public OutstandingPerformanceAward()
        {
            EndDate = DateTime.Now;
            StartDate = EndDate.AddYears(-1);
            ComponentViewName = "Exemplary";
            AwardClass = "Exemplary Performance Award";
            Name = "Outstanding Performance Award";
            HasRibbon = false;
            Description = "To be eligible for the Outstanding Performance Award, the employee must have received at minimum a rating of 'Exceeds Satisfactory' on the last appraisal. See the options below for more detail.";
            AwardTypes = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Employee has received an overall rating of 'Exceeds Satisfactory' on the annual appraisal (Awards 8 Hours Annual Leave)",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Employee has received an overall rating of 'Outstanding' on the annual appraisal (Awards 16 Hours Annual Leave)",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "Employee has received an overall rating of 'Outstanding' on the past two consecutive appraisals (Awards 24 Hours Annual Leave)",
                    Value = "3"
                }
            };
        }
    }
}
