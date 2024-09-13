using APIContactBook.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace APIContactBook.Data
{
    public interface IAppDbContext: IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }
        IQueryable<ContactPaginated> GetPaginatedContact(char? letter, string? search, int page = 1, int pageSize = 4, string sortOrder = "asc");
        IQueryable<ContactPaginated> GetDetailByBirthMonth(int month);
        IQueryable<ContactPaginated> GetDetailByStateId(int stateId);
        int CountContactBasedOnGender(char gender);
        int CountContactBasedOnCountry(int countryId);
    }
}
