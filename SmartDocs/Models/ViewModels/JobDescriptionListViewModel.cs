using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    public class JobDescriptionListViewModel
    {
        public IEnumerable<JobDescriptionListViewModeltem> Jobs { get; set; }
    }
}
