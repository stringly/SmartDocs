using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    public class SmartParagraph
    {
        public Paragraph Paragraph { get; set; }        
        public SmartParagraph()
        {

        }
        public SmartParagraph(TableCell cell)
        {
            Paragraph = cell.Elements<Paragraph>().First();
        }

        public void Write(string newText, bool bold = true, RunProperties runProperties = null)
        {
            Run run1 = new Run();
            RunProperties runProperties1 = new RunProperties();
            if (runProperties != null)
            {
                runProperties1 = runProperties;
            }
            if(bold == true)
            {
                Bold bold1 = new Bold();
                runProperties1.Append(bold1);
            }
            run1.Append(runProperties1);
            run1.Append(new Text(newText));
            Paragraph.Append(run1);
        }
    }
}
