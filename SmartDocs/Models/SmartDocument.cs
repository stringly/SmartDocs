using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models
{
    public class SmartDocument
    {
        [Key]
        public int DocumentId { get; set; }
        public int AuthorUserId { get;set; }
        public virtual SmartUser Author { get;set;}
        public int TemplateId { get; set; }
        public virtual SmartTemplate Template { get; set; }
        public SmartDocumentType Type { get;set;}
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string FileName { get; set; }
        [Column(TypeName = "xml")]
        public string FormData { get; set; }
        [NotMapped]
        public XElement FormDataXml {
            get { return XElement.Parse(FormData); }
            set { FormData = value.ToString(); }
        }
        public enum SmartDocumentType
        {
            [Display(Name = "Past Performance Appraisal")]
            PPA,
            [Display(Name = "Performance Assessment Form")]
            CounselingForm,
            [Display(Name = "Job Description")]
            JobDescription,
            [Display(Name = "Award Form")]
            AwardForm

        }
    }


}
