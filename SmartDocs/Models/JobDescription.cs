using System;
using System.Xml;
using System.Xml.Linq;
using SmartDocs.Models.Types;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that manages Job Description interactions
    /// </summary>
    /// <remarks>
    /// This class expands the data contained in a <see cref="T:SmartDocs.Models.SmartJob"/>
    /// </remarks>
    public class JobDescription
    {
        /// <summary>
        /// Gets or sets the identifier for the associated <see cref="T:SmartDocs.Models.SmartJob"/>.
        /// </summary>
        /// <value>
        /// The smart job identifier.
        /// </value>
        public int SmartJobId { get; set; }

        /// <summary>
        /// Gets or sets the Class Title.
        /// </summary>
        /// <value>
        /// The class title.
        /// </value>
        public string ClassTitle { get; set; }

        /// <summary>
        /// Gets or sets the Working Title.
        /// </summary>
        /// <value>
        /// The working title.
        /// </value>
        public string WorkingTitle { get; set; }

        /// <summary>
        /// Gets or sets the Job Grade.
        /// </summary>
        /// <remarks>
        /// The Job Grade is the PG Personnel code for class of the position, i.e "L01"
        /// </remarks>
        /// <value>
        /// The grade.
        /// </value>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the working hours for the position.
        /// </summary>
        /// <value>
        /// The working hours.
        /// </value>
        public string WorkingHours { get; set; }

        /// <summary>
        /// Gets or sets the Rank associated with the position.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets the Job's Categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        /// <seealso cref="T:SmartDocs.Models.Types.JobDescriptionCategory"/>
        public List<JobDescriptionCategory> Categories { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.JobDescription"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless class constructor
        /// </remarks>
        public JobDescription()
        {
            // set the categories to a new list to prevent NullRef errors
            Categories = new List<JobDescriptionCategory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.JobDescription"/> class.
        /// </summary>
        /// <remarks>
        /// Class constructor that requires a <see cref="T:SmartDocs.Models.SmartJob"/>
        /// </remarks>
        /// <param name="job">A <see cref="T:SmartDocs.Models.SmartJob"/>.</param>
        public JobDescription(SmartJob job)
        {
            SmartJobId = job.JobId;

            // create a new category list
            List<JobDescriptionCategory> results = new List<JobDescriptionCategory>();

            // retrieve the XML data column from the SmartJob Entity parameter
            XElement root = job.JobDataXml;
            
            // assign the properties that are stored in the XML data
            ClassTitle = root.Element("ClassTitle").Value;
            WorkingTitle = root.Element("WorkingTitle").Value;
            Grade = root.Element("Grade").Value;
            WorkingHours = root.Element("WorkingHours").Value;
            Rank = root.Element("Rank").Value;

            // the XML has a child element named "Categories" that has children named "Category"
            IEnumerable<XElement> CategoryList = root.Element("Categories").Elements("Category");

            // loop through the categories and map them to JobDescriptionCategory class objects
            foreach (XElement category in CategoryList)
            {
                JobDescriptionCategory cat = new JobDescriptionCategory
                {
                    Letter = category.Element("Letter").Value,
                    Weight = Convert.ToInt32(category.Element("Weight").Value),
                    Title = category.Element("Title").Value
                };

                // each category contains a child element named "PositionDescriptionFields" that contains children named "PositionDescriptionItem"
                IEnumerable<XElement> positionDescriptionFields = category.Element("PositionDescriptionFields").Elements("PositionDescriptionItem");

                // loop through the PositionDescriptionItems and map to PositionDescriptionItem class objects
                foreach (XElement positionDescriptionItem in positionDescriptionFields)
                {
                    PositionDescriptionItem item = new PositionDescriptionItem { Detail = positionDescriptionItem.Value };
                    // add each object to the Category Object's collection
                    cat.PositionDescriptionItems.Add(item);
                }

                // each category contains a child element named "PerformanceStandardFields" that contains children named "PerformanceStandardItem"
                IEnumerable<XElement> performanceStandardFields = category.Element("PerformanceStandardFields").Elements("PerformanceStandardItem");

                // loop through the PerformanceStandardItems and map to PerformanceStandardItem class objects
                foreach (XElement performanceStandardItem in performanceStandardFields)
                {
                    PerformanceStandardItem item = new PerformanceStandardItem { Initial = performanceStandardItem.Attribute("initial").Value, Detail = performanceStandardItem.Value };
                    // add each object to the Category Object's collection
                    cat.PerformanceStandardItems.Add(item);
                }
                // now, add the Category object itself to the "result" list of categories
                results.Add(cat);
            }
            // assign the assembled category list to the Job Description's Categories property
            Categories = results;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.JobDescription"/> class.
        /// </summary>
        /// <remarks>
        /// This method is used to create a Job Description Object from the form data in a <see cref="SmartDocs.Models.ViewModels.JobDescriptionViewModel"/>
        /// </remarks>
        /// <param name="formData">The <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionViewModel"/>.</param>
        public JobDescription(JobDescriptionViewModel formData)
        {
            ClassTitle = formData.Rank;
            WorkingTitle = formData.WorkingTitle;
            Grade = formData.Grade;
            WorkingHours = formData.WorkingHours;
            Rank = formData.Rank;
            Categories = formData.Categories;
        }

        /// <summary>
        /// Gets the overall score.
        /// </summary>
        /// <remarks>
        /// This method calculates the overall numerical score based on the Categories property.
        /// </remarks>
        /// <returns>A double representing the Calculated overall rating</returns>
        public double GetOverallScore()
        {
            double results = 0.00;
            foreach (JobDescriptionCategory c in Categories)
            {
                results += c.GetCategoryRatedScore();
            }
            return results;
        }

        /// <summary>
        /// Gets the overall rating.
        /// </summary>
        /// <remarks>
        /// This method calculates the overall rating based on the scores in the Job Description's Categories property
        /// </remarks>
        /// <returns>A string representing the associated rating keyword.</returns>
        public string GetOverallRating()
        {
            double sumOfScores = GetOverallScore();            
            if (sumOfScores <= 0.5) {
                return "Unsatisfactory";
            }
            else if (sumOfScores >= 0.51 && sumOfScores <= 1.5) {
                return "Improvement Needed";
            }
            else if (sumOfScores >= 1.51 && sumOfScores <= 2.5) {
                return "Satisfactory";
            }
            else if (sumOfScores >= 2.51 && sumOfScores <= 3.5){
                return "Exceeds Satisfactory";
            }
            else {
                return "Outstanding";
            }
        }

        /// <summary>
        /// Writes the job description to an external XML file.
        /// </summary>
        /// <remarks>
        /// This method writes an external XML file. For writing XML to the XML data column of a <see cref="T:SmartDocs.Models.SmartJob"/>, 
        /// see <see cref="M:SmartDocs.Models.JobDescription.JobDescriptionToXml"/>
        /// </remarks>
        /// <param name="filePath">The file path to write the resulting file.</param>
        public void WriteJobDescriptionToXml(string filePath)
        {
            XElement root = new XElement("JobDescription");
            root.Add(new XElement("ClassTitle", ClassTitle, new XAttribute("id", "ClassTitle")));
            root.Add(new XElement("WorkingTitle", WorkingTitle, new XAttribute("id", "WorkingTitle")));
            root.Add(new XElement("Grade", Grade, new XAttribute("id", "Grade")));
            root.Add(new XElement("Rank", Rank, new XAttribute("id", "Rank")));
            root.Add(new XElement("WorkingHours", WorkingHours, new XAttribute("id", "WorkingHours")));
            XElement categories = new XElement("Categories", new XAttribute("id", "Categories"));
            foreach (JobDescriptionCategory c in Categories)
            {
                if (!String.IsNullOrEmpty(c.Title)) {
                    XElement category = new XElement("Category", new XAttribute("id", "Category"));
                    category.Add(new XElement("Letter", c.Letter, new XAttribute("id", "Letter")));
                    category.Add(new XElement("Weight", c.Weight, new XAttribute("id", "Weight")));
                    category.Add(new XElement("Title", c.Title, new XAttribute("id", "Title")));
                    XElement positionDescriptionFields = new XElement("PositionDescriptionFields", new XAttribute("id", "PositionDescriptionFields"));
                    foreach(PositionDescriptionItem p in c.PositionDescriptionItems)
                    {
                        if (!String.IsNullOrEmpty(p.Detail))
                        {
                            positionDescriptionFields.Add(new XElement("PositionDescriptionItem", p.Detail));
                        }                    
                    }
                    category.Add(positionDescriptionFields);
                    XElement performanceStandardFields = new XElement("PerformanceStandardFields", new XAttribute("id", "PerformanceStandardFields"));
                    int itemCount = 0;
                    foreach (PerformanceStandardItem p in c.PerformanceStandardItems)
                    {                        
                        if (!String.IsNullOrEmpty(p.Detail))
                        {
                            itemCount++;
                            performanceStandardFields.Add(new XElement("PerformanceStandardItem", p.Detail, new XAttribute("initial", $"{c.Letter}{itemCount}")));
                        }                    
                    }
                    category.Add(performanceStandardFields);
                    categories.Add(category);
                }
            }
            root.Add(categories);
            root.Save(filePath + $"{Rank}-{WorkingTitle}.xml");
        }

        /// <summary>
        /// Writes the Job Description data to an XML XElement.
        /// </summary>
        /// <returns>An <see cref="T:System.Xml.Linq.XElement"/></returns>
        public XElement JobDescriptionToXml()
        {
            XElement root = new XElement("JobDescription");
            root.Add(new XElement("ClassTitle", ClassTitle, new XAttribute("id", "ClassTitle")));
            root.Add(new XElement("WorkingTitle", WorkingTitle, new XAttribute("id", "WorkingTitle")));
            root.Add(new XElement("Grade", Grade, new XAttribute("id", "Grade")));
            root.Add(new XElement("Rank", Rank, new XAttribute("id", "Rank")));
            root.Add(new XElement("WorkingHours", WorkingHours, new XAttribute("id", "WorkingHours")));
            XElement categories = new XElement("Categories", new XAttribute("id", "Categories"));
            foreach (JobDescriptionCategory c in Categories)
            {
                if (!String.IsNullOrEmpty(c.Title)) {
                    XElement category = new XElement("Category", new XAttribute("id", "Category"));
                    category.Add(new XElement("Letter", c.Letter, new XAttribute("id", "Letter")));
                    category.Add(new XElement("Weight", c.Weight, new XAttribute("id", "Weight")));
                    category.Add(new XElement("Title", c.Title, new XAttribute("id", "Title")));
                    XElement positionDescriptionFields = new XElement("PositionDescriptionFields", new XAttribute("id", "PositionDescriptionFields"));
                    foreach(PositionDescriptionItem p in c.PositionDescriptionItems)
                    {
                        if (!String.IsNullOrEmpty(p.Detail))
                        {
                            positionDescriptionFields.Add(new XElement("PositionDescriptionItem", p.Detail));
                        }                    
                    }
                    category.Add(positionDescriptionFields);
                    XElement performanceStandardFields = new XElement("PerformanceStandardFields", new XAttribute("id", "PerformanceStandardFields"));
                    int itemCount = 0;
                    foreach (PerformanceStandardItem p in c.PerformanceStandardItems)
                    {                        
                        if (!String.IsNullOrEmpty(p.Detail))
                        {
                            itemCount++;
                            performanceStandardFields.Add(new XElement("PerformanceStandardItem", p.Detail, new XAttribute("initial", $"{c.Letter}{itemCount}")));
                        }                    
                    }
                    category.Add(performanceStandardFields);
                    categories.Add(category);
                }
            }
            root.Add(categories);
            return root;
        }
    }

}
