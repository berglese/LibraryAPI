using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models
{
    public class AppDBContext: DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookExample> BookExamples { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
