using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("GetAllContactsByPaginationSP")]
        public IActionResult GetPaginatedContactsSP(char? letter, string? search, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactPaginated>>();
            response = _contactService.GetPaginatedContactsSP(page, pageSize, letter, search, sortOrder);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("GetDetailByStateId/{stateId}")]
        public IActionResult GetDetailByStateId(int stateId)
        {
            var response = new ServiceResponse<IEnumerable<ContactPaginated>>();
            response = _contactService.GetDetailByStateId(stateId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("GetDetailByBirthMonth/{month}")]
        public IActionResult GetDetailByBirthMonth(int month)
         {
            var response = new ServiceResponse<IEnumerable<ContactPaginated>>();
            response = _contactService.GetDetailByBirthMonth(month);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("CountContactBasedOnGender/{gender}")]
        public IActionResult CountContactBasedOnGender(char gender)
        {
            var response = _contactService.CountContactBasedOnGender(gender);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountContactBasedOnCountry/{countryId}")]
        public IActionResult CountContactBasedOnCountry(int countryId)
        {
            var response = _contactService.CountContactBasedOnCountry(countryId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var response = _contactService.GetContacts();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllFavourite")]
        public IActionResult GetAllFavourite()
        {
            var response = _contactService.GetAllFavourite();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContactById/{id}")]
        public IActionResult GetContactById(int id)
        {
            var response = _contactService.GetContact(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        //[Authorize]
        [HttpPost("Create")]
        public IActionResult AddContact(AddContactDto contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact()
                {
                    FirstName = contactDto.FirstName,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    Image = contactDto.Image,
                    ContactNumber= contactDto.ContactNumber,
                    ImageByte = contactDto.ImageByte,
                    BirthDate = contactDto.BirthDate,
                    Address = contactDto.Address,
                    Gender = contactDto.Gender,
                    Favourite = contactDto.Favourite,
                    StateId = contactDto.StateId,
                    CountryId = contactDto.CountryId,
                   
                };
                var result = _contactService.AddContact(contact);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        //[Authorize]
        [HttpPut("Edit")]
        public IActionResult Edit(UpdateContactDto contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact()
                {
                    ContactId=contactDto.ContactId,
                    FirstName = contactDto.FirstName,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    Image = contactDto.Image,
                    BirthDate = contactDto.BirthDate,
                    ImageByte = contactDto.ImageByte,
                    Address = contactDto.Address,
                    ContactNumber = contactDto.ContactNumber,
                    Gender = contactDto.Gender,
                    Favourite = contactDto.Favourite,
                    StateId = contactDto.StateId,
                    CountryId = contactDto.CountryId,
                };
                var result = _contactService.ModifyContact(contact);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
       // [Authorize]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id > 0)
            {
                var result = _contactService.RemoveContact(id);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest("Please enter proper data");
            }
        }
        [HttpGet("GetAllContactsByPagination")]
        public IActionResult GetPaginatedContacts(char? letter, string? search, int page = 1, int pageSize = 2,  string sortOrder= "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
                response = _contactService.GetPaginatedContacts(page, pageSize, letter,search, sortOrder);
            
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("GetPaginatedFavouriteContacts")]
        public IActionResult GetPaginatedFavouriteContacts(char? letter, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
           
                response = _contactService.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder);
            
            
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpGet("GetContactsCount")]
        public IActionResult GetContactsCount(char? letter, string? search)
        {
            var response = _contactService.TotalContacts(letter,search);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("TotalContactFavourite")]
        public IActionResult TotalContactFavourite(char? letter)
        {
            var response = _contactService.TotalContactFavourite(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


    }
}
