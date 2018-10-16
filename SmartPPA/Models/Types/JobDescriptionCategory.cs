using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.Types
{
    public class JobDescriptionCategory
    {
        public string Letter { get; set; }
        public int Weight { get; set; }
        public string Title { get; set; }
        public int SelectedScore {get; set;}
        public int[] Scores { get; set; }
        public List<PositionDescriptionItem> PositionDescriptionItems { get; set; }
        public List<PerformanceStandardItem> PerformanceStandardItems { get; set; }

        public JobDescriptionCategory()
        {
            PositionDescriptionItems = new List<PositionDescriptionItem>();
            PerformanceStandardItems = new List<PerformanceStandardItem>();
            Scores = new int[] {0,1,2,3,4};
        }

        public double GetCategoryRatedScore()
        {
            return Math.Round((Weight * SelectedScore * 0.01), 2);
        }

        // Creates a Header Row for the Category instance and adds its children.
        public TableRow GetCategoryHeaderRow()
        {
            TableRow tableRow1 = new TableRow();

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)360U };
            TableJustification tableJustification1 = new TableJustification() { Val = TableRowAlignmentValues.Center };

            tableRowProperties1.Append(tableRowHeight1);
            tableRowProperties1.Append(tableJustification1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "535", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Nil };

            tableCellBorders1.Append(bottomBorder1);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            KeepNext keepNext1 = new KeepNext();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            Bold bold1 = new Bold();
            FontSize fontSize1 = new FontSize() { Val = "20" };

            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSize1);

            paragraphProperties1.Append(keepNext1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            Bold bold2 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "20" };

            runProperties1.Append(bold2);
            runProperties1.Append(fontSize2);
            Text text1 = new Text();
            text1.Text = Weight.ToString();

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "545", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.Nil };

            tableCellBorders2.Append(bottomBorder2);
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellBorders2);
            tableCellProperties2.Append(shading2);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            KeepNext keepNext2 = new KeepNext();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            Bold bold3 = new Bold();
            FontSize fontSize3 = new FontSize() { Val = "20" };

            paragraphMarkRunProperties2.Append(bold3);
            paragraphMarkRunProperties2.Append(fontSize3);

            paragraphProperties2.Append(keepNext2);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            Bold bold4 = new Bold();
            FontSize fontSize4 = new FontSize() { Val = "20" };

            runProperties2.Append(bold4);
            runProperties2.Append(fontSize4);
            Text text2 = new Text();
            text2.Text = Letter + ":";

            run2.Append(runProperties2);
            run2.Append(text2);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);
            paragraph2.Append(bookmarkStart1);
            paragraph2.Append(bookmarkEnd1);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "4902", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders3.Append(rightBorder1);
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders3);
            tableCellProperties3.Append(shading3);

            Paragraph paragraph3 = new Paragraph();

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            KeepNext keepNext3 = new KeepNext();

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            Bold bold5 = new Bold();
            Caps caps1 = new Caps();
            FontSize fontSize5 = new FontSize() { Val = "20" };
            Underline underline1 = new Underline() { Val = UnderlineValues.Single };

            paragraphMarkRunProperties3.Append(bold5);
            paragraphMarkRunProperties3.Append(caps1);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(underline1);

            paragraphProperties3.Append(keepNext3);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            Bold bold6 = new Bold();
            Caps caps2 = new Caps();
            FontSize fontSize6 = new FontSize() { Val = "20" };
            Underline underline2 = new Underline() { Val = UnderlineValues.Single };

            runProperties3.Append(bold6);
            runProperties3.Append(caps2);
            runProperties3.Append(fontSize6);
            runProperties3.Append(underline2);
            Text text3 = new Text();
            text3.Text = Title;

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "403", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders4.Append(leftBorder1);
            tableCellBorders4.Append(rightBorder2);
            Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders4);
            tableCellProperties4.Append(shading4);

            Paragraph paragraph4 = new Paragraph();

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            KeepLines keepLines1 = new KeepLines();

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            FontSize fontSize7 = new FontSize() { Val = "16" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "16" };

            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript1);

            paragraphProperties4.Append(keepLines1);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            paragraph4.Append(paragraphProperties4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "5135", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Nil };

            tableCellBorders5.Append(leftBorder2);
            Shading shading5 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders5);
            tableCellProperties5.Append(shading5);

            Paragraph paragraph5 = new Paragraph();

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            KeepLines keepLines2 = new KeepLines();

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            FontSize fontSize8 = new FontSize() { Val = "16" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "16" };

            paragraphMarkRunProperties5.Append(fontSize8);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript2);

            paragraphProperties5.Append(keepLines2);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            paragraph5.Append(paragraphProperties5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            return tableRow1;
        }

        // Creates an TableRow instance and adds its children.
        public TableRow GenerateDetailsRow()
        {
            TableRow tableRow1 = new TableRow();

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            CantSplit cantSplit1 = new CantSplit();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)1063U };
            TableJustification tableJustification1 = new TableJustification() { Val = TableRowAlignmentValues.Center };

            tableRowProperties1.Append(cantSplit1);
            tableRowProperties1.Append(tableRowHeight1);
            tableRowProperties1.Append(tableJustification1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "535", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Nil };

            tableCellBorders1.Append(topBorder1);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunStyle runStyle1 = new RunStyle() { Val = "TimesLarge" };

            paragraphMarkRunProperties1.Append(runStyle1);

            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            paragraph1.Append(paragraphProperties1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "545", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            TopBorder topBorder2 = new TopBorder() { Val = BorderValues.Nil };

            tableCellBorders2.Append(topBorder2);
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9" };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellBorders2);
            tableCellProperties2.Append(shading2);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunStyle runStyle2 = new RunStyle() { Val = "TimesLarge" };

            paragraphMarkRunProperties2.Append(runStyle2);

            paragraphProperties2.Append(paragraphMarkRunProperties2);

            paragraph2.Append(paragraphProperties2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "4902", Type = TableWidthUnitValues.Dxa };
            TableCellBorders tableCellBorders3 = new TableCellBorders();
            TopBorder topBorder3 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder3 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            RightBorder rightBorder3 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(topBorder3);
            tableCellBorders3.Append(leftBorder3);
            tableCellBorders3.Append(bottomBorder3);
            tableCellBorders3.Append(rightBorder3);

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders3);
            foreach (PositionDescriptionItem p in PositionDescriptionItems)
            {
                Paragraph paragraph3 = new Paragraph();

                ParagraphProperties paragraphProperties3 = new ParagraphProperties();
                SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines(){ After = "100" };

                ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
                RunStyle runStyle3 = new RunStyle() { Val = "TimesLarge" };
                Bold bold1 = new Bold() { Val = false };
                Caps caps1 = new Caps() { Val = false };
                FontSize fontSize1 = new FontSize() { Val = "16" };
                FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "16" };

                paragraphMarkRunProperties3.Append(runStyle3);
                paragraphMarkRunProperties3.Append(bold1);
                paragraphMarkRunProperties3.Append(caps1);
                paragraphMarkRunProperties3.Append(fontSize1);
                paragraphMarkRunProperties3.Append(fontSizeComplexScript1);
                paragraphProperties3.Append(spacingBetweenLines3);
                paragraphProperties3.Append(paragraphMarkRunProperties3);
                ProofError proofError1 = new ProofError() { Type = ProofingErrorValues.SpellStart };

                Run run1 = new Run();

                RunProperties runProperties1 = new RunProperties();
                RunStyle runStyle4 = new RunStyle() { Val = "TimesLarge" };
                Bold bold2 = new Bold() { Val = false };
                Caps caps2 = new Caps() { Val = false };
                FontSize fontSize2 = new FontSize() { Val = "16" };
                FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "16" };

                runProperties1.Append(runStyle4);
                runProperties1.Append(bold2);
                runProperties1.Append(caps2);
                runProperties1.Append(fontSize2);
                runProperties1.Append(fontSizeComplexScript2);
                Text text1 = new Text();
                text1.Text = p.Detail;

                run1.Append(runProperties1);
                run1.Append(text1);
                ProofError proofError2 = new ProofError() { Type = ProofingErrorValues.SpellEnd };

                paragraph3.Append(paragraphProperties3);
                paragraph3.Append(proofError1);
                paragraph3.Append(run1);
                paragraph3.Append(proofError2);


                tableCell3.Append(paragraph3);
            } // To Here?
            tableCell3.Append(tableCellProperties3);
            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "5538", Type = TableWidthUnitValues.Dxa };
            TableCellBorders tableCellBorders4 = new TableCellBorders();
            TopBorder topBorder4 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            LeftBorder leftBorder4 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder4 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            RightBorder rightBorder4 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(topBorder4);
            tableCellBorders4.Append(leftBorder4);
            tableCellBorders4.Append(bottomBorder4);
            tableCellBorders4.Append(rightBorder4);
            GridSpan gridSpan1 = new GridSpan() { Val = 2 };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(gridSpan1);
            tableCellProperties4.Append(tableCellBorders4);
            int bulletListCount = 1;
            foreach (PerformanceStandardItem p in PerformanceStandardItems)
            {
                
                Paragraph paragraph4 = new Paragraph();
                ParagraphProperties paragraphProperties4 = new ParagraphProperties();
                SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines(){ After = "100" };

                ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
                FontSize fontSize3 = new FontSize() { Val = "16" };
                FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "16" };
                Indentation indentation4 = new Indentation(){Left = "270", Hanging ="270"};

                paragraphMarkRunProperties4.Append(fontSize3);
                paragraphMarkRunProperties4.Append(fontSizeComplexScript3);
                paragraphProperties4.Append(indentation4);
                paragraphProperties4.Append(spacingBetweenLines4);
                paragraphProperties4.Append(paragraphMarkRunProperties4);
                ProofError proofError3 = new ProofError() { Type = ProofingErrorValues.SpellStart };

                Run run2 = new Run();

                RunProperties runProperties2 = new RunProperties();
                FontSize fontSize4 = new FontSize() { Val = "16" };
                FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "16" };

                runProperties2.Append(fontSize4);
                runProperties2.Append(fontSizeComplexScript4);
                Text text2 = new Text();
                text2.Text = $"{Letter}{bulletListCount}: {p.Detail}";
                bulletListCount++;
                run2.Append(runProperties2);
                run2.Append(text2);
                ProofError proofError4 = new ProofError() { Type = ProofingErrorValues.SpellEnd };

                paragraph4.Append(paragraphProperties4);
                paragraph4.Append(proofError3);
                paragraph4.Append(run2);
                paragraph4.Append(proofError4);

            
                tableCell4.Append(paragraph4);
            }
            tableCell4.Append(tableCellProperties4);
            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            return tableRow1;
        }
    }
}
