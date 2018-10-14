using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using SmartPPA.Models.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class DocumentGenerator
    {
        private List<MappedField> Fields;
        
        public DocumentGenerator()
        {
            initializeFieldMap();
        }

        public MemoryStream PopulateDocumentViaMappedList(Dictionary<string, string> formData)
        {
            var mem = new MemoryStream();
            try
            {
                JobDescription job = new JobDescription(formData.GetValueOrDefault("Job"));
                formData.Add("ClassTitle", job.ClassTitle);
                formData.Add("Grade", job.Grade);
                formData.Add("WorkingTitle", job.WorkingTitle);
                byte[] byteArray = File.ReadAllBytes("TemplateNoJobDescriptionCell.docx");
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {

                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    foreach (KeyValuePair<string, string> kvp in formData)
                    {                           
                        List<MappedField> results = Fields.Where(f => f.FieldName.Contains(kvp.Key)).ToList();
                        foreach (MappedField x in results)
                        {
                            if (x.FieldName.Contains("Header"))
                            {
                                Table table = mainPart.HeaderParts.ElementAt(2).RootElement.Elements<Table>().ElementAt(x.TableIndex);                                
                                x.Write(table, kvp.Value);
                            }
                            else
                            {
                                Table table = mainPart.Document.Body.Elements<Table>().ElementAt(x.TableIndex);
                                x.Write(table, kvp.Value);
                            }
                                
                        }                            
                                               
                    }                 


                    int currentCategoryNumber = 0;                                       
                    foreach (JobDescriptionCategory c in job.Categories)
                    {
                        currentCategoryNumber++;
                        MappedField categoryNameTarget = Fields.Where(f => f.FieldName.Contains("CategoryTitle_" + currentCategoryNumber)).Single();
                        MappedField categoryWeightTarget = Fields.Where(f => f.FieldName.Contains("CategoryWeight_" + currentCategoryNumber)).Single();
                        categoryNameTarget.Write(mainPart.Document.Body.Elements<Table>().ElementAt(categoryNameTarget.TableIndex), c.Title);
                        categoryWeightTarget.Write(mainPart.Document.Body.Elements<Table>().ElementAt(categoryWeightTarget.TableIndex), c.Weight.ToString());
                        TableRow headerRow = c.GetCategoryHeaderRow();
                        TableRow detailsRow = c.GenerateDetailsRow();
                        Table table = mainPart.Document.Body.Elements<Table>().ElementAt(6);
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

        private void initializeFieldMap()
        {
            Fields = new List<MappedField>
            {
                
                new MappedField { FieldName = "EmployeeName_1", TableIndex = 0, RowIndex = 1, CellIndex = 1},                
                new MappedField { FieldName = "PayrollId_1", TableIndex = 0, RowIndex = 1, CellIndex = 2},
                new MappedField { FieldName = "ClassTitle_1", TableIndex = 0, RowIndex = 3, CellIndex = 1},
                new MappedField { FieldName = "Grade_1", TableIndex = 0, RowIndex = 3, CellIndex = 2},
                new MappedField { FieldName = "PositionNumber_1", TableIndex = 0, RowIndex = 3, CellIndex = 3},
                new MappedField { FieldName = "StartDate_1", TableIndex = 0, RowIndex = 4, CellIndex = 2},
                new MappedField { FieldName = "EndDate_1", TableIndex = 0, RowIndex = 4, CellIndex = 0},
                new MappedField { FieldName = "DistrictDivision_1", TableIndex = 0, RowIndex = 6, CellIndex = 3},
                new MappedField { FieldName = "AgencyActivity_1", TableIndex = 0, RowIndex = 6, CellIndex = 4},

                new MappedField { FieldName = "CategoryTitle_1", TableIndex = 1, RowIndex = 2, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_1", TableIndex = 1, RowIndex = 2, CellIndex = 2},
                new MappedField { FieldName = "CategoryRating_1_0", TableIndex = 1, RowIndex = 2, CellIndex = 3},
                new MappedField { FieldName = "CategoryRating_1_1", TableIndex = 1, RowIndex = 2, CellIndex = 4},
                new MappedField { FieldName = "CategoryRating_1_2", TableIndex = 1, RowIndex = 2, CellIndex = 5},
                new MappedField { FieldName = "CategoryRating_1_3", TableIndex = 1, RowIndex = 2, CellIndex = 6},
                new MappedField { FieldName = "CategoryRating_1_4", TableIndex = 1, RowIndex = 2, CellIndex = 7},
                new MappedField { FieldName = "CategoryTotal_1", TableIndex = 1, RowIndex = 2, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_2", TableIndex = 1, RowIndex = 3, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_2", TableIndex = 1, RowIndex = 3, CellIndex = 2},
                new MappedField { FieldName = "CategoryRating_2_0", TableIndex = 1, RowIndex = 3, CellIndex = 3},
                new MappedField { FieldName = "CategoryRating_2_1", TableIndex = 1, RowIndex = 3, CellIndex = 4},
                new MappedField { FieldName = "CategoryRating_2_2", TableIndex = 1, RowIndex = 3, CellIndex = 5},
                new MappedField { FieldName = "CategoryRating_2_3", TableIndex = 1, RowIndex = 3, CellIndex = 6},
                new MappedField { FieldName = "CategoryRating_2_4", TableIndex = 1, RowIndex = 3, CellIndex = 7},
                new MappedField { FieldName = "CategoryTotal_2", TableIndex = 1, RowIndex = 3, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_3", TableIndex = 1, RowIndex = 4, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_3", TableIndex = 1, RowIndex = 4, CellIndex = 2},
                new MappedField { FieldName = "CategoryRating_3_0", TableIndex = 1, RowIndex = 4, CellIndex = 3},
                new MappedField { FieldName = "CategoryRating_3_1", TableIndex = 1, RowIndex = 4, CellIndex = 4},
                new MappedField { FieldName = "CategoryRating_3_2", TableIndex = 1, RowIndex = 4, CellIndex = 5},
                new MappedField { FieldName = "CategoryRating_3_3", TableIndex = 1, RowIndex = 4, CellIndex = 6},
                new MappedField { FieldName = "CategoryRating_3_4", TableIndex = 1, RowIndex = 4, CellIndex = 7},
                new MappedField { FieldName = "CategoryTotal_3", TableIndex = 1, RowIndex = 4, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_4", TableIndex = 1, RowIndex = 5, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_4", TableIndex = 1, RowIndex = 5, CellIndex = 2},
                new MappedField { FieldName = "CategoryRating_4_0", TableIndex = 1, RowIndex = 5, CellIndex = 3},
                new MappedField { FieldName = "CategoryRating_4_1", TableIndex = 1, RowIndex = 5, CellIndex = 4},
                new MappedField { FieldName = "CategoryRating_4_2", TableIndex = 1, RowIndex = 5, CellIndex = 5},
                new MappedField { FieldName = "CategoryRating_4_3", TableIndex = 1, RowIndex = 5, CellIndex = 6},
                new MappedField { FieldName = "CategoryRating_4_4", TableIndex = 1, RowIndex = 5, CellIndex = 7},
                new MappedField { FieldName = "CategoryTotal_4", TableIndex = 1, RowIndex = 5, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_5", TableIndex = 1, RowIndex = 6, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_5", TableIndex = 1, RowIndex = 6, CellIndex = 2},
                new MappedField { FieldName = "CategoryRating_5_0", TableIndex = 1, RowIndex = 6, CellIndex = 3},
                new MappedField { FieldName = "CategoryRating_5_1", TableIndex = 1, RowIndex = 6, CellIndex = 4},
                new MappedField { FieldName = "CategoryRating_5_2", TableIndex = 1, RowIndex = 6, CellIndex = 5},
                new MappedField { FieldName = "CategoryRating_5_3", TableIndex = 1, RowIndex = 6, CellIndex = 6},
                new MappedField { FieldName = "CategoryRating_5_4", TableIndex = 1, RowIndex = 6, CellIndex = 7},
                new MappedField { FieldName = "CategoryTotal_5", TableIndex = 1, RowIndex = 6, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_6", TableIndex = 1, RowIndex = 7, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_6", TableIndex = 1, RowIndex = 7, CellIndex = 2},
                new MappedField { FieldName = "CategoryRating_6_0", TableIndex = 1, RowIndex = 7, CellIndex = 3},
                new MappedField { FieldName = "CategoryRating_6_1", TableIndex = 1, RowIndex = 7, CellIndex = 4},
                new MappedField { FieldName = "CategoryRating_6_2", TableIndex = 1, RowIndex = 7, CellIndex = 5},
                new MappedField { FieldName = "CategoryRating_6_3", TableIndex = 1, RowIndex = 7, CellIndex = 6},
                new MappedField { FieldName = "CategoryRating_6_4", TableIndex = 1, RowIndex = 7, CellIndex = 7},
                new MappedField { FieldName = "CategoryTotal_6", TableIndex = 1, RowIndex = 7, CellIndex = 8},

                new MappedField { FieldName = "TotalRatingValue", TableIndex = 1, RowIndex = 8, CellIndex = 2},
                new MappedField { FieldName = "OverallAppraisal", TableIndex = 1, RowIndex = 9, CellIndex = 2},
                // PAF Form Header Fields
                new MappedField { FieldName = "EmployeeName_Header", TableIndex = 0, RowIndex = 3, CellIndex = 1 },
                new MappedField { FieldName = "PayrollId_Header", TableIndex = 0, RowIndex = 3, CellIndex = 3 },
                new MappedField { FieldName = "StartDate_Header", TableIndex = 0, RowIndex = 4, CellIndex = 1 },
                new MappedField { FieldName = "EndDate_Header", TableIndex = 0, RowIndex = 4, CellIndex = 3 },
                new MappedField { FieldName = "ClassTitle_Header", TableIndex = 0, RowIndex = 4, CellIndex = 5 },
                new MappedField { FieldName = "DistrictDivision_Header", TableIndex = 0, RowIndex = 5, CellIndex = 2 },
                // PAF Performance Assessment Field
                new MappedField { FieldName = "PerformanceAssessment", TableIndex = 3, RowIndex = 1, CellIndex = 0 },
                // PAF Supervisor's Recommendation Fields
                new MappedField { FieldName = "SupervisorsRecommendations", TableIndex = 4, RowIndex = 1, CellIndex = 0 },
                // Job Description Page Fields
                new MappedField { FieldName = "EmployeeName_2", TableIndex = 5, RowIndex = 3, CellIndex = 0 },
                new MappedField { FieldName = "DistrictDivision_2", TableIndex = 5, RowIndex = 1, CellIndex = 2 }, 
                new MappedField { FieldName = "DistrictDivisionCode_1", TableIndex = 5, RowIndex = 1, CellIndex = 3 },
                new MappedField { FieldName = "PositionNumber_2", TableIndex = 5, RowIndex = 1, CellIndex = 4 }, 
                new MappedField { FieldName = "ClassTitle_2", TableIndex = 5, RowIndex = 3, CellIndex = 2 },
                new MappedField { FieldName = "Grade_1", TableIndex = 5, RowIndex = 3, CellIndex = 3 },
                new MappedField { FieldName = "WorkingTitle_1", TableIndex = 5, RowIndex = 3, CellIndex = 3 },
                new MappedField { FieldName = "PlaceOfWork_1", TableIndex = 5, RowIndex = 5, CellIndex = 0 },
                new MappedField { FieldName = "WorkingHours", TableIndex = 5, RowIndex = 5, CellIndex = 1 },
                new MappedField { FieldName = "Supervisor_1", TableIndex = 5, RowIndex = 7, CellIndex = 0 },
                new MappedField { FieldName = "Supervises_1", TableIndex = 5, RowIndex = 9, CellIndex = 0 }
            };
        }
    }
}
