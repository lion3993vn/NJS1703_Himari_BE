using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD392_Himari.Repository.Entities;

namespace SWD392_Himari.Repository.Data
{
    public class SWD392HimariDbContext : DbContext
    {
        public SWD392HimariDbContext(DbContextOptions<SWD392HimariDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PartSympton> PartSymptons { get; set; }
        public DbSet<CustomerConsulting> CustomerConsultings { get; set; }
        public DbSet<ConsultingResult> ConsultingResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ConsultingResults -> CustomerConsultings
            modelBuilder.Entity<ConsultingResult>()
                .HasOne(cr => cr.CustomerConsulting)
                .WithMany()
                .HasForeignKey(cr => cr.CustomerConsultingId)
                .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa cascade

            // ConsultingResults -> Products
            modelBuilder.Entity<ConsultingResult>()
                .HasOne(cr => cr.Product)
                .WithMany()
                .HasForeignKey(cr => cr.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa cascade
        }

    }


}