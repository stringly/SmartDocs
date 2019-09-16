using System;
using System.Collections.Generic;

namespace SmartDocs.OldModels
{
    public partial class Users
    {
        public Users()
        {
            Ppas = new HashSet<Ppas>();
        }

        public int UserId { get; set; }
        public int BlueDeckId { get; set; }
        public string LogonName { get; set; }
        public string DisplayName { get; set; }

        public ICollection<Ppas> Ppas { get; set; }
    }
}
