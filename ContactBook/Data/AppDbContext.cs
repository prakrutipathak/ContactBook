using ContactBook.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactBook.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
