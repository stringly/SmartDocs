using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    /// <summary>
    /// Class that corresponds to fields on the pre-2021 Smart PPA form template that are populated by form data.
    /// </summary>
    public class SmartPAFMappedFieldSet
    {
        /// <summary>
        /// Constructs a new instance of the class. 
        /// </summary>
        /// <param name="mainPart">The template document's <see cref="MainDocumentPart"/></param>
        public SmartPAFMappedFieldSet(MainDocumentPart mainPart)
        {
            Table PAFFields = mainPart.Document.Body.Elements<Table>().ElementAt(0);
            PAF_ProbationaryMidpoint = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(1));
            PAF_PeriodicPerformanceAssessment = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(2).Elements<TableCell>().ElementAt(1));
            PAF_RatingJustification = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(1));
            PAF_EmployeeName = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(5).Elements<TableCell>().ElementAt(1));
            PAF_PayrollId = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(6).Elements<TableCell>().ElementAt(1));
            PAF_StartDate = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(7).Elements<TableCell>().ElementAt(1));
            PAF_EndDate = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(7).Elements<TableCell>().ElementAt(3));
            PAF_ClassGrade = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(9).Elements<TableCell>().ElementAt(1));
            PAF_DistrictDivision = new SmartParagraph(PAFFields.Elements<TableRow>().ElementAt(8).Elements<TableCell>().ElementAt(1));

            PAF_Assessment = new SmartParagraph(mainPart.Document.Body.Elements<Table>().ElementAt(1).Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(0));
            PAF_Assessment_Chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, "assessmentChunk");
            PAF_Recommendations = new SmartParagraph(mainPart.Document.Body.Elements<Table>().ElementAt(2).Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(0));
            PAF_Recommendations_Chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, "recommendationsChunk");
        }
        /// <summary>
        /// Probationary Midpoint checkbox
        /// </summary>
        public SmartParagraph PAF_ProbationaryMidpoint { get; set; }
        /// <summary>
        /// Periodic Performance Assessment checkbox
        /// </summary>
        public SmartParagraph PAF_PeriodicPerformanceAssessment { get; set; }
        /// <summary>
        /// Rating Justification
        /// </summary>
        public SmartParagraph PAF_RatingJustification { get; set; }
        /// <summary>
        /// The employee name on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_EmployeeName { get; set; }
        /// <summary>
        /// The employee Payroll Id on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_PayrollId { get; set; }
        /// <summary>
        /// The appraisal period start date on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_StartDate { get; set; }
        /// <summary>
        /// The appraisal period end date on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_EndDate { get; set; }
        /// <summary>
        /// The employee's class title/grade on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_ClassGrade { get; set; }
        /// <summary>
        /// The employee's district/division on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_DistrictDivision { get; set; }
        /// <summary>
        /// The supervisor's assessment on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_Assessment { get; set; }
        /// <summary>
        /// The chunk section containing the HTML rich text comments from the Performance Assessment Form.
        /// </summary>
        public AlternativeFormatImportPart PAF_Assessment_Chunk { get; set; }
        /// <summary>
        /// The supervisor's recommendations on the Performance Appraisal Form.
        /// </summary>
        public SmartParagraph PAF_Recommendations { get; set; }
        /// <summary>
        /// The chunk section containing the HTML rich text comments from the Performance Assessment Form.
        /// </summary>
        public AlternativeFormatImportPart PAF_Recommendations_Chunk { get; set; }

        /// <summary>
        /// Method that writes the XML data to the template
        /// </summary>
        /// <param name="root">A <see cref="XElement"/> containing the XML form field data.</param>
        public void WriteXMLToFields(XElement root)
        {
            // Write PAF form fields
            XElement job = root.Element("JobDescription");
            string assessmentType = root.Element("SelectedPAFType").Value;
            RunFonts checkBoxRun = new RunFonts { Ascii = "Wingdings" };
            RunProperties checkBoxRunProperties = new RunProperties();
            checkBoxRunProperties.Append(checkBoxRun);
            switch (assessmentType)
            {
                case "Periodic Performance Assessment":
                    PAF_PeriodicPerformanceAssessment.Write("\u2713", true, checkBoxRunProperties);
                    break;
                case "Probationary Midpoint":
                    PAF_ProbationaryMidpoint.Write("\u2713", true, checkBoxRunProperties);
                    break;
                case "Rating Justification":
                    PAF_RatingJustification.Write("\u2713", true, checkBoxRunProperties);
                    break;
            }

            PAF_EmployeeName.Write($"{root.Element("LastName").Value}, {root.Element("FirstName").Value}");
            PAF_PayrollId.Write(root.Element("PayrollIdNumber").Value);
            PAF_StartDate.Write(DateTime.Parse(root.Element("StartDate").Value).ToString("MM/dd/yy"));
            PAF_EndDate.Write(DateTime.Parse(root.Element("EndDate").Value).ToString("MM/dd/yy"));
            PAF_ClassGrade.Write($"{job.Element("ClassTitle").Value} / {job.Element("Grade").Value}");
            PAF_DistrictDivision.Write(root.Element("DepartmentDivision").Value);
            // HTML from the RTF Textarea needs to be chunked.
            using (Stream chunkStream = PAF_Assessment_Chunk.GetStream(FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter stringStream = new StreamWriter(chunkStream))
                {
                    stringStream.Write($"<html>{root.Element("Assessment").Value}</html>");
                }
            }
            AltChunk assessmentChunk = new AltChunk
            {
                Id = "assessmentChunk"
            };
            PAF_Assessment.Paragraph.InsertBeforeSelf(assessmentChunk);
            using (Stream chunkStream = PAF_Recommendations_Chunk.GetStream(FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter stringStream = new StreamWriter(chunkStream))
                {
                    stringStream.Write($"<html>{root.Element("Recommendation").Value}</html>");
                }
            }
            AltChunk recommendationsChunk = new AltChunk
            {
                Id = "recommendationsChunk"
            };
            PAF_Recommendations.Paragraph.InsertBeforeSelf(recommendationsChunk);
        }
    }
}
