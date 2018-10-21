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
        public int PPAId { get; set; }
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
        public string SupervisedByEmployee { get; set; }
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
        public JobDescription job { get; set; }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PPAFormViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// This method is designed to facilitate editing an existing PPA in the Db.
        /// </remarks>
        /// <param name="ppa">A <see cref=""/> </param>
        public PPAFormViewModel(SmartPPA ppa)
        {
            PPAId = ppa.PPAId;
            FirstName = ppa.EmployeeFirstName;
            LastName = ppa.EmployeeLastName;
            DepartmentIdNumber = ppa.DepartmentIdNumber;
            PayrollIdNumber = ppa.PayrollIdNumber;
            PositionNumber = ppa.PositionNumber;
            DepartmentDivision = ppa.DepartmentDivision;
            DepartmentDivisionCode = ppa.DepartmentDivisionCode;
            WorkPlaceAddress = ppa.WorkplaceAddress;
            SupervisedByEmployee = ppa.SupervisedByEmployee;
            StartDate = ppa.StartDate;
            EndDate = ppa.EndDate;
            JobId = ppa.Job.JobId;
            AuthorUserId = ppa.Owner.UserId;
            Assessment = ppa.AssessmentComments;
            Recommendation = ppa.RecommendationComments;
            int?[] scores = {ppa.CategoryScore_1, ppa.CategoryScore_2, ppa.CategoryScore_3, ppa.CategoryScore_4, ppa.CategoryScore_5, ppa.CategoryScore_6 };
            job = new JobDescription(ppa.Job);
            for (int i = 0; i < job.Categories.Count(); i++)
            {
                job.Categories[i].SelectedScore = scores[i] ?? 0;
            }
            Categories = job.Categories;
            
        }

        // contentPath deprecated with SQL persistence
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


    }
}
