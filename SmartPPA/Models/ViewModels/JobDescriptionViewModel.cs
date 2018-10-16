using SmartPPA.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartPPA.Models.ViewModels
{
    public class JobDescriptionViewModel
    {      
        
        [Display(Name = "Working Title"), StringLength(50), Required]
        public string WorkingTitle { get; set; }
        [Display(Name = "Class Title"), StringLength(50), Required]
        public string Grade { get; set; }
        [Display(Name = "Working Hours"), StringLength(50), Required]
        public string WorkingHours { get; set; }
        [Display(Name = "Rank"), StringLength(50), Required]
        public string Rank { get; set; }
        public List<JobDescriptionCategory> Categories { get; set; }
        public List<string> Ranks { get; set; }

        public JobDescriptionViewModel()
        {
            Categories = new List<JobDescriptionCategory>();
            WorkingHours = "Varied";
            Ranks = DefaultRankList();
        }

        public JobDescriptionViewModel(string filePath)
        {
            Ranks = DefaultRankList();
            List<JobDescriptionCategory> results = new List<JobDescriptionCategory>();
            XElement root = XElement.Load(filePath);
            //ClassTitle = root.Element("ClassTitle").Value;
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

        private List<string> DefaultRankList()
        {
            List<string> results = new List<string>();
            results.Add("Police Officer");
            results.Add("Police Officer First Class");
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
