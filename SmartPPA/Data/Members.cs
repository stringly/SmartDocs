using System;
using System.Collections.Generic;

namespace SmartPPA.Data
{
    public partial class Members
    {
        public int MemberId { get; set; }
        public int? RankId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public int? PositionId { get; set; }

        public Positions Position { get; set; }
        public MemberRanks Rank { get; set; }
    }
}
