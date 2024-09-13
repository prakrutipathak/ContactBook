using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBook.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "User Name is Required.")]
        [DisplayName("User Name")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

    }
}
