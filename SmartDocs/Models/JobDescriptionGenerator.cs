using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartDocs.Models
{
    public class JobDescriptionGenerator
    {
        public List<MappedField> Fields { get;set;}

        private JobDescription _job;
        public string EmployeeName { get;set;}
        public string DepartmentDivisionName { get; set; }
        public string DepartmentDivisionCode { get; set; }
        public string PositionNumber { get; set; }
        public string ClassTitle { get; set; }
        public string Grade { get; set; }
        public string WorkingTitle { get; set; }
        public string PlaceOfWork { get; set; }
        public string WorkingHours { get; set; }
        public string Supervisor { get; set; }
        public string Supervises { get; set; }

        public JobDescriptionGenerator(ServeJobDescriptionViewModel formData, JobDescription job)
        {
            _job = job;
            EmployeeName = $"{formData.EmployeeFirstName}, {formData.EmployeeLastName}";
            DepartmentDivisionName = formData.DepartmentDivisionName;
            DepartmentDivisionCode = formData.DepartmentDivisionCode;
            PositionNumber = formData.PositionNumber;
            ClassTitle = formData.ClassTitle;
            Grade = formData.Grade;
            WorkingTitle = formData.WorkingTitle;
            PlaceOfWork = formData.PlaceOfWork;
            WorkingHours = formData.WorkingHours;
            Supervisor = formData.Supervisor;
            Supervises = formData.Supervises;
            InitializeFieldMap();
            
        }

        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            try
            {
                byte[] byteArray = File.ReadAllBytes("Resources/Templates/Job_Description_Template.docx");
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {

                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    

                    int currentCategoryNumber = 0;
                    foreach (JobDescriptionCategory c in _job.Categories)
                    {
                        currentCategoryNumber++;
                        TableRow headerRow = c.GetCategoryHeaderRow();
                        TableRow detailsRow = c.GenerateDetailsRow();
                        Table table = mainPart.Document.Body.Elements<Table>().ElementAt(1);
                        table.Append(headerRow);
                        table.Append(detailsRow);
                    }
                    mainPart.Document.Save();
                }
            }
            catch
            {
                mem.Dispose();
                throw;
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }


        private void InitializeFieldMap()
        {
            // set mapped fields here.
            Fields = new List<MappedField>
            {
                new MappedField { FieldName = "EmployeeName", TableIndex = 0, RowIndex = 3, CellIndex = 0 },
                new MappedField { FieldName = "DistrictDivision", TableIndex = 0, RowIndex = 1, CellIndex = 2 },
                new MappedField { FieldName = "AgencyActivity", TableIndex = 0, RowIndex = 1, CellIndex = 3 },
                new MappedField { FieldName = "PositionNumber", TableIndex = 0, RowIndex = 1, CellIndex = 4 },
                new MappedField { FieldName = "JobClass", TableIndex = 0, RowIndex = 3, CellIndex = 1 },
                new MappedField { FieldName = "Grade", TableIndex = 0, RowIndex = 3, CellIndex = 2 },
                new MappedField { FieldName = "JobWorking", TableIndex = 0, RowIndex = 5, CellIndex = 1 },
                new MappedField { FieldName = "PlaceOfWork", TableIndex = 0, RowIndex = 7, CellIndex = 0 },
                new MappedField { FieldName = "WorkingHours", TableIndex = 0, RowIndex = 7, CellIndex = 1 },
                new MappedField { FieldName = "Supervisor", TableIndex = 0, RowIndex = 9, CellIndex = 0 },
                new MappedField { FieldName = "Supervises", TableIndex = 0, RowIndex = 11, CellIndex = 0 }
            };
        }
    }
}
