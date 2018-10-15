using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.Types
{
    public class JobDescriptionListItem
    {
        public int JobId { get; set; }
        public string Rank { get; set; }
        public string JobTitle { get; set; }
        public string DisplayName { get; set; }
        public string Grade { get; set; }
        public string FilePath { get; set; }
    }
}
