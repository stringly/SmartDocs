using System.Collections.Generic;

namespace SmartDocs.Models
{
    /// <summary>
    /// Interface that defines Database Interactions
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Gets the Users
        /// </summary>
        IEnumerable<SmartUser> Users { get; }        
        /// <summary>
        /// Gets the Templates
        /// </summary>
        IEnumerable<SmartTemplate> Templates { get; }
        /// <summary>
        /// Gets the Documents
        /// </summary>
        IEnumerable<SmartDocument> Documents { get; }
        /// <summary>
        /// Gets the Jobs
        /// </summary>
        IEnumerable<SmartJob> Jobs { get; }
        /// <summary>
        /// Gets the Organization Units.
        /// </summary>
        IEnumerable<OrganizationUnit> Units { get; }
        /// <summary>
        /// Gets the Performance Appraisal Forms.
        /// </summary>
        IEnumerable<SmartDocument> PerformanceAppraisalForms { get;}        
        /// <summary>
        /// Gets the Perfomance Assessment Forms.
        /// </summary>
        IEnumerable<SmartDocument> PerformanceAssessmentForms { get; }
        /// <summary>
        /// Gets the Award Forms.
        /// </summary>
        IEnumerable<SmartDocument> AwardForms { get; }
        /// <summary>
        /// Gets the Job Description forms.
        /// </summary>
        IEnumerable<SmartDocument> JobDescriptionForms { get; }

        /// <summary>
        /// Saves a <see cref="SmartJob"/>
        /// </summary>
        /// <param name="job">The <see cref="SmartJob"/> to save.</param>
        void SaveJob(SmartJob job);
        /// <summary>
        /// Removes a <see cref="SmartJob"/>
        /// </summary>
        /// <param name="job">The <see cref="SmartJob"/> to remove.</param>
        void RemoveJob(SmartJob job);
        /// <summary>
        /// Saves a <see cref="SmartTemplate"/>
        /// </summary>
        /// <param name="template">The <see cref="SmartTemplate"/> to save.</param>
        void SaveTemplate(SmartTemplate template);
        /// <summary>
        /// Saves a <see cref="SmartDocument"/>
        /// </summary>
        /// <param name="doc">The saved <see cref="SmartDocument"/></param>
        /// <returns></returns>
        SmartDocument SaveSmartDoc(SmartDocument doc);
        /// <summary>
        /// Removes a <see cref="SmartDocument"/>
        /// </summary>
        /// <param name="doc">The <see cref="SmartDocument"/> to remove.</param>
        void RemoveSmartDoc(SmartDocument doc);
        /// <summary>
        /// Saves a <see cref="SmartUser"/>
        /// </summary>
        /// <param name="user">The <see cref="SmartUser"/> to save.</param>
        void SaveUser(SmartUser user);
        /// <summary>
        /// Removes a <see cref="SmartUser"/>
        /// </summary>
        /// <param name="user">The <see cref="SmartUser"/> to remove.</param>
        void RemoveUser(SmartUser user);
        /// <summary>
        /// Saves a <see cref="OrganizationUnit"/>
        /// </summary>
        /// <param name="unit">The <see cref="OrganizationUnit"/> to save.</param>
        void SaveUnit(OrganizationUnit unit);
        /// <summary>
        /// Removes a <see cref="OrganizationUnit"/>
        /// </summary>
        /// <param name="unit">The <see cref="OrganizationUnit"/> to remove.</param>
        void RemoveUnit(OrganizationUnit unit);
        /// <summary>
        /// Retrieves a <see cref="SmartUser"/> via logon name.
        /// </summary>
        /// <param name="logonName">The logon name of the user.</param>
        /// <returns>A <see cref="SmartUser"/></returns>
        SmartUser GetUserByLogonName(string logonName);
        /// <summary>
        /// Gets the list of Documents for the User.
        /// </summary>
        IEnumerable<SmartDocument> GetDocumentsForUser(int userId)
    }
}
