using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    public class SmartPPAMappedFieldSet
    {
        public SmartParagraph PPA_EmployeeName { get; set; }
        public SmartParagraph PPA_PayrollId { get; set; }
        public SmartParagraph PPA_ClassTitle { get; set; }
        public SmartParagraph PPA_Grade { get; set; }
        public SmartParagraph PPA_PositionNumber { get; set; }
        public SmartParagraph PPA_StartDate { get; set; }
        public SmartParagraph PPA_EndDate { get; set; }
        public SmartParagraph PPA_DistrictDivision { get; set; }
        public SmartParagraph PPA_AgencyActivity { get; set; }
        public List<SmartCategory> PPA_Categories { get; set; }
        public SmartParagraph PPA_TotalRatingValue { get; set; }
        public SmartParagraph PPA_OverallAppraisal { get; set; }
        public SmartParagraph PAF_EmployeeName { get; set; }
        public SmartParagraph PAF_PayrollId { get; set; }
        public SmartParagraph PAF_StartDate { get; set; }
        public SmartParagraph PAF_EndDate { get; set; }
        public SmartParagraph PAF_ClassGrade { get; set; }
        public SmartParagraph PAF_DistrictDivision { get; set; }
        public SmartParagraph PAF_Assessment { get; set; }
        public AlternativeFormatImportPart PAF_Assessment_Chunk { get; set; }
        public SmartParagraph PAF_Recommendations { get; set; }
        public AlternativeFormatImportPart PAF_Recommendations_Chunk { get; set; }
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
        public SmartPPAMappedFieldSet(MainDocumentPart mainPart)
        {
            // PPA Fields start in table 1
            Table PPAFields = mainPart.Document.Body.Elements<Table>().ElementAt(0);
            PPA_EmployeeName = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(1));
            PPA_PayrollId = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(2));
            PPA_ClassTitle = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(1));
            PPA_Grade = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(2));
            PPA_PositionNumber = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(3));
            PPA_StartDate = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(0));
            PPA_EndDate = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(2));
            PPA_DistrictDivision = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(6).Elements<TableCell>().ElementAt(3));
            PPA_AgencyActivity = new SmartParagraph(PPAFields.Elements<TableRow>().ElementAt(6).Elements<TableCell>().ElementAt(4));

            PPA_Categories = new List<SmartCategory>();
            Table PPACategories = mainPart.Document.Body.Elements<Table>().ElementAt(1);
            for (int i = 2; i < 8; i++)
            {
                PPA_Categories.Add(new SmartCategory(PPACategories.Elements<TableRow>().ElementAt(i)));
            }
            PPA_TotalRatingValue = new SmartParagraph(PPACategories.Elements<TableRow>().ElementAt(8).Elements<TableCell>().ElementAt(2));
            PPA_OverallAppraisal = new SmartParagraph(PPACategories.Elements<TableRow>().ElementAt(9).Elements<TableCell>().ElementAt(2));
            Table PAFHeader = mainPart.HeaderParts.ElementAt(2).RootElement.Elements<Table>().ElementAt(0);
            PAF_EmployeeName = new SmartParagraph(PAFHeader.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(1));
            PAF_PayrollId = new SmartParagraph(PAFHeader.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(3));
            PAF_StartDate = new SmartParagraph(PAFHeader.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(1));
            PAF_EndDate = new SmartParagraph(PAFHeader.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(3));
            PAF_ClassGrade = new SmartParagraph(PAFHeader.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(5));
            PAF_DistrictDivision = new SmartParagraph(PAFHeader.Elements<TableRow>().ElementAt(5).Elements<TableCell>().ElementAt(1));
            PAF_Assessment = new SmartParagraph(mainPart.Document.Body.Elements<Table>().ElementAt(3).Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(0));
            PAF_Assessment_Chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, "assessmentChunk");
            PAF_Recommendations = new SmartParagraph(mainPart.Document.Body.Elements<Table>().ElementAt(4).Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(0));
            PAF_Recommendations_Chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, "recommendationsChunk");
            Table JOBFields = mainPart.Document.Body.Elements<Table>().ElementAt(5);
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
            JobDescriptionTable = mainPart.Document.Body.Elements<Table>().ElementAt(6);
        }
        public void WriteXMLToFields(XElement root)
        {
            JobDescription job = ExtractJobDescriptionFromXMLFormData(root);
            // Write PPA Fields
            PPA_EmployeeName.Write($"{root.Element("LastName").Value}, {root.Element("FirstName").Value}");
            PPA_PayrollId.Write(root.Element("PayrollIdNumber").Value);
            // attempt to append newline subtext for Working Title
            
            PPA_ClassTitle.Write(job.ClassTitle);
            Run titleRun = new Run();
            RunProperties titleRunProperties = new RunProperties();
            FontSize titleRunFontSize = new FontSize() { Val = "16" };
            titleRunProperties.Append(titleRunFontSize);
            titleRun.Append(titleRunProperties);
            Text titleRunText = new Text(job.WorkingTitle);
            titleRun.Append(titleRunText);

            PPA_ClassTitle.Paragraph.InsertAfterSelf(new Paragraph(titleRun));
            PPA_Grade.Write(job.Grade);
            PPA_PositionNumber.Write(root.Element("PositionNumber").Value);
            PPA_StartDate.Write(DateTime.Parse(root.Element("StartDate").Value).ToString("MM/dd/yy"));
            PPA_EndDate.Write(DateTime.Parse(root.Element("EndDate").Value).ToString("MM/dd/yy"));
            PPA_DistrictDivision.Write(root.Element("DepartmentDivision").Value);
            PPA_AgencyActivity.Write(root.Element("DepartmentDivisionCode").Value);


            // Write Job Categories and Details
            int i = 0;
            foreach (JobDescriptionCategory category in job.Categories)
            {
                PPA_Categories[i].Write(category.Title, category.Weight, category.SelectedScore, category.GetCategoryRatedScore());
                JobDescriptionTable.Append(category.GetCategoryHeaderRow());
                JobDescriptionTable.Append(category.GenerateDetailsRow());
                i++;
            }
            PPA_TotalRatingValue.Write(job.GetOverallScore().ToString());
            PPA_OverallAppraisal.Write(job.GetOverallRating());

            // Write PAF form fields
            PAF_EmployeeName.Write($"{root.Element("LastName").Value}, {root.Element("FirstName").Value}");
            PAF_PayrollId.Write(root.Element("PayrollIdNumber").Value);
            PAF_StartDate.Write(DateTime.Parse(root.Element("StartDate").Value).ToString("MM/dd/yy"));
            PAF_EndDate.Write(DateTime.Parse(root.Element("EndDate").Value).ToString("MM/dd/yy"));
            PAF_ClassGrade.Write($"{job.ClassTitle} - {job.Grade}");
            PAF_DistrictDivision.Write(root.Element("DepartmentDivision").Value);
            // HTML from the RTF Textarea needs to be chunked.
            using (Stream chunkStream = PAF_Assessment_Chunk.GetStream(FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter stringStream = new StreamWriter(chunkStream))
                {
                    stringStream.Write($"<html>{root.Element("Assessment").Value}</html>");
                }
            }
            AltChunk assessmentChunk = new AltChunk();
            assessmentChunk.Id = "assessmentChunk";
            PAF_Assessment.Paragraph.InsertBeforeSelf(assessmentChunk);
            using (Stream chunkStream = PAF_Recommendations_Chunk.GetStream(FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter stringStream = new StreamWriter(chunkStream))
                {
                    stringStream.Write($"<html>{root.Element("Recommendation").Value}</html>");
                }
            }
            AltChunk recommendationsChunk = new AltChunk();
            recommendationsChunk.Id = "recommendationsChunk";
            PAF_Recommendations.Paragraph.InsertBeforeSelf(recommendationsChunk);

            // Write Job Description Fields
            JOB_EmployeeName.Write($"{root.Element("LastName").Value}, {root.Element("FirstName").Value}");
            JOB_DistrictDivision.Write(root.Element("DepartmentDivision").Value);
            JOB_AgencyActivity.Write(root.Element("DepartmentDivisionCode").Value);
            JOB_PositionNumber.Write(root.Element("PositionNumber").Value);
            JOB_ClassTitle.Write(job.ClassTitle);
            JOB_Grade.Write(job.Grade);
            JOB_WorkingTitle.Write(job.WorkingTitle);
            JOB_WorkAddress.Write(root.Element("WorkPlaceAddress").Value);
            JOB_WorkingHours.Write(job.WorkingHours);
            JOB_Supervisor.Write(root.Element("AuthorName").Value);
            JOB_Supervises.Write(root.Element("SupervisedByEmployee").Value);
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
                    SelectedScore = Convert.ToInt32(category.Element("SelectedScore").Value),
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
