using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace ClientApplicationContactBook.ViewModels
{
    public class AddContactViewModel
    {

        [Key]
        [DisplayName("Contact Id")]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)] //data annotations
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)] //data annotations
        public string LastName { get; set; }
        public string? Image { get; set; }
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        public bool Favourite { get; set; }
        public byte[]? ImageByte { get; set; }

        //foreign key
        [DisplayName("Country name")]
        [Required(ErrorMessage = "Country name is required")]
        public int CountryId { get; set; }
        public List<ContactsCountryViewModel>? Countries { get; set; }
        [DisplayName("State name")]
        [Required(ErrorMessage = "State name is required")]
        public int StateId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? RemoveImageHidden { get; set; }
        public List<ContactsStateViewModel>? States { get; set; }

    }
}
