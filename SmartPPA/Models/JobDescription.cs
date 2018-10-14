using System;
using System.Xml;
using System.Xml.Linq;
using SmartPPA.Models.Types;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

namespace SmartPPA.Models
{
    public class JobDescription
    {
        public string ClassTitle { get; set; }
        public string WorkingTitle { get; set; }
        public string Grade { get; set; }
        public string WorkingHours { get; set; }
        public string Rank { get; set; }
        public List<JobDescriptionCategory> Categories { get; set; }

        public JobDescription()
        {
            Categories = new List<JobDescriptionCategory>();
        }

        public JobDescription(string jobName)
        {
            List<JobDescriptionCategory> results = new List<JobDescriptionCategory>();
            XElement root = XElement.Load(jobName + ".xml");
            ClassTitle = root.Element("ClassTitle").Value;
            WorkingTitle = root.Element("WorkingTitle").Value;
            Grade = root.Element("Grade").Value;
            WorkingHours = root.Element("WorkingHours").Value;
            Rank = root.Element("Rank").Value;
            IEnumerable<XElement> CategoryList = root.Element("Categories").Elements("Category");
            foreach (XElement category in CategoryList)
            {
                JobDescriptionCategory cat = new JobDescriptionCategory
                {
                    Letter = category.Element("Letter").Value,
                    Weight = Convert.ToInt32(category.Element("Weight").Value),
                    Title = category.Element("Title").Value
                };
                IEnumerable<XElement> positionDescriptionFields = category.Element("PositionDescriptionFields").Elements("PositionDescriptionItem");
                foreach (XElement positionDescriptionItem in positionDescriptionFields)
                {
                    PositionDescriptionItem item = new PositionDescriptionItem { Detail = positionDescriptionItem.Value };
                    cat.PositionDescriptionItems.Add(item);
                }
                IEnumerable<XElement> performanceStandardFields = category.Element("PerformanceStandardFields").Elements("PerformanceStandardItem");
                foreach (XElement performanceStandardItem in performanceStandardFields)
                {
                    PerformanceStandardItem item = new PerformanceStandardItem { Initial = performanceStandardItem.Attribute("initial").Value, Detail = performanceStandardItem.Value };
                    cat.PerformanceStandardItems.Add(item);
                }

                results.Add(cat);
            }
            Categories = results;
        }

        public void WriteJobValueItems(MainDocumentPart mainPart)
        {
            
        }
    }

}
