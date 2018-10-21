using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.ViewModels
{
    public class UserIndexListViewModel
    {
        public IEnumerable<SmartUser> Users { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public UserIndexListViewModel()
        {
        }
        public UserIndexListViewModel(IEnumerable<SmartUser> users)
        {
            Users = users;
        }
    }
}
