using Microsoft.EntityFrameworkCore;
using System;
using Vaquinha.Domain.Entities;
using Vaquinha.Repository.Mapping;

namespace Vaquinha.Repository.Context
{
    public class VaquinhaOnlineDBContext : DbContext
    {
        public VaquinhaOnlineDBContext(DbContextOptions<VaquinhaOnlineDBContext> options)
            : base(options)
        { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<Donation> Doacoes { get; set; }
        public DbSet<Causa> Causas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaMapping());
            modelBuilder.ApplyConfiguration(new AddressMapping());
            modelBuilder.ApplyConfiguration(new DonationMapping());
            modelBuilder.ApplyConfiguration(new CausaMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}