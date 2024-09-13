using ContactBook.Data.Contract;
using ContactBook.Models;
using ContactBook.Services.Contract;

namespace ContactBook.Services.Implementation
{
    public class ContactService:IContactService
    {
        private readonly IContactRepository _contactRepository;
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IEnumerable<Contact> GetContacts(char? letter)
        {

            var contacts = _contactRepository.GetAll(letter);
            if (contacts != null && contacts.Any())
            {
                return contacts;
            }
            return new List<Contact>();
        }
        public int TotalContacts(char? letter)
        {
            return _contactRepository.TotalContacts(letter);
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize)
        {
            return _contactRepository.GetPaginatedContacts(page, pageSize);
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter)
        {
            return _contactRepository.GetPaginatedContacts(page, pageSize, letter);
        }


        public Contact GetContact(int id)
        {
            var contact = _contactRepository.GetContact(id);
            return contact;
        }
        public string AddContact(Contact contact)
        {
            if (_contactRepository.ContactExists(contact.FirstName))
            {
                return "Contact already exists.";
            }
            var result = _contactRepository.InsertContact(contact);
            return result ? "Contact saved successfully" : "Something went wrong after sometime";
        }
        public string ModifyContact(Contact contact)
        {
            var message = string.Empty;
            if (_contactRepository.ContactExists(contact.ContactId, contact.FirstName))
            {
                message = "Contact Exists!";
                return message;

            }
            var existingContact = _contactRepository.GetContact(contact.ContactId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Address = contact.Address;
                existingContact.Email = contact.Email;


                result = _contactRepository.UpdateContact(existingContact);
            }
            message = result ? "Contact Updated Successfully!" : "Something went wrong please try after sometime";
            return message;
        }

        public string RemoveContact(int id)
        {
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                return "Contact Deleted Successfully";
            }
            else
            {
                return "Something went wrong please try after sometime";
            }
        }
    }
}
