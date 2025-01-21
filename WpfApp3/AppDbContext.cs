using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WpfApp3
{
    public class LibraryContext : DbContext
    {
        internal DbSet<Book> books { get; set; }
        internal DbSet<Person> people { get; set; }
        internal DbSet<Tabel> tabels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=library;Username=postgres;Password=admin");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tabel>()
                .HasKey(t => new { t.bookId, t.personId });

            modelBuilder.Entity<Tabel>()
                .HasOne(t => t.book)
                .WithMany() 
                .HasForeignKey(t => t.bookId);

            modelBuilder.Entity<Tabel>()
            .HasOne(t => t.person)
                .WithMany() 
                .HasForeignKey(t => t.personId);
        }
    }
}