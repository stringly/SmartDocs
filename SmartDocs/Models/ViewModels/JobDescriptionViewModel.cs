using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// View Model for Job Description screens: Create/Edit
    /// </summary>
    /// <remarks>
    /// This View Model displays a single Job Description
    /// </remarks>
    public class JobDescriptionViewModel
    {
        /// <summary>
        /// Gets or sets the Job Description Identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the Job's Working Title.
        /// </summary>
        /// <remarks>
        /// Working title is "Patrol Officer" or "NED Investigator," which is distinct from the Jobs' Rank or Class Title
        /// </remarks>
        /// <value>
        /// The working title.
        /// </value>
        [Display(Name = "Working Title"), StringLength(50), Required]
        public string WorkingTitle { get; set; }

        /// <summary>
        /// Gets or sets the Grade for the Job Description.
        /// </summary>
        /// <remarks>
        /// The grade is the PG Classified Position designation, such as "L01" or "P10"
        /// </remarks>
        /// <value>
        /// The grade.
        /// </value>
        [Display(Name = "Grade"), StringLength(50), Required]
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the working hours for the specified Job.
        /// </summary>
        /// <remarks>
        /// The vast majority of the Jobs have "Varied" working hours. In the event a job has specific hours,
        /// they should be entered here in plaintext. They will be displayed on generated documents exactly as entered.
        /// </remarks>
        /// <value>
        /// The working hours.
        /// </value>
        [Display(Name = "Working Hours"), StringLength(50), Required]
        public string WorkingHours { get; set; }

        /// <summary>
        /// Gets or sets the rank associated with the Job.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        [Display(Name = "Rank"), StringLength(50), Required]
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <remarks>
        /// This was deprecated prior to 1.0, when persistance was moved to SQL Server
        /// </remarks>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the list of categories in the Job Description.
        /// </summary>
        /// <remarks>
        /// This is a list of <see cref="T:SmartDocs.Models.Types.JobDescriptionCategory"/>
        /// </remarks>
        /// <value>
        /// The categories.
        /// </value>
        public List<JobDescriptionCategory> Categories { get; set; }

        /// <summary>
        /// Gets or sets the ranks.
        /// </summary>
        /// <remarks>
        /// This list is used to populate a Rank selection drop-down list on the View
        /// </remarks>
        /// <value>
        /// The ranks.
        /// </value>
        public List<string> Ranks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless class constructor.
        /// </remarks>
        public JobDescriptionViewModel()
        {
            Categories = new List<JobDescriptionCategory>();
            WorkingHours = "Varied";
            Ranks = DefaultRankList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Class Constructor that takes a <see cref="SmartDocs.Models.SmartJob"/> parameter.
        /// </remarks>
        /// <param name="job">A <see cref="SmartDocs.Models.SmartJob"/>.</param>
        public JobDescriptionViewModel(SmartJob job)
        {
            JobId = job.JobId;
            // populate the Rank List via the DefaultRankList method below
            Ranks = DefaultRankList();
                      

            // create an empty list of JobDescription Categories to be populated from DB Job XML column data
            List<JobDescriptionCategory> results = new List<JobDescriptionCategory>();
            // retrieve the XML data column for the job to parse for Job Data
            XElement root = job.JobDataXml;
            
            // assign the simple values
            WorkingTitle = root.Element("WorkingTitle").Value;
            Grade = root.Element("Grade").Value;
            WorkingHours = root.Element("WorkingHours").Value;
            Rank = root.Element("Rank").Value;

            // pull all of the Categories from the root element
            // categories are stored in the root's "Categories" child, which contains "Category" children
            IEnumerable<XElement> CategoryList = root.Element("Categories").Elements("Category");

            // loop through the list of Categories and build a JobDescriptionCategory class object for each
            foreach (XElement category in CategoryList)
            {
                // create a new JobDescriptionCategory object, and set the Letter, Weight, and Title from the XElement's children
                JobDescriptionCategory cat = new JobDescriptionCategory
                {
                    Letter = category.Element("Letter").Value,
                    Weight = Convert.ToInt32(category.Element("Weight").Value),
                    Title = category.Element("Title").Value
                };
                // The XElement built from a "Category" has a child named "PositionDescriptionFields" that contains "PositionDescriptionItem" children                
                IEnumerable<XElement> positionDescriptionFields = category.Element("PositionDescriptionFields").Elements("PositionDescriptionItem");

                // Loop through the collection of PositionDescriptionFields and create a PositionDescriptionItem class object from each
                foreach (XElement positionDescriptionItem in positionDescriptionFields)
                {
                    // create the PositionDescriptionItem class object
                    PositionDescriptionItem item = new PositionDescriptionItem { Detail = positionDescriptionItem.Value };
                    // add the PositionDescriptionItem class object to the Category's collection
                    cat.PositionDescriptionItems.Add(item);
                }
                // The XElement build from a "Category" has a child named "PerformanceStandardFields" that contains "PerformanceStandardItem" objects
                IEnumerable<XElement> performanceStandardFields = category.Element("PerformanceStandardFields").Elements("PerformanceStandardItem");

                // Loop through the collection of PerformanceStandardFields and create a PerformanceStandardItem class object from each
                foreach (XElement performanceStandardItem in performanceStandardFields)
                {
                    // create a new PerformanceStandardItem class object and populate it's properties from the XML data
                    PerformanceStandardItem item = new PerformanceStandardItem { Initial = performanceStandardItem.Attribute("initial").Value, Detail = performanceStandardItem.Value };
                    // add the PerformanceStandardItem class object to the Category's collection.
                    cat.PerformanceStandardItems.Add(item);
                }
                // the JobDescriptionCategory is built, so add it to the "results" collection
                results.Add(cat);
            }
            // assign the newly built collection of JobDescriptionCategory items named "results" to the class Categories properties
            Categories = results;
        }

        /// <summary>
        /// Creates a default rank list.
        /// </summary>
        /// <returns>A list of string Rank Names.</returns>
        private List<string> DefaultRankList()
        {
            List<string> results = new List<string>();
            results.Add("Police Officer");
            results.Add("POFC");
            results.Add("Police Corporal");
            results.Add("Police Sergeant");
            results.Add("Police Lieutenant");
            results.Add("Police Captain");
            results.Add("Police Major");
            results.Add("General Clerk III");
            results.Add("General Clerk IV");
            return results;

        }
    }
}
