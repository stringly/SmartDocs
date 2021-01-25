using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    public class SmartJobDescriptionMappedFieldSet
    {
        public SmartParagraph JOB_EmployeeName { get; set; }
        public SmartParagraph JOB_DistrictDivision { get; set; }
        public SmartParagraph JOB_AgencyActivity { get; set; }
        public SmartParagraph JOB_PositionNumber { get; set; }
        public SmartParagraph JOB_ClassTitle { get; set; }
        public SmartParagraph JOB_Grade { get; set; }
        public SmartParagraph JOB_WorkingTitle { get; set; }
        public SmartParagraph JOB_WorkAddress { get; set; }
        public SmartParagraph JOB_WorkingHours { get; set; }
        public SmartParagraph JOB_Supervisor { get; set; }
        public SmartParagraph JOB_Supervises { get; set; }
        public Table JobDescriptionTable { get; set; }
        public SmartJobDescriptionMappedFieldSet(MainDocumentPart mainPart)
        {
            Table JOBFields = mainPart.Document.Body.Elements<Table>().ElementAt(0);
            JOB_EmployeeName = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(0));
            JOB_DistrictDivision = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(2));
            JOB_AgencyActivity = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(3));
            JOB_PositionNumber = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(4));
            JOB_ClassTitle = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(1));
            JOB_Grade = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(2));
            JOB_WorkingTitle = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(5).Elements<TableCell>().ElementAt(1));
            JOB_WorkAddress = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(7).Elements<TableCell>().ElementAt(0));
            JOB_WorkingHours = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(7).Elements<TableCell>().ElementAt(1));
            JOB_Supervisor = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(9).Elements<TableCell>().ElementAt(0));
            JOB_Supervises = new SmartParagraph(JOBFields.Elements<TableRow>().ElementAt(11).Elements<TableCell>().ElementAt(0));
            JobDescriptionTable = mainPart.Document.Body.Elements<Table>().ElementAt(1);
        }
        public void WriteXMLToFields(XElement root)
        {
            JobDescription job = ExtractJobDescriptionFromXMLFormData(root);
            foreach (JobDescriptionCategory category in job.Categories)
            {                
                JobDescriptionTable.Append(category.GetCategoryHeaderRow());
                JobDescriptionTable.Append(category.GenerateDetailsRow());             
            }
            // Write Job Description Fields
            JOB_EmployeeName.Write($"{root.Element("LastName").Value}, {root.Element("FirstName").Value}", true, new RunProperties(new FontSize(){ Val = "28"}));
            JOB_DistrictDivision.Write(root.Element("DepartmentDivision").Value, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_AgencyActivity.Write(root.Element("DepartmentDivisionCode").Value, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_PositionNumber.Write(root.Element("PositionNumber").Value, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_ClassTitle.Write(job.ClassTitle, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_Grade.Write(job.Grade, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_WorkingTitle.Write(job.WorkingTitle, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_WorkAddress.Write(root.Element("WorkPlaceAddress").Value, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_WorkingHours.Write(job.WorkingHours, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_Supervisor.Write(root.Element("AuthorName").Value, true, new RunProperties(new FontSize() { Val = "20" }));
            JOB_Supervises.Write(root.Element("SupervisedByEmployee").Value, true, new RunProperties(new FontSize() { Val = "20" }));
        }
        public JobDescription ExtractJobDescriptionFromXMLFormData(XElement rootElement)
        {
            XElement jobDescription = rootElement.Element("JobDescription");
            // Build Job
            JobDescription job = new JobDescription
            {
                SmartJobId = Convert.ToInt32(rootElement.Element("JobId").Value),
                ClassTitle = jobDescription.Element("ClassTitle").Value,
                WorkingTitle = jobDescription.Element("WorkingTitle").Value,
                Grade = jobDescription.Element("Grade").Value,
                WorkingHours = jobDescription.Element("WorkingHours").Value,
                Categories = new List<JobDescriptionCategory>()
            };
            IEnumerable<XElement> CategoryList = jobDescription.Element("Categories").Elements("Category");
            foreach (XElement category in CategoryList)
            {
                JobDescriptionCategory JobCategory = new JobDescriptionCategory
                {
                    Letter = category.Element("Letter").Value,
                    Weight = Convert.ToInt32(category.Element("Weight").Value),
                    Title = category.Element("Title").Value,
                    PositionDescriptionItems = new List<PositionDescriptionItem>(),
                    PerformanceStandardItems = new List<PerformanceStandardItem>()
                };
                IEnumerable<XElement> positionItems = category.Element("PositionDescriptionFields").Elements("PositionDescriptionItem");
                foreach (XElement item in positionItems)
                {
                    JobCategory.PositionDescriptionItems.Add(new PositionDescriptionItem
                    {
                        Detail = item.Value
                    });
                }
                IEnumerable<XElement> performanceItems = category.Element("PerformanceStandardFields").Elements("PerformanceStandardItem");
                foreach (XElement item in performanceItems)
                {
                    JobCategory.PerformanceStandardItems.Add(new PerformanceStandardItem
                    {
                        Initial = item.Attribute("initial").Value,
                        Detail = item.Value
                    });
                }
                job.Categories.Add(JobCategory);
            }
            return job;
        }
    }
}
