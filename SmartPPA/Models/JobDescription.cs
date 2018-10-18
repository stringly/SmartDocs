using System;
using System.Xml;
using System.Xml.Linq;
using SmartPPA.Models.Types;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using SmartPPA.Models.ViewModels;

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
        public JobDescription(SmartJob job)
        {
            List<JobDescriptionCategory> results = new List<JobDescriptionCategory>();
            XElement root = job.JobDataXml;
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


        public JobDescription(string filePath)
        {
            List<JobDescriptionCategory> results = new List<JobDescriptionCategory>();
            XElement root = XElement.Load(filePath);
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

        public JobDescription(JobDescriptionViewModel formData)
        {
            ClassTitle = formData.Rank;
            WorkingTitle = formData.WorkingTitle;
            Grade = formData.Grade;
            WorkingHours = formData.WorkingHours;
            Rank = formData.Rank;
            Categories = formData.Categories;
        }

        public double GetOverallScore()
        {
            double results = 0.00;
            foreach (JobDescriptionCategory c in Categories)
            {
                results += c.GetCategoryRatedScore();
            }
            return results;
        }

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

        public void WriteJobDescriptionToXml(string filePath)
        {
            XElement root = new XElement("JobDescription");
            // TODO: Parse Files in directory to get list of Ids...
            // root.Add(new XElement("JobId", 2, new XAttribute("id", "JobId")));
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
        public XElement JobDescriptionToXml()
        {
            XElement root = new XElement("JobDescription");
            // TODO: Parse Files in directory to get list of Ids...
            // root.Add(new XElement("JobId", jobId, new XAttribute("id", "JobId")));
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
