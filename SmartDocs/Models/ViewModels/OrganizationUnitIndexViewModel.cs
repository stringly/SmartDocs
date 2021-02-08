using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// View model class used in the OrganizationUnit/Index view. Inherits <see cref="IndexViewModelBase"/>
    /// </summary>
    public class OrganizationUnitIndexViewModel : IndexViewModelBase
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public OrganizationUnitIndexViewModel()
        {
            Units = new List<OrganizationUnit>();
        }
        /// <summary>
        /// Optional string parameter to filter the list by division.
        /// </summary>
        public string SelectedDivision { get; set; }
        /// <summary>
        /// Optional string parameter to filter the list by bureau.
        /// </summary>
        public string SelectedBureau { get; set; }
        /// <summary>
        /// String parameter used to order the list by Title.
        /// </summary>
        public string TitleSort { get; set; }
        /// <summary>
        /// String parameter used to order the list by Address.
        /// </summary>
        public string AddressSort { get; set; }
        /// <summary>
        /// String parameter used to order the list by Code.
        /// </summary>
        public string CodeSort { get; set; }
        /// <summary>
        /// String parameter used to order the list by Division.
        /// </summary>
        public string DivisionSort { get; set; }
        /// <summary>
        /// String parameter used to order the list by Bureau.
        /// </summary>
        public string BureauSort { get; set; }
        /// <summary>
        /// List of Divisions
        /// </summary>
        public List<SelectListItem> Divisions { get; set; }
        /// <summary>
        /// List of Bureaus
        /// </summary>
        public List<SelectListItem> Bureaus { get; set; }
        /// <summary>
        /// The list of <see cref="OrganizationUnit"/>
        /// </summary>
        public List<OrganizationUnit> Units { get; set; }
        /// <summary>
        /// Populates the select list properties of the model.
        /// </summary>
        /// <param name="divisions">A list of string names of divisions.</param>
        /// <param name="bureaus">A list of string names of bureaus.</param>
        public void HydrateLists(List<string> divisions, List<string> bureaus)
        {
            Bureaus = bureaus
                .OrderBy(x => x)
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x, Value = x });
            Divisions = divisions
                .OrderBy(x => x)
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x, Value = x });
        }
    }
}
