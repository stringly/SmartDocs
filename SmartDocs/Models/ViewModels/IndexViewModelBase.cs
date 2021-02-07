using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    public abstract class IndexViewModelBase
    {
        public string CurrentSort { get; set; }
        [Display(Name = "Search")]
        public string CurrentFilter { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
