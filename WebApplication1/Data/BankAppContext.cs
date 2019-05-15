using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISTP_LABA_3.Models;

namespace ISTP_LABA_3.Data
{
    public class BankAppContext : DbContext
    {
        public BankAppContext(DbContextOptions<BankAppContext> options): base(options)
        {

        }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>().ToTable("Contract");
            modelBuilder.Entity<Condition>().ToTable("Condition");
            modelBuilder.Entity<OfferType>().ToTable("OfferType");
            modelBuilder.Entity<Offer>().ToTable("Offer");
            modelBuilder.Entity<Bank>().ToTable("Bank");
            modelBuilder.Entity<Client>().ToTable("Client");
        }
    }
}
