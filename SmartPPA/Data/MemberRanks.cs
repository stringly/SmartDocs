using System;
using System.Collections.Generic;

namespace SmartPPA.Data
{
    public partial class MemberRanks
    {
        public MemberRanks()
        {
            Members = new HashSet<Members>();
        }

        public int RankId { get; set; }
        public string RankFullName { get; set; }
        public string RankShort { get; set; }
        public string PayGrade { get; set; }

        public ICollection<Members> Members { get; set; }
    }
}
