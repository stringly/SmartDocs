using System;
using System.Collections.Generic;

namespace SmartDocs.OldModels
{
    public partial class Templates
    {
        public Templates()
        {
            Ppas = new HashSet<Ppas>();
        }

        public int TemplateId { get; set; }
        public string DocumentName { get; set; }
        public byte[] DataStream { get; set; }

        public ICollection<Ppas> Ppas { get; set; }
    }
}
