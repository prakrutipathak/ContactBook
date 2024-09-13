using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace APIContactBook.Models
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
        [Required]
        [StringLength(10)]
        public string ContactNumber { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Favourite { get; set; }
        public string Address { get; set; }
        //foreign key
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
