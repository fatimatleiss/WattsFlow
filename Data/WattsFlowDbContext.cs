
using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Models;

namespace WattsFlowProject.Data
{
    public class WattsFlowDbContext : DbContext
    {
        public WattsFlowDbContext(DbContextOptions<WattsFlowDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerTraffic> Traffics { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<ExpenseType> Types { get; set; }
        public virtual DbSet<PostDetails> Details { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
        public virtual DbSet<CustomerTrafficHistory> TrafficHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.PostDetails) 
                .WithMany(p => p.Customers) 
                .HasForeignKey(c => c.PostId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<CustomerTraffic>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Traffics) 
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<CustomerTrafficHistory>()
                .HasOne(t => t.Customer) 
                .WithMany(c => c.TrafficHistories) 
                .HasForeignKey(t => t.CustomerId) 
                .OnDelete(DeleteBehavior.Cascade); 

            base.OnModelCreating(modelBuilder);
        }


    }
}