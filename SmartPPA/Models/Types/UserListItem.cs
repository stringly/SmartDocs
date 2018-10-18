using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.Types
{
    public class UserListItem
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }

        public UserListItem()
        {
        }

        public UserListItem(SmartUser user)
        {
            UserId = user.UserId;
            DisplayName = user.DisplayName;
        }
    }
}
