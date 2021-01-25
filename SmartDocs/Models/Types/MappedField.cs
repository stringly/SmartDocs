﻿using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SmartDocs.Models.Types
{
    /// <summary>
    /// Class used to map Table/Cell index and value
    /// </summary>
    public class MappedField
    {
        /// <summary>
        /// Gets or sets the index of the table.
        /// </summary>
        /// <remarks>
        /// This is the index of the table in the Template document
        /// </remarks>
        /// <value>
        /// The index of the table.
        /// </value>
        public int TableIndex { get; set; }
    
        /// <summary>
        /// Gets or sets the index of the row.
        /// </summary>
        /// <value>
        /// The index of the row.
        /// </value>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the cell.
        /// </summary>
        /// <value>
        /// The index of the cell.
        /// </value>
        public int CellIndex { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Writes the data in the mapped field to the document.
        /// </summary>
        /// <param name="table">
        /// The target <see cref="T:DocumentFormat.OpenXml.Wordprocessing.Table"/>. 
        /// This parameter is required because the Table index depends on whether the MappedField is in a Header/Footer or in the document's mainpart, which means that the TableIndex is not
        /// sufficient to target the proper table. 
        /// </param>
        /// <param name="newText">The new text.</param>
        public void Write(Table table, string newText)
        {            
            TableRow row = table.Elements<TableRow>().ElementAt(RowIndex);
            TableCell cell = row.Elements<TableCell>().ElementAt(CellIndex);
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
