using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DbInfrastructure.Configurations
{
    public class PorudzbinaConfigurations : IEntityTypeConfiguration<Porudzbina>
    {
        public void Configure(EntityTypeBuilder<Porudzbina> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(porudzbina => porudzbina.AdresaDostave).HasMaxLength(100);
            builder.Property(porudzbina => porudzbina.AdresaDostave).IsRequired();
            builder.Property(porudzbina => porudzbina.Komentar).HasMaxLength(300);

            // brisanjem korisnika, obrisi sve njegove porudzbine
            builder.HasOne(porudzbina => porudzbina.Korisnik)
                .WithMany(korisnik => korisnik.MojePorudzbine)
                .HasForeignKey(porudzbina => porudzbina.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);
            

        }
    }
}
