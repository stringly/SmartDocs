using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public interface IDocumentRepository
    {   
        
        IEnumerable<SmartUser> Users { get; }        
        IEnumerable<SmartTemplate> Templates { get; }
        IEnumerable<SmartJob> Jobs { get; }
        IEnumerable<SmartPPA> PPAs { get; }

        
        void SaveJob(SmartJob job);
        // So Add the record here
        void SaveSmartPPA(SmartPPA ppa);
        SmartUser GetCurrentUser();
    }
}
