using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Code = "10",
                    Name = "برنامه نویسی",
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Code = "11",
                    Name = "ریاضیات",
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Code = "12",
                    Name = "عمومی",
                }
            );
        }
    }
}
