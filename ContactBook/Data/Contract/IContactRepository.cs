using ContactBook.Models;

namespace ContactBook.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll(char? letter);
        Contact? GetContact(int id);
        bool ContactExists(string name);
        bool ContactExists(int id, string name);
        bool InsertContact(Contact contact);
        bool DeleteContact(int id);
        bool UpdateContact(Contact contact);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter);
        int TotalContacts(char? letter);
    }
}
