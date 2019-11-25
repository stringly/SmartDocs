using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.Types
{
    public class AwardType
    {
        public string AwardClass { get; set; }
        public string Name { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public double MoneyBonus { get; set; }
        public bool HasRibbon { get; set; }
    }
}
