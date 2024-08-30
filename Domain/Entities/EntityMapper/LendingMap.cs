using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.EntityMapper
{
    public class LendingMap : IEntityTypeConfiguration<Lending>
    {
        public void Configure(EntityTypeBuilder<Lending> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.Lendings)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x=>x.Book)
                .WithMany(x=>x.Lendings)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
