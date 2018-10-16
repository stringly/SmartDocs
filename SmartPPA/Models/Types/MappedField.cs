using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SmartPPA.Models.Types
{
    public class MappedField
    {
        public int TableIndex { get; set; }
        public int RowIndex { get; set; }
        public int CellIndex { get; set; }
        public string FieldName { get; set; }

        public void Write(Table table, string newText)
        {            
            TableRow row = table.Elements<TableRow>().ElementAt(RowIndex);
            TableCell cell = row.Elements<TableCell>().ElementAt(CellIndex);
            //Paragraph p = new Paragraph(new Run(new Text("Hello, World!")));
            Paragraph p = cell.Elements<Paragraph>().First();
            Run r = new Run();
            RunProperties runProperties1 = new RunProperties();
            if (FieldName.Contains("CategoryRating"))
            {
                var runFont1 = new RunFonts { Ascii = "Wingdings" };
                runProperties1.Append(runFont1);
            }
            else if (FieldName != "Assessment" && FieldName != "Recommendations")
            {                
                Bold bold1 = new Bold();
                runProperties1.Append(bold1);                
            }
            r.Append(runProperties1);
            Text t = new Text(newText);            
            r.Append(t);
            p.Append(r);
        }


    }
}
