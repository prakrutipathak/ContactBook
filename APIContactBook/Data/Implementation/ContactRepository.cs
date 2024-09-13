using APIContactBook.Data.Contract;
using APIContactBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIContactBook.Data.Implementation
{
    public class ContactRepository: IContactRepository
    {
        private readonly IAppDbContext _appDbContext;
        public ContactRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Contact> GetAll()
        {
            List<Contact> contacts = _appDbContext.Contacts.Include(c => c.State).Include(c => c.Country).ToList();
            return contacts;
        }
        public IEnumerable<Contact> GetAllFavourite()
        {
            List<Contact> contacts = _appDbContext.Contacts.Include(c => c.State).Include(c => c.Country).Where(c => c.Favourite == true).ToList();
            return contacts;
        }

        public int TotalContacts(char? letter, string? search)
        {
            IQueryable<Contact> query = _appDbContext.Contacts;

            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));

            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            return query.Count();
        }

        public int TotalContactFavourite(char? letter)
        {
            IQueryable<Contact> query = _appDbContext.Contacts.Where(c => c.Favourite == true);

            if (letter.HasValue)
            {
                query = query
                    .Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }

        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            int skip = (page - 1) * pageSize;
            IQueryable<Contact> query = _appDbContext.Contacts
                .Include(c => c.State)
                .Include(c => c.Country);

            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));

            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
            }
            return query
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public IEnumerable<ContactPaginated> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            var results = _appDbContext.GetPaginatedContact(letter, search, page, pageSize, sortOrder);
            return results.ToList();
        }
        public IEnumerable<ContactPaginated> GetDetailByStateId(int stateId)
        {
            var results = _appDbContext.GetDetailByStateId(stateId);
            return results.ToList();
        }
        public IEnumerable<ContactPaginated> GetDetailByBirthMonth(int month)
        {
            var results = _appDbContext.GetDetailByBirthMonth(month);
            return results.ToList();
        }
        public int CountContactBasedOnCountry(int countryId)
        {
            var results = _appDbContext.CountContactBasedOnCountry(countryId);
            return results;
        }
        public int CountContactBasedOnGender(char gender)
        {
            var results = _appDbContext.CountContactBasedOnGender(gender);
            return results;
        }

        public IEnumerable<Contact> GetPaginatedFavouriteContacts(int page, int pageSize, char? letter, string? sortOrder)
        {
            int skip = (page - 1) * pageSize;
            IQueryable<Contact> query = _appDbContext.Contacts
                .Include(c => c.State)
                .Include(c => c.Country)
                .Where(c => c.Favourite == true);

            if (letter != null)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
            }

            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName);
                    break;
            }
            return query
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public Contact? GetContact(int id)
        {
            var contact = _appDbContext.Contacts.Include(c=>c.State).Include(c => c.Country).FirstOrDefault(c => c.ContactId == id);
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

        public bool ContactExists(string number)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactNumber == number);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ContactExists(int id, string number)
        {
            var contact = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId != id && c.ContactNumber == number);
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
