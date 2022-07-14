using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DbInfrastructure.Configurations
{
    public class KorisnikConfigurations : IEntityTypeConfiguration<Korisnik>
    {
        public void Configure(EntityTypeBuilder<Korisnik> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(korisnik => korisnik.Ime).HasMaxLength(30);
            builder.Property(korisnik => korisnik.Ime).IsRequired();
            builder.Property(korisnik => korisnik.Prezime).HasMaxLength(60);
            builder.Property(korisnik => korisnik.Prezime).IsRequired();
            builder.Property(korisnik => korisnik.Email).HasMaxLength(320);
            builder.Property(korisnik => korisnik.Email).IsRequired();
            builder.HasIndex(korisnik => korisnik.Email).IsUnique();
            builder.Property(korisnik => korisnik.Password).IsRequired();


            //builder.HasMany(korisnik => korisnik.MojePorudzbine)
            //    .WithOne(porudzbina => porudzbina.Korisnik)
            //    .HasForeignKey(porudzbina => porudzbina.KorisnikId)
            //    .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
