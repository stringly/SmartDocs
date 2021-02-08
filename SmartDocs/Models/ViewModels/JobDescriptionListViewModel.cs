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
        /// <summary>
        /// Optional string parameter to sort the list by name.
        /// </summary>
        public string NameSort { get; set; }
        /// <summary>
        /// Optional string parameter to sort the list by rank.
        /// </summary>
        public string RankSort { get; set; }
        /// <summary>
        /// Optional string parameter to sort the list by grade.
        /// </summary>
        public string GradeSort { get; set; }
        /// <summary>
        /// Optional string parameter to filter the list by rank.
        /// </summary>
        public string SelectedRank { get; set; }
        /// <summary>
        /// Optional string parameter to filter the list by grade.
        /// </summary>
        public string SelectedGrade { get; set; }
        /// <summary>
        /// List of ranks.
        /// </summary>
        public List<SelectListItem> Ranks { get; set; }
        /// <summary>
        /// List of grades.
        /// </summary>
        public List<SelectListItem> Grades { get; set; }
        /// <summary>
        /// Gets or sets an IEnumerable list of <see cref="JobDescriptionListViewModeltem"/>.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public IEnumerable<JobDescriptionListViewModeltem> Jobs { get; set; }
        /// <summary>
        /// Populates the list of ranks and grades.
        /// </summary>
        /// <param name="ranks">A list of string names of ranks.</param>
        /// <param name="grades">A list of string names of grades.</param>
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
