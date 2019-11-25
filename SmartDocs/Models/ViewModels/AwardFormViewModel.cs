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
        [Display(Name = "Nominee's Agency"), Required]
        public string AgencyName { get; set; }
        
        [Display(Name = "Nominee's Name"), Required]
        public string NomineeName { get; set; }

        public string ClassTitle { get; set; }

        public string Division { get; set; }

        public List<OrganizationComponent> Components { get; set; }
        public List<SmartUser> Users { get; set; }        
        public List<AwardType> Awards { get; set; }

    }
}
