using System.ComponentModel.DataAnnotations;

namespace SmartPPA.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int BlueDeckId { get; set; }
        public string LogonName { get; set; }
        public string DisplayName { get; set; }
    }
}
