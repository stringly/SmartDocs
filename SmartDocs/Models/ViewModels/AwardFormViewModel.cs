using Microsoft.AspNetCore.Mvc.Rendering;
using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// Viewmodel to serve CRUD views for award forms
    /// </summary>
    /// <remarks>
    /// I think I'll use the ghetto combobox method and populate the VM Bound string fields for Class and Division from a drop-down with data annotations.
    /// </remarks>
    public class AwardFormViewModel
    {
        public int DocumentId { get; set; }
        public int AuthorUserId { get; set; }
        [Display(Name = "Nominee's Agency"), Required]
        public string AgencyName { get; set; }
        
        [Display(Name = "Nominee's Name"), Required]
        public string NomineeName { get; set; }
        [Display(Name = "Nominee's Class Title"), Required]
        public string ClassTitle { get; set; }
        [Display(Name = "Nominee's District/Division"), Required]
        public string Division { get; set; }

        public List<OrganizationComponent> Components { get; set; }
        public List<SmartUser> Users { get; set; }
        [Display(Name = "Select Award")]
        public List<AwardSelectListOption> AwardList { get; set; }
        [Required]
        public AwardType Award { get; set; }

        public AwardFormViewModel()
        {
            AwardList = new List<AwardSelectListOption>
            {
                new AwardSelectListOption
                {
                    Text = "Good Conduct Award",
                    Value = "1",
                    SubText = "(No Sustained Discipline in past 24 months.)"
                    
                },
                new AwardSelectListOption
                {
                    Text = "Exemplary Performance",
                    Value = "2",
                    SubText = "(Exceeds Satisfactory or above on appraisal)"
                },
            };
        }
    }

    public class AwardSelectListOption : SelectListItem {
        public string SubText { get; set; }
    }
}
