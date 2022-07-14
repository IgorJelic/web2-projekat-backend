using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DbInfrastructure.Configurations
{
    public class ProizvodConfigurations : IEntityTypeConfiguration<Proizvod>
    {
        public void Configure(EntityTypeBuilder<Proizvod> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(proizvod => proizvod.Ime).HasMaxLength(60);
            builder.Property(proizvod => proizvod.Ime).IsRequired();
            builder.Property(proizvod => proizvod.Sastojci).HasMaxLength(200);

            builder.HasIndex(proizvod => proizvod.Ime).IsUnique();

            //builder.HasMany(proizvod => proizvod.PoruceniProizvodi)
            //    .WithOne(poruceniProizvod => poruceniProizvod.Proizvod)
            //    .HasForeignKey(poruceniProizvod => poruceniProizvod.ProizvodId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
