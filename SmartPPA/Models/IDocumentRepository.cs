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
        void RemoveJob(SmartJob job);
        void SaveTemplate(SmartTemplate template);
        void SaveSmartPPA(SmartPPA ppa);
        void RemoveSmartPPA(SmartPPA ppa);
        void SaveUser(SmartUser user);
        void RemoveUser(SmartUser user);

        SmartUser GetCurrentUser();
    }
}
