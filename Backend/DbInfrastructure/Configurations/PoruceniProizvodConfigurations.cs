using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DbInfrastructure.Configurations
{
    public class PoruceniProizvodConfigurations : IEntityTypeConfiguration<PoruceniProizvod>
    {
        public void Configure(EntityTypeBuilder<PoruceniProizvod> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(pp => pp.Proizvod)
                .WithMany(proizvod => proizvod.PoruceniProizvodi)
                .HasForeignKey(pp => pp.ProizvodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pp => pp.Porudzbina)
                .WithMany(porudzbina => porudzbina.PoruceniProizvodi)
                .HasForeignKey(pp => pp.PorudzbinaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
