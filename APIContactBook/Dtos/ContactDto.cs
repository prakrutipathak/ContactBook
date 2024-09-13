﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using APIContactBook.Models;

namespace APIContactBook.Dtos
{
    public class ContactDto
    {
        [Key]
        [DisplayName("Contact Id")]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(50)] //data annotations
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(50)] //data annotations
        public string LastName { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact number is required")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool Favourite { get; set; }
        //foreign key
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
