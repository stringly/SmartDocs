using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using SmartPPA.Models.Types;
using SmartPPA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartPPA.Models
{
    public class SmartPPAGenerator
    {
        private IDocumentRepository _repository;
        private List<MappedField> Fields;
        private Dictionary<string, string> formData;
        private JobDescription job;
        public SmartPPA dbPPA;
        
        public SmartPPAGenerator(IDocumentRepository repo)
        {
            _repository = repo;
            initializeFieldMap();
        }
        
        public void ReDownloadPPA(int PPAid)
        {
            SmartPPA dbPPA = _repository.PPAs.FirstOrDefault(x => x.PPAId == PPAid);
            PPAFormViewModel vm = new PPAFormViewModel(dbPPA);
            SeedFormInfo(vm);
        }

        public void SeedFormInfo(PPAFormViewModel form)
        {            

            Dictionary<string, string> results = new Dictionary<string, string>();
            SmartJob  dbJob = _repository.Jobs.FirstOrDefault(j => j.JobId == form.JobId);            
            SmartUser author = _repository.Users.FirstOrDefault(u => u.UserId == form.AuthorUserId);
            // I think this is the point I want to save the FormData XML...
            // The idea is that I can reconsitute the VM Form and re-enter here to re-create the document
            // Snapshot it here, but don't write the record until the formData seeds successfully?
            dbPPA = new SmartPPA
            {
                PPAId = form.PPAId,
                EmployeeFirstName = form.FirstName,
                EmployeeLastName = form.LastName,
                DepartmentIdNumber = form.DepartmentIdNumber,
                PayrollIdNumber = form.PayrollIdNumber,
                PositionNumber = form.PositionNumber,
                DepartmentDivision = form.DepartmentDivision,
                DepartmentDivisionCode = form.DepartmentDivisionCode,
                WorkplaceAddress = form.WorkPlaceAddress,
                SupervisedByEmployee = form.SupervisedByEmployee, 
                StartDate = form.StartDate,
                EndDate = form.EndDate,
                AssessmentComments = form.Assessment,
                RecommendationComments = form.Recommendation,
                Job = dbJob,
                Owner = author,
                CategoryScore_1 = form.Categories[0]?.SelectedScore ?? null,
                CategoryScore_2 = form.Categories[1]?.SelectedScore ?? null,
                CategoryScore_3 = form.Categories[2]?.SelectedScore ?? null,
                CategoryScore_4 = form.Categories[3]?.SelectedScore ?? null,
                CategoryScore_5 = form.Categories[4]?.SelectedScore ?? null,
                CategoryScore_6 = form.Categories[5]?.SelectedScore ?? null,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Template = null, // TODO: Add template
                DocumentName = $"{form.LastName}, {form.FirstName} {form.DepartmentIdNumber} {form.EndDate.ToString("yyyy")} Performance Appraisal.docx"
            };

            job = new JobDescription(dbJob);
            for (int i = 0; i < job.Categories.Count(); i++)
            {
                job.Categories[i].SelectedScore = form.Categories[i].SelectedScore;
            }
            
            // Map the VM to a Dict because I am very lazy            
            results.Add("EmployeeName", $"{form.LastName}, {form.FirstName}");
            results.Add("PayrollId", form.PayrollIdNumber);
            results.Add("PositionNumber", form.PositionNumber);
            results.Add("StartDate", form.StartDate.ToShortDateString());
            results.Add("EndDate", form.EndDate.ToShortDateString());
            results.Add("DistrictDivision", form.DepartmentDivision);
            results.Add("Assessment", form.Assessment);
            results.Add("Recommendations", form.Recommendation);
            results.Add("AgencyActivity", form.DepartmentDivisionCode);
            results.Add("PlaceOfWork", form.WorkPlaceAddress);
            results.Add("Supervisor", author.DisplayName);
            results.Add("Supervises", form?.SupervisedByEmployee ?? "N/A");
            results.Add("ClassTitle", job.ClassTitle);
            results.Add("Grade", job.Grade);
            results.Add("WorkingTitle", job.WorkingTitle);
            
            for (int i = 0; i < form.Categories.Count(); i++)
            {
                switch (job.Categories[i].SelectedScore)
                {
                    case 0:
                        results.Add($"CategoryRating_{i+1}_0", "\u2713");                        
                        break;
                    case 1:
                        results.Add($"CategoryRating_{i+1}_1", "\u2713");
                        break;
                    case 2:
                        results.Add($"CategoryRating_{i+1}_2", "\u2713");
                        break;
                    case 3:
                        results.Add($"CategoryRating_{i+1}_3", "\u2713");
                        break;
                    case 4:
                        results.Add($"CategoryRating_{i+1}_4", "\u2713");
                        break;
                }

                results.Add($"CategoryTotal_{i+1}", job.Categories[i].GetCategoryRatedScore().ToString());
            }
            results.Add("TotalRatingValue", job.GetOverallScore().ToString());
            results.Add("OverallAppraisal", job.GetOverallRating());
            formData = results;
            
        }        

        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            int chunkCount = 1;
            try
            {
                byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.TemplateId == 1).DataStream;
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
                                if (x.FieldName == "Recommendations" || x.FieldName == "Assessment")
                                {
                                    
                                    string altChunkId = $"AltChunkId{chunkCount}";
                                    chunkCount++;
                                    AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);
                                    using (Stream chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
                                    {
                                        using (StreamWriter stringStream = new StreamWriter(chunkStream))
                                        {
                                            // TODO: Is this defaulted to the right font?
                                            stringStream.Write($"<html style='font-size:10px'>{kvp.Value}</html>");
                                        }
                                    }
                                    AltChunk altChunk = new AltChunk();
                                    altChunk.Id = altChunkId;
                                    TableRow row = table.Elements<TableRow>().ElementAt(1);
                                    TableCell cell = row.Elements<TableCell>().ElementAt(0);
                                    Paragraph p = cell.Elements<Paragraph>().First();
                                    p.InsertBeforeSelf(altChunk);
                                    mainPart.Document.Save();
                                }                                
                                else
                                {
                                    x.Write(table, kvp.Value);
                                }
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
            // If we get this far, we can save the Db entry
            mem.Seek(0, SeekOrigin.Begin);
            _repository.SaveSmartPPA(dbPPA);
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
                new MappedField { FieldName = "StartDate_1", TableIndex = 0, RowIndex = 4, CellIndex = 0},
                new MappedField { FieldName = "EndDate_1", TableIndex = 0, RowIndex = 4, CellIndex = 2},
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
                new MappedField { FieldName = "DistrictDivision_Header", TableIndex = 0, RowIndex = 5, CellIndex = 1 },
                // PAF Performance Assessment Field
                new MappedField { FieldName = "Assessment", TableIndex = 3, RowIndex = 1, CellIndex = 0 },
                // PAF Supervisor's Recommendation Fields
                new MappedField { FieldName = "Recommendations", TableIndex = 4, RowIndex = 1, CellIndex = 0 },
                // Job Description Page Fields
                new MappedField { FieldName = "EmployeeName_2", TableIndex = 5, RowIndex = 3, CellIndex = 0 },
                new MappedField { FieldName = "DistrictDivision_2", TableIndex = 5, RowIndex = 1, CellIndex = 2 }, 
                new MappedField { FieldName = "AgencyActivity_2", TableIndex = 5, RowIndex = 1, CellIndex = 3 },
                new MappedField { FieldName = "PositionNumber_2", TableIndex = 5, RowIndex = 1, CellIndex = 4 }, 
                new MappedField { FieldName = "ClassTitle_2", TableIndex = 5, RowIndex = 3, CellIndex = 1 },
                new MappedField { FieldName = "Grade_2", TableIndex = 5, RowIndex = 3, CellIndex = 2 },
                new MappedField { FieldName = "WorkingTitle_1", TableIndex = 5, RowIndex = 5, CellIndex = 1 },
                new MappedField { FieldName = "PlaceOfWork_1", TableIndex = 5, RowIndex = 7, CellIndex = 0 },
                new MappedField { FieldName = "WorkingHours", TableIndex = 5, RowIndex = 7, CellIndex = 1 },
                new MappedField { FieldName = "Supervisor_1", TableIndex = 5, RowIndex = 9, CellIndex = 0 },
                new MappedField { FieldName = "Supervises_1", TableIndex = 5, RowIndex = 11, CellIndex = 0 }
            };
        }


        // TESTING /////////////////////////////////////////////////////////////////////
        public void WriteTemplate()
        {
            SmartTemplate temp = new SmartTemplate();
            byte[] byteArray = File.ReadAllBytes("TemplateNoJobDescriptionCell.docx");
            temp.DocumentName = "SmartPPA XML v1.0";
            temp.DataStream = byteArray;
            _repository.SaveTemplate(temp);
        }
        // HOLY SHIT IT WORKS!!!!!!!!!!!!!!!!!!!!!!!!!!
        public MemoryStream TestAltChunkInsert()
        {
            var mem = new MemoryStream();
            string altChunkId = "AltChunkId1";
            byte[] byteArray = File.ReadAllBytes("TemplateNoJobDescriptionCell.docx");
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);
                using (Stream chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stringStream = new StreamWriter(chunkStream))
                    {
                        stringStream.Write("<html><p><strong>In the merry month of June</strong></p><ul><li><strong><em>From me home I started</em></strong></li><li><strong><em>Left the girls of Tume</em></strong></li><li><strong><em>Merely brok-en hearted</em></strong></li></ul><p>Test.</p></html>");
                    }
                }
                AltChunk altChunk = new AltChunk();
                altChunk.Id = altChunkId;
                Table table = mainPart.Document.Body.Elements<Table>().ElementAt(3);
                TableRow row = table.Elements<TableRow>().ElementAt(1);
                TableCell cell = row.Elements<TableCell>().ElementAt(0);
                Paragraph p = cell.Elements<Paragraph>().First();
                p.InsertBeforeSelf(altChunk);
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }
    }
}
