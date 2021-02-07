using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// Viewmodel to display a list of <see cref="SmartJob"/> in the repo
    /// </summary>
    public class JobDescriptionListViewModel : IndexViewModelBase
    {
        public string NameSort { get; set; }
        public string RankSort { get; set; }
        public string GradeSort { get; set; }
        public string SelectedRank { get; set; }
        public string SelectedGrade { get; set; }
        public List<SelectListItem> Ranks { get; set; }
        public List<SelectListItem> Grades { get; set; }
        /// <summary>
        /// Gets or sets an IEnumerable list of <see cref="JobDescriptionListViewModeltem"/>.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public IEnumerable<JobDescriptionListViewModeltem> Jobs { get; set; }
        public void HydrateLists(List<string> ranks, List<string> grades)
        {
            Ranks = ranks
                .OrderBy(x => x)
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x, Value = x });
            Grades = grades
                .OrderBy(x => x)
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x, Value = x });
        }
    }
}
