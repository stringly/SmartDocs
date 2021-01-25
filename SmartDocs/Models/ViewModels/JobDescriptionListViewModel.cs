using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// Viewmodel to display a list of <see cref="SmartDocs.Models.SmartJob"/> in the repo
    /// </summary>
    public class JobDescriptionListViewModel
    {
        /// <summary>
        /// Gets or sets an IEnumerable list of <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionListViewModeltem"/>.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public IEnumerable<JobDescriptionListViewModeltem> Jobs { get; set; }
    }
}
