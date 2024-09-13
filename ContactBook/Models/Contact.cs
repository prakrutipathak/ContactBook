using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBook.Models
{
    public class Contact
    {
        [Key]
        [DisplayName("Contact Id")]
        public int ContactId { get; set; }

        [Required]
        [StringLength(50)] //data annotations
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)] //data annotations
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
