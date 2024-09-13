using APIContactBook.Models;

namespace APIContactBook.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        Contact? GetContact(int id);
        bool ContactExists(string number);
        bool ContactExists(int id, string number);
        bool InsertContact(Contact contact);
        bool DeleteContact(int id);
        bool UpdateContact(Contact contact);
         int TotalContacts(char? letter, string? search);
        int TotalContactFavourite(char? letter);
        IEnumerable<Contact> GetAllFavourite();
         IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter, string sortOrder, string? search);
         IEnumerable<Contact> GetPaginatedFavouriteContacts(int page, int pageSize, char? letterr, string? sortOrder);
        IEnumerable<ContactPaginated> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder);
         IEnumerable<ContactPaginated> GetDetailByStateId(int stateId);
         IEnumerable<ContactPaginated> GetDetailByBirthMonth(int month);
         int CountContactBasedOnCountry(int countryId);
         int CountContactBasedOnGender(char gender);

    }
}
