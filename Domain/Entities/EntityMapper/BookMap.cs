using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.EntityMapper
{
    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);

            builder.Property(x => x.Author)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);

            builder.Property(x => x.Publisher)
                .IsRequired()
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Books)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
