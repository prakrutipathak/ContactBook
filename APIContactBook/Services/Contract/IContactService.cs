using APIContactBook.Dtos;
using APIContactBook.Models;

namespace APIContactBook.Services.Contract
{
    public interface IContactService
    {
        ServiceResponse<IEnumerable<ContactDto>> GetContacts();
        ServiceResponse<IEnumerable<ContactDto>> GetAllFavourite();
        ServiceResponse<ContactDto> GetContact(int id);
        ServiceResponse<string> AddContact(Contact contact);
        ServiceResponse<string> ModifyContact(Contact contact);
        ServiceResponse<string> RemoveContact(int id);
        ServiceResponse<int> TotalContactFavourite(char? letter);
         ServiceResponse<int> TotalContacts(char? letter, string? search);
         ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter, string? search, string sortOrder);
         ServiceResponse<IEnumerable<ContactDto>> GetPaginatedFavouriteContacts(int page, int pageSize, char? letterr, string sortOrder);
        ServiceResponse<IEnumerable<ContactPaginated>> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder);
        ServiceResponse<IEnumerable<ContactPaginated>> GetDetailByBirthMonth(int month);
        ServiceResponse<IEnumerable<ContactPaginated>> GetDetailByStateId(int stateId);
        ServiceResponse<int> CountContactBasedOnGender(char gender);
        ServiceResponse<int> CountContactBasedOnCountry(int countryId);

    }
}
