using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPPA.Data;

namespace SmartPPA.Models.Types
{
    public class ComponentListItem
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }

        public ComponentListItem()
        {
        }
        public ComponentListItem(Components c)
        {
            ComponentId = c.ComponentId;
            ComponentName = c.Name;
        }
    }
}
