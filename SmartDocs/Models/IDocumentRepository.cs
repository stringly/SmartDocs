using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models
{
    /// <summary>
    /// Interface that encompasses Database Interactions
    /// </summary>
    public interface IDocumentRepository
    {
        IEnumerable<SmartUser> Users { get; }        
        IEnumerable<SmartTemplate> Templates { get; }
        IEnumerable<SmartDocument> Documents { get; }
        IEnumerable<SmartJob> Jobs { get; }
        IEnumerable<SmartDocument> PPAs { get;}
        IEnumerable<OrganizationComponent> Components { get; }

        
        void SaveJob(SmartJob job);
        void RemoveJob(SmartJob job);
        void SaveTemplate(SmartTemplate template);
        SmartDocument SaveSmartDoc(SmartDocument doc);
        void RemoveSmartDoc(SmartDocument doc);
        void SaveUser(SmartUser user);
        void RemoveUser(SmartUser user);
        void SaveComponent(OrganizationComponent component);
        void RemoveComponent(OrganizationComponent component);

        SmartUser GetUserByLogonName(string logonName);
    }
}
