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
                JobDescription job = new JobDescription(formData.GetValueOrDefault("ClassTitle"));
                byte[] byteArray = File.ReadAllBytes("TemplateNoJobDescriptionCell.docx");
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {

                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    foreach (KeyValuePair<string, string> kvp in formData)
                    {
                        if (kvp.Key != "ClassTitle")
                        {
                            List<MappedField> results = Fields.Where(f => f.FieldName.Contains(kvp.Key)).ToList();
                            foreach (MappedField x in results)
                            {
                                x.Write(mainPart, kvp.Value);
                            }                            
                        }                        
                    }
                    // TODO: Write values into HeaderPart
                    Table tbl = mainPart.HeaderParts.Where(x => x.)
                    List<MappedField> classTitleTargets = Fields.Where(f => f.FieldName.Contains("ClassTitle")).ToList();
                    foreach (MappedField m in classTitleTargets)
                    {
                        m.Write(mainPart, job.ClassTitle);
                    }
                    List<MappedField> gradeTargets = Fields.Where(f => f.FieldName.Contains("Grade")).ToList();
                    foreach (MappedField m in classTitleTargets)
                    {
                        m.Write(mainPart, job.Grade);
                    }

                    int currentCategoryNumber = 0;                                       
                    foreach (JobDescriptionCategory c in job.Categories)
                    {
                        currentCategoryNumber++;
                        MappedField categoryNameTarget = Fields.Where(f => f.FieldName.Contains("CategoryTitle_" + currentCategoryNumber)).Single();
                        MappedField categoryWeightTarget = Fields.Where(f => f.FieldName.Contains("CategoryWeight_" + currentCategoryNumber)).Single();
                        categoryNameTarget.Write(mainPart, c.Title);
                        categoryWeightTarget.Write(mainPart, c.Weight.ToString());
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


        public MemoryStream PopulateDocumentViaCellReference()
        {
            var mem = new MemoryStream();
            try
            {
                byte[] byteArray = File.ReadAllBytes("TemplateWithOutContentControls.docx");
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {

                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    // List<Table> tables = mainPart.Document.Body.Elements<Table>().ToList();
                    Table table = mainPart.Document.Body.Elements<Table>().First();
                    TableRow row = table.Elements<TableRow>().ElementAt(1);
                    TableCell cell = row.Elements<TableCell>().ElementAt(2);
                    //Paragraph p = new Paragraph(new Run(new Text("Hello, World!")));
                    Paragraph p = cell.Elements<Paragraph>().First();
                    Run r = new Run(new Text("TEST TEST"));
                    p.Append(r);
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
        public MemoryStream MakeDocumentFromTemplate()
        {
            var mem = new MemoryStream();
            try
            {
                byte[] byteArray = File.ReadAllBytes("Template.docx");
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {

                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    SdtElement nameControl = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "EmployeeName1").FirstOrDefault();
                    if (nameControl != null)
                    {
                        //Paragraph p = new Paragraph();
                        //Run r = new Run();
                        //Text t = new Text();
                        //t.Text = "THIS IS A MASSIVE MESS";
                        //r.Append(t);
                        //p.Append(r);
                        OpenXmlElement parent = nameControl.Parent;

                        //TableCell tc = GenerateTableCell("THIS IS A BIGGER MESS");
                        //parent.Append(tc);
                        //parent.Append(p);
                        nameControl.Remove();
                        mainPart.Document.Save();
                    }
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
        // Creates an TableCell instance and adds its children.
        public static TableCell GenerateTableCell(string text)
        {
            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4428", Type = TableWidthUnitValues.Dxa };

            tableCellProperties1.Append(tableCellWidth1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "001677F0", RsidRunAdditionDefault = "00DE3A9E" };

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts1 = new RunFonts() { Hint = FontTypeHintValues.EastAsia };

            runProperties1.Append(runFonts1);
            Text text1 = new Text();
            text1.Text = text;

            run1.Append(runProperties1);
            run1.Append(text1);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            paragraph1.Append(run1);
            paragraph1.Append(bookmarkStart1);
            paragraph1.Append(bookmarkEnd1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);
            return tableCell1;
        }
        public MemoryStream MakeDocumentWithMargins()
        {
            var mem = new MemoryStream();
            try
            {

                // Create Document
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    // Add a main document part.
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    new Document(new Body()).Save(mainPart);

                    Body body = mainPart.Document.Body;
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text("Hello, World!"));

                    SectionProperties sectionProperties1 = new SectionProperties(); 
                    PageSize pageSize1 = new PageSize() { Width = (UInt32Value)12240U, Height = (UInt32Value)15840U };
                    PageMargin pageMargin1 = new PageMargin() { Top = 533, Right = (UInt32Value)533U, Bottom = 533, Left = (UInt32Value)533U, Header = (UInt32Value)720U, Footer = (UInt32Value)720U, Gutter = (UInt32Value)0U };
                    
                    PageBorders pageBorders1 = new PageBorders() { OffsetFrom = PageBorderOffsetValues.Page };
                    TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)24U };
                    LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)24U };
                    BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)24U };
                    RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)24U };

                    pageBorders1.Append(topBorder1);
                    pageBorders1.Append(leftBorder1);
                    pageBorders1.Append(bottomBorder1);
                    pageBorders1.Append(rightBorder1);

                    sectionProperties1.Append(pageSize1);
                    sectionProperties1.Append(pageMargin1);
                    sectionProperties1.Append(pageBorders1);
                    body.Append(sectionProperties1);


                    mainPart.Document.Save();
                }
                mem.Seek(0, SeekOrigin.Begin);
                return mem;

            }
            catch
            {
                mem.Dispose();
                throw;
            }
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
                new MappedField { FieldName = "Department_1", TableIndex = 0, RowIndex = 6, CellIndex = 3},
                new MappedField { FieldName = "AgencyActivity_1", TableIndex = 0, RowIndex = 6, CellIndex = 4},

                new MappedField { FieldName = "CategoryTitle_1", TableIndex = 1, RowIndex = 2, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_1", TableIndex = 1, RowIndex = 2, CellIndex = 2},
                new MappedField { FieldName = "CategoryTotal_1", TableIndex = 1, RowIndex = 2, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_2", TableIndex = 1, RowIndex = 3, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_2", TableIndex = 1, RowIndex = 3, CellIndex = 2},
                new MappedField { FieldName = "CategoryTotal_2", TableIndex = 1, RowIndex = 3, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_3", TableIndex = 1, RowIndex = 4, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_3", TableIndex = 1, RowIndex = 4, CellIndex = 2},
                new MappedField { FieldName = "CategoryTotal_3", TableIndex = 1, RowIndex = 4, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_4", TableIndex = 1, RowIndex = 5, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_4", TableIndex = 1, RowIndex = 5, CellIndex = 2},
                new MappedField { FieldName = "CategoryTotal_4", TableIndex = 1, RowIndex = 5, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_5", TableIndex = 1, RowIndex = 6, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_5", TableIndex = 1, RowIndex = 6, CellIndex = 2},
                new MappedField { FieldName = "CategoryTotal_5", TableIndex = 1, RowIndex = 6, CellIndex = 8},

                new MappedField { FieldName = "CategoryTitle_6", TableIndex = 1, RowIndex = 7, CellIndex = 1},
                new MappedField { FieldName = "CategoryWeight_6", TableIndex = 1, RowIndex = 7, CellIndex = 2},
                new MappedField { FieldName = "CategoryTotal_6", TableIndex = 1, RowIndex = 7, CellIndex = 8},

                new MappedField { FieldName = "TotalRatingValue", TableIndex = 1, RowIndex = 8, CellIndex = 2},
                new MappedField { FieldName = "OverallAppraisal", TableIndex = 1, RowIndex = 9, CellIndex = 2},

                new MappedField {FieldName = "EmployeeName_2", TableIndex =  }
            };
        }
    }
}
