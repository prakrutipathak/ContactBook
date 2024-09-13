using ContactBook.Data.Contract;
using ContactBook.Models;
using System;

namespace ContactBook.Data.Implementation
{
    public class ContactRepository:IContactRepository

    {
        private readonly AppDbContext _appDbContext;
        public ContactRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Contact> GetAll(char? letter)
        {
            List<Contact> contacts = _appDbContext.Contacts.Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            return contacts;
        }
      
        public int TotalContacts(char? letter)
        {
            IQueryable<Contact> query = _appDbContext.Contacts;

            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.Contacts
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.Contacts
                .Where(c => c.FirstName.StartsWith(letter.ToString()))
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public Contact? GetContact(int id)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId == id);
            return contact;
        }

        public bool InsertContact(Contact contact)
        {
            var result = false;
            if (contact != null)
            {
                _appDbContext.Contacts.Add(contact);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }
        public bool UpdateContact(Contact contact)
        {
            var result = false;
            if (contact != null)
            {
                _appDbContext.Contacts.Update(contact);

                _appDbContext.SaveChanges();
                result = true;
            }
            return result;

        }

        public bool ContactExists(string name)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.FirstName == name);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ContactExists(int id, string name)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId != id && c.FirstName == name);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId == id);
            if (contact != null)
            {
                _appDbContext.Remove(contact);
                _appDbContext.SaveChanges();
                result = true;

            }
            return result;
        }


    }
}

