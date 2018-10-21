using System;
using System.Collections.Generic;

namespace SmartPPA.Data
{
    public partial class Components
    {
        public Components()
        {
            InverseParentComponentComponent = new HashSet<Components>();
            Positions = new HashSet<Positions>();
        }

        public int ComponentId { get; set; }
        public int? ParentComponentComponentId { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }

        public Components ParentComponentComponent { get; set; }
        public ICollection<Components> InverseParentComponentComponent { get; set; }
        public ICollection<Positions> Positions { get; set; }
    }
}
