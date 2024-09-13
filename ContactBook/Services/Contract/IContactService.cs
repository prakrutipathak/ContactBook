using ContactBook.Models;

namespace ContactBook.Services.Contract
{
    public interface IContactService
    {
        IEnumerable<Contact> GetContacts(char? letter);
        Contact GetContact(int id);
        string AddContact(Contact contact);
        string RemoveContact(int id);
        string ModifyContact(Contact contact);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter);
        int TotalContacts(char? letter);

    }
}
