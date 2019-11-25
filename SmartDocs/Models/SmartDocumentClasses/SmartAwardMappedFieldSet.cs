using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    public class SmartAwardMappedFieldSet
    {
        public SmartParagraph AgencyName { get; set;}
        public SmartParagraph EmployeeName { get;set;}
        /// <summary>
        /// The Employee's Work Class Title
        /// </summary>
        public SmartParagraph EmployeeClassTitle { get; set; }

        public SmartParagraph EmployeeDivision { get; set; }

        // Award Type Cells

        public SmartParagraph DedicatedAttendanceAward { get;set;}
        public SmartParagraph HonoraryAward { get; set; }
        public SmartParagraph CreativityAward { get; set; }
        public SmartParagraph ExemplaryPerformanceAward { get; set; }
        public SmartParagraph ExemplaryPerformanceDateRange { get; set; }
        public SmartParagraph SpecialAchievementAward { get; set; }
        public SmartParagraph EmployeeOfTheMonth { get;set;}
        public SmartParagraph EmployeeOfTheMonthDate { get; set; }
        public SmartParagraph EmployeeOfTheYear { get; set; }
        public SmartParagraph EmployeeOfTheYearDate { get; set; }
        public SmartParagraph CountyExecutiveAward { get; set; }

        // Recoginition Types
        public SmartParagraph LetterOfCommendation { get; set; }
        public SmartParagraph Certificate { get; set; }
        public SmartParagraph ConversionOfLeave { get; set; }
        public SmartParagraph ConversionOfLeaveConvertFromCount { get; set; }
        public SmartParagraph ConversionOfLeaveConvertToCount { get; set; }
        public SmartParagraph GrantOfLeave { get; set;}
        public SmartParagraph GrantOfLeaveCount { get; set; }
        public SmartParagraph NonBaseSalaryBonus { get; set;}
        public SmartParagraph NonBaseSalaryBonusPayDaysCount { get; set; }
        public SmartParagraph NonBaseSalaryBonusPayAmount { get; set; }
        public SmartParagraph OtherNonBaseSalaryBonus { get; set; }
        public SmartParagraph OtherNonBaseSalaryBonusAmount { get; set; }
        public SmartParagraph OtherRecognition { get; set; }
        public SmartParagraph OtherRecognitionSpecified { get; set; }

        // Justification Options

        public SmartParagraph OutstandingPerformance1 { get; set;}
        public SmartParagraph OutstandingPerformance2 { get; set;}
        public SmartParagraph OutstandingPerformance3 { get; set; }

        public SmartParagraph GoodConductAppraisalMinimumRatingRequirement { get; set; }
        public SmartParagraph GoodConductApprovalObtained { get; set; }
        public SmartParagraph GoodConductApprovalObtainedDate { get; set; }

        public SmartParagraph OtherJustification { get; set; }

        public SmartAwardMappedFieldSet(MainDocumentPart mainPart)
        {
            Table HeaderFields = mainPart.Document.Body.Elements<Table>().ElementAt(0);
            AgencyName = new SmartParagraph(HeaderFields.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(1));
            EmployeeName = new SmartParagraph(HeaderFields.Elements<TableRow>().ElementAt(2).Elements<TableCell>().ElementAt(1));
            EmployeeClassTitle = new SmartParagraph(HeaderFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(1));
            EmployeeDivision = new SmartParagraph(HeaderFields.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(3));

            Table MainTable = mainPart.Document.Body.Elements<Table>().ElementAt(1);
            DedicatedAttendanceAward = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(1).Elements<TableCell>().ElementAt(0));
            HonoraryAward = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(2).Elements<TableCell>().ElementAt(0));
            CreativityAward = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(3).Elements<TableCell>().ElementAt(0));
            ExemplaryPerformanceAward = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(0));
            ExemplaryPerformanceDateRange = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(4).Elements<TableCell>().ElementAt(3));
            SpecialAchievementAward = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(6).Elements<TableCell>().ElementAt(0));
            EmployeeOfTheMonth = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(7).Elements<TableCell>().ElementAt(0));
            EmployeeOfTheMonthDate = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(7).Elements<TableCell>().ElementAt(3));
            EmployeeOfTheYear = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(8).Elements<TableCell>().ElementAt(0));
            EmployeeOfTheYearDate = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(8).Elements<TableCell>().ElementAt(3));
            CountyExecutiveAward = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(9).Elements<TableCell>().ElementAt(0));

            LetterOfCommendation = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(11).Elements<TableCell>().ElementAt(0));
            Certificate = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(12).Elements<TableCell>().ElementAt(0));
            ConversionOfLeave = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(13).Elements<TableCell>().ElementAt(0));
            ConversionOfLeaveConvertFromCount = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(13).Elements<TableCell>().ElementAt(2));
            ConversionOfLeaveConvertToCount = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(13).Elements<TableCell>().ElementAt(4));
            GrantOfLeave = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(14).Elements<TableCell>().ElementAt(0));
            GrantOfLeaveCount = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(14).Elements<TableCell>().ElementAt(2));
            NonBaseSalaryBonus = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(15).Elements<TableCell>().ElementAt(0));
            NonBaseSalaryBonusPayDaysCount = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(15).Elements<TableCell>().ElementAt(2));
            NonBaseSalaryBonusPayAmount = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(15).Elements<TableCell>().ElementAt(4));
            OtherNonBaseSalaryBonus = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(16).Elements<TableCell>().ElementAt(0));
            OtherNonBaseSalaryBonusAmount = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(16).Elements<TableCell>().ElementAt(2));
            OtherRecognition = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(17).Elements<TableCell>().ElementAt(0));
            OtherRecognitionSpecified = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(17).Elements<TableCell>().ElementAt(2));
            OutstandingPerformance1 = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(22).Elements<TableCell>().ElementAt(0));
            OutstandingPerformance2 = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(21).Elements<TableCell>().ElementAt(0));
            OutstandingPerformance3 = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(20).Elements<TableCell>().ElementAt(0));
            GoodConductAppraisalMinimumRatingRequirement = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(24).Elements<TableCell>().ElementAt(0));
            GoodConductApprovalObtained = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(25).Elements<TableCell>().ElementAt(0));
            GoodConductApprovalObtainedDate = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(25).Elements<TableCell>().ElementAt(3));
            OtherJustification = new SmartParagraph(MainTable.Elements<TableRow>().ElementAt(27).Elements<TableCell>().ElementAt(0));
        }

        public void WriteXMLToFields(XElement root)
        {
            AgencyName.Write(root.Element("AgencyName").Value);
            EmployeeName.Write(root.Element("NomineeName").Value);
            EmployeeClassTitle.Write(root.Element("ClassTitle").Value);
            EmployeeDivision.Write(root.Element("Division").Value);
            XElement award = root.Element("AwardType");
            switch (award.Element("ComponentViewName").Value)
            {
                case "GoodConduct":
                    SpecialAchievementAward.Write("X");
                    GrantOfLeave.Write("X");
                    GrantOfLeaveCount.Write("2");
                    OtherRecognition.Write("X");
                    OtherRecognitionSpecified.Write("Award Ribbon");
                    GoodConductAppraisalMinimumRatingRequirement.Write("X");
                    GoodConductApprovalObtained.Write("X");
                    GoodConductApprovalObtainedDate.Write(Convert.ToDateTime(award.Element("EligibilityConfirmationDate").Value).ToString("MM/dd/yy"));
                    break;
                case "Exemplary":
                    ExemplaryPerformanceAward.Write("X");
                    ExemplaryPerformanceDateRange.Write($"{Convert.ToDateTime(award.Element("StartDate").Value).ToString("MM/yy")} - {Convert.ToDateTime(award.Element("EndDate").Value).ToString("MM/yy")}");
                    GrantOfLeave.Write("X");
                    GrantOfLeaveCount.Write(award.Element("SelectedAwardType").Value);
                    switch (award.Element("SelectedAwardType").Value)
                    {
                        case "1":
                            OutstandingPerformance1.Write("X");
                            break;
                        case "2":
                            OutstandingPerformance2.Write("X");
                            break;
                        case "3":
                            OutstandingPerformance3.Write("X");
                            break;
                    }
                    break;
                default:
                    throw new NotImplementedException("The Award Type specified in the XML Form Data is not recognized.");
            }
        }



    }
}
