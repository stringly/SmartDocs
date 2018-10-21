using System;
using System.Collections.Generic;

namespace SmartPPA.Data
{
    public partial class Positions
    {
        public Positions()
        {
            Members = new HashSet<Members>();
        }

        public int PositionId { get; set; }
        public int? ParentComponentComponentId { get; set; }
        public string Name { get; set; }
        public bool IsUnique { get; set; }
        public string JobTitle { get; set; }
        public bool IsManager { get; set; }

        public Components ParentComponentComponent { get; set; }
        public ICollection<Members> Members { get; set; }
    }
}
