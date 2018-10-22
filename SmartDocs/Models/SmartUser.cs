using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    public class SmartUser
    {
        [Key]
        public int UserId { get; set; }
        public int BlueDeckId { get; set; }
        public string LogonName { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
    }
}
