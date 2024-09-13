using APIContactBook.Data.Contract;
using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;

namespace APIContactBook.Services.Implementation
{
    public class ContactService: IContactService
    {
        private readonly IContactRepository _contactRepository;
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetContacts()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAll();
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto() { 
                        ContactId = contact.ContactId, 
                        FirstName = contact.FirstName, 
                        LastName = contact.LastName, 
                        Image = contact.Image, 
                        ContactNumber = contact.ContactNumber, 
                        Email = contact.Email, 
                        Address = contact.Address,
                        Gender = contact.Gender,
                        Favourite = contact.Favourite,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,

                        State=new State
                        {
                            StateId = contact.State.StateId,
                            StateName=contact.State.StateName,
                        },
                        Country= new Country
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,  
                        }
                        
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetAllFavourite()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllFavourite();
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Image = contact.Image,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        Address = contact.Address,
                        Gender = contact.Gender,
                        Favourite = contact.Favourite,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,

                        State = new State
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                        },
                        Country = new Country
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }

                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }

        public ServiceResponse<int> TotalContacts(char? letter, string? search)
        {

            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContacts(letter,search);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Paginated";

            return response;
        }
        public ServiceResponse<int> TotalContactFavourite(char? letter)
        {

            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContactFavourite(letter);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Paginated";

            return response;
        }
     
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter,string? search, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize,letter,search, sortOrder);
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Image = contact.Image,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        Address = contact.Address,
                        Gender = contact.Gender,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        Favourite = contact.Favourite,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        State = new State
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                        },
                        Country = new Country
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }

                    });
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactPaginated>> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactPaginated>>();
            var contacts = _contactRepository.GetPaginatedContactsSP(page, pageSize, letter, search, sortOrder);
            if (contacts != null && contacts.Any())
            {
                List<ContactPaginated> contactDtos = new List<ContactPaginated>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactPaginated()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Image = contact.Image,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        Address = contact.Address,
                        Gender = contact.Gender,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        Favourite = contact.Favourite,
                      
                            StateName = contact.StateName,
                       
                            CountryName = contact.CountryName,
                        

                    });
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactPaginated>> GetDetailByBirthMonth(int month)
        {
            var response = new ServiceResponse<IEnumerable<ContactPaginated>>();
            var contacts = _contactRepository.GetDetailByBirthMonth(month);
            if (contacts != null && contacts.Any())
            {
                List<ContactPaginated> contactDtos = new List<ContactPaginated>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactPaginated()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Image = contact.Image,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        Address = contact.Address,
                        Gender = contact.Gender,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        Favourite = contact.Favourite,

                        StateName = contact.StateName,

                        CountryName = contact.CountryName,


                    });
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactPaginated>> GetDetailByStateId(int stateId)
        {
            var response = new ServiceResponse<IEnumerable<ContactPaginated>>();
            var contacts = _contactRepository.GetDetailByStateId(stateId);
            if (contacts != null && contacts.Any())
            {
                List<ContactPaginated> contactDtos = new List<ContactPaginated>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactPaginated()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Image = contact.Image,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        Address = contact.Address,
                        Gender = contact.Gender,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        Favourite = contact.Favourite,

                        StateName = contact.StateName,

                        CountryName = contact.CountryName,


                    });
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<int> CountContactBasedOnGender(char gender)
        {

            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.CountContactBasedOnGender(gender);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Count";

            return response;
        }
        public ServiceResponse<int> CountContactBasedOnCountry(int countryId)
        {

            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.CountContactBasedOnCountry(countryId);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Count";

            return response;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedFavouriteContacts(int page, int pageSize, char? letter, string? sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedFavouriteContacts(page, pageSize, letter,sortOrder);
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                        contactDtos.Add(new ContactDto()
                        {
                            ContactId = contact.ContactId,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Image = contact.Image,
                            ContactNumber = contact.ContactNumber,
                            Email = contact.Email,
                            Address = contact.Address,
                            Gender = contact.Gender,
                            BirthDate = contact.BirthDate,
                            ImageByte = contact.ImageByte,
                            Favourite = contact.Favourite,
                            StateId = contact.StateId,
                            CountryId = contact.CountryId,
                            State = new State
                            {
                                StateId = contact.State.StateId,
                                StateName = contact.State.StateName,
                            },
                            Country = new Country
                            {
                                CountryId = contact.Country.CountryId,
                                CountryName = contact.Country.CountryName,
                            }

                        });
                    
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }



        public ServiceResponse<ContactDto> GetContact(int id)
        {
            var response = new ServiceResponse<ContactDto>();
            var existingContact = _contactRepository.GetContact(id);
            if (existingContact != null)
            {
                var contact = new ContactDto()
                {
                    ContactId = id,
                    FirstName = existingContact.FirstName,
                    LastName = existingContact.LastName,
                    ContactNumber = existingContact.ContactNumber,
                    Image =existingContact.Image,
                    Email = existingContact.Email,
                    Address = existingContact.Address,
                    Gender = existingContact.Gender,
                    BirthDate = existingContact.BirthDate,
                    ImageByte = existingContact.ImageByte,
                    Favourite = existingContact.Favourite,
                    StateId = existingContact.StateId,
                    CountryId = existingContact.CountryId,
                    State = new State
                    {
                        StateId = existingContact.State.StateId,
                        StateName = existingContact.State.StateName,
                    },
                    Country = new Country
                    {
                        CountryId = existingContact.Country.CountryId,
                        CountryName = existingContact.Country.CountryName,
                    }
                };
                response.Data = contact;

            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;


        }

        public ServiceResponse<string> AddContact(Contact contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.ContactNumber))
            {
                response.Success = false;
                response.Message = "Contact already exists.";
                return response;
            }
            if (contact.BirthDate > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Enter valid date";
                return response;
            }
            var result = _contactRepository.InsertContact(contact);
            if (result)
            {
                response.Success = true;
                response.Message = "Contact saved successfully";
                response.Data = contact.ContactId.ToString();
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong after sometime";
            }
            return response;
        }
        public ServiceResponse<string> ModifyContact(Contact contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.ContactId, contact.ContactNumber))
            {
                response.Success = false;
                response.Message = "Contact Exists!";
                return response;

            }
            if (contact.BirthDate > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Enter valid date";
                return response;
            }
            var existingContact = _contactRepository.GetContact(contact.ContactId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Address = contact.Address;
                existingContact.Email = contact.Email;
                existingContact.Image = contact.Image;
                existingContact.ContactNumber=contact.ContactNumber;
                existingContact.Gender = contact.Gender;
                existingContact.BirthDate= contact.BirthDate;
                existingContact.ImageByte= contact.ImageByte;
                existingContact.Favourite = contact.Favourite;
                existingContact.CountryId = contact.CountryId;
                existingContact.StateId=contact.StateId;


                result = _contactRepository.UpdateContact(existingContact);
            }
           
            if (result)
            {
                response.Success = true;
                response.Message = "Contact Updated successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong after sometime";
            }
            return response;
        }

        public ServiceResponse<string> RemoveContact(int id)
        {
            var response = new ServiceResponse<string>();
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                response.Success = true;
                response.Message= "Contact Deleted Successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong please try after sometime";      
            }
            return response;
        }
    }
}
