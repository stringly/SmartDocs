using SmartPPA.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartPPA.Models.ViewModels
{
    public class PPAFormViewModel
    {
        [Display(Name = "First Name:"), StringLength(50), Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name:"), StringLength(50), Required]
        public string LastName { get; set; }
        [Display(Name = "ID#"), StringLength(5), Required]
        public string DepartmentIdNumber { get; set; }
        [Display(Name = "Payroll ID#"), StringLength(10), Required]
        public string PayrollIdNumber { get; set; }
        [Display(Name = "Position Number"), StringLength(10), Required]
        public string PositionNumber { get; set; }
        [Display(Name = "Department/Division:"), StringLength(50), Required]
        public string DepartmentDivision {get;set;}
        [Display(Name = "Department/Division Code:"), StringLength(50), Required]
        public string DepartmentDivisionCode { get; set; }
        [Display(Name = "Work Location:"), StringLength(50), Required]
        public string WorkPlaceAddress { get; set; }     
        
        [Display(Name = "Supervised by Employee:"), StringLength(50)]
        public string SupervisedByEmployeeName { get; set; }
        [Display(Name = "Start Date:"), Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date:"), Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Job Title"), Required]
        public int JobId { get; set; }
        [Display(Name = "Immediate Supervisor"), Required]
        public int AuthorUserId { get; set; }
        [MaxLength(5000), Required, Display(Name ="Performance Assessment:")]
        public string Assessment { get; set; }
        [MaxLength(5000), Required, Display(Name = "Supervisor's Recommendations:")]
        public string Recommendation { get; set; }
        public List<JobDescriptionCategory> Categories { get; set; }
        public List<JobDescriptionListItem> JobList { get; set; }
        public List<UserListItem> Users { get; set; }

        public PPAFormViewModel()
        {
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;
            
        }

        public PPAFormViewModel(string contentPath)
        {
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;
            List<JobDescriptionListItem> results = new List<JobDescriptionListItem>();
            DirectoryInfo dir = new DirectoryInfo(contentPath);
            var files = dir.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.Extension == ".xml")
                {
                    XElement root = XElement.Load(f.FullName);
                    JobDescriptionListItem item = new JobDescriptionListItem
                    {
                        JobId = Convert.ToInt32(root.Element("JobId").Value),
                        Rank = root.Element("Rank").Value,
                        Grade = root.Element("Grade").Value,
                        DisplayName = $"{root.Element("Rank").Value} ({root.Element("Grade").Value}) - {root.Element("WorkingTitle").Value}"                                             
                    };
                    results.Add(item);
                }
            }
            JobList = results;
        }

        public void RepopulateJobList(string contentPath)
        {
            List<JobDescriptionListItem> results = new List<JobDescriptionListItem>();
            DirectoryInfo dir = new DirectoryInfo(contentPath);
            var files = dir.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.Extension == ".xml")
                {
                    XElement root = XElement.Load(f.FullName);
                    JobDescriptionListItem item = new JobDescriptionListItem
                    {
                        JobId = Convert.ToInt32(root.Element("JobId").Value),
                        Rank = root.Element("Rank").Value,
                        Grade = root.Element("Grade").Value,
                        DisplayName = $"{root.Element("Rank").Value} ({root.Element("Grade").Value}) - {root.Element("WorkingTitle").Value}",                        
                    };
                    results.Add(item);
                }
            }
            JobList = results;
        }

        public XElement FormDataToXml()
        {
            XElement root = new XElement("PPAFormData");
            PropertyInfo[] properties = typeof(PPAFormViewModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                root.Add(new XElement(property.Name, property.GetValue(this, null)));
            }
            return root;
        }
    }
}
