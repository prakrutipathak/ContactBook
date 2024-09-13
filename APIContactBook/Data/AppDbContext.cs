using APIContactBook.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APIContactBook.Data
{
    public class AppDbContext: DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }
        public virtual IQueryable<ContactPaginated> GetPaginatedContact(char? letter,string? search, int page = 1, int pageSize = 4,string sortOrder="asc")
        {
            var letterParam = new SqlParameter("@Letter", letter ?? (object)DBNull.Value);
            var searchParam = new SqlParameter("@Search", search  ?? (object)DBNull.Value);
            var sortOrderParam = new SqlParameter("@SortOrder", sortOrder);
            var pageParam = new SqlParameter("@Page", page);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);

            return Set<ContactPaginated>().FromSqlRaw("dbo.GetPaginatedContacts  @Page,@PageSize,@Letter,@Search,@SortOrder", pageParam, pageSizeParam, letterParam, searchParam, sortOrderParam);
        }
        public virtual IQueryable<ContactPaginated> GetDetailByBirthMonth(int month)
        {
            var monthParam = new SqlParameter("@Month", month);
            return Set<ContactPaginated>().FromSqlRaw("dbo.GetDetailByBirthMonth  @Month", monthParam);
        }
        public virtual IQueryable<ContactPaginated> GetDetailByStateId(int stateId)
        {
            var stateIdParam = new SqlParameter("@StateId", stateId);
            return Set<ContactPaginated>().FromSqlRaw("dbo.GetDetailByStateId  @StateId", stateIdParam);
        }
        public virtual int CountContactBasedOnGender(char gender)
        {
            var genderParam = new SqlParameter("@Gender", gender);
            var result= Set<ReportCount>().FromSqlRaw("dbo.CountContactBasedOnGender  @Gender", genderParam).AsEnumerable() 
                    .FirstOrDefault();
            return result.count;
        }
        public virtual int CountContactBasedOnCountry(int countryId)
        {
            var countryIdParam = new SqlParameter("@CountryId", countryId);
            var result = Set<ReportCount>().FromSqlRaw("dbo.CountContactBasedOnCountry  @CountryId", countryIdParam).AsEnumerable()
                    .FirstOrDefault();
            return result.count;
        }
        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
                 .HasOne(p => p.Country)
                 .WithMany(p => p.States)
                 .HasForeignKey(p => p.CountryId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_State_Country");
            modelBuilder.Entity<ContactPaginated>().HasNoKey().ToView(null);
            modelBuilder.Entity<ReportCount>().HasNoKey().ToView(null);


        }
    }
}
