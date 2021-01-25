using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    public class SmartCategory
    {
        private SmartParagraph CategoryTitle { get; set; }
        private SmartParagraph CategoryWeight { get; set; }
        private SmartParagraph CategoryRating0 { get; set; }
        private SmartParagraph CategoryRating1 { get; set; }
        private SmartParagraph CategoryRating2 { get; set; }
        private SmartParagraph CategoryRating3 { get; set; }
        private SmartParagraph CategoryRating4 { get; set; }
        private SmartParagraph CategoryTotal { get; set; }

        public SmartCategory(TableRow row)
        {
            CategoryTitle = new SmartParagraph(row.Elements<TableCell>().ElementAt(1));
            CategoryWeight = new SmartParagraph(row.Elements<TableCell>().ElementAt(2));
            CategoryRating0 = new SmartParagraph(row.Elements<TableCell>().ElementAt(3));
            CategoryRating1 = new SmartParagraph(row.Elements<TableCell>().ElementAt(4));
            CategoryRating2 = new SmartParagraph(row.Elements<TableCell>().ElementAt(5));
            CategoryRating3 = new SmartParagraph(row.Elements<TableCell>().ElementAt(6));
            CategoryRating4 = new SmartParagraph(row.Elements<TableCell>().ElementAt(7));
            CategoryTotal = new SmartParagraph(row.Elements<TableCell>().ElementAt(8));

        }

        public void Write(string categoryTitle, int categoryWeight, int selectedScore, double categoryScore)
        {
            CategoryTitle.Write(categoryTitle);
            CategoryWeight.Write(categoryWeight.ToString());
            RunFonts runFonts1 = new RunFonts { Ascii = "Wingdings" };
            RunProperties runProperties1 = new RunProperties();
            runProperties1.Append(runFonts1);
            switch (selectedScore)
            {
                case 1:
                    CategoryRating1.Write("\u2713", true, runProperties1);
                    break;
                case 2:
                    CategoryRating2.Write("\u2713", true, runProperties1);
                    break;
                case 3:
                    CategoryRating3.Write("\u2713", true, runProperties1);
                    break;
                case 4:
                    CategoryRating4.Write("\u2713", true, runProperties1);
                    break;
                default:
                    CategoryRating0.Write("\u2713", true, runProperties1);
                    break;
            }
            CategoryTotal.Write(categoryScore.ToString());
        }
    }
}
