using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class DocumentCleaner
    {
        public void RemoveComments(string filename)

        {

            //Open up the document

            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(filename, true))

            {

                //Access main document part

                MainDocumentPart mainPart = myDoc.MainDocumentPart;

                //Delete the comment part, plus any other part referenced, like image parts

                mainPart.DeletePart(mainPart.WordprocessingCommentsPart);

                //Find all elements that are assoicated with comments

                IEnumerable<OpenXmlElement> elementList = mainPart.Document.Descendants()

                .Where(el => el is CommentRangeStart ||

                el is CommentRangeEnd ||

                el is CommentReference);

                //Delete every found element

                foreach (OpenXmlElement e in elementList)

                {

                    e.Remove();

                }

                //Save changes

                mainPart.Document.Save();

            }

        }
    }
}
