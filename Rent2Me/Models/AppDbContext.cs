using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Rent2Me.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<CarDetails> CarDetails { get; set; }
        public DbSet<UserFeedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationId);
            modelBuilder.Entity<Notification>().Property(n => n.CreatedAt).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Customer>()
            .HasOne(u => u.SubscriptionPlan)
            .WithMany(p => p.Customers)
            .HasForeignKey(u => u.SubscriptionPlanName);

            modelBuilder.Entity<CarDetails>()
            .HasOne(c => c.Customer)
            .WithMany(c => c.Cars)
            .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<RentalRequest>()
            .HasOne(c => c.Customer)
            .WithMany(c => c.Requests)
            .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<UserFeedback>()
                .HasKey(uf => uf.FeedbackId);

            modelBuilder.Entity<UserFeedback>()
                .HasOne(uf => uf.FromUser)
                .WithMany(u => u.GivenFeedbacks)
                .HasForeignKey(uf => uf.FromUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<UserFeedback>()
                .HasOne(uf => uf.ToUser)
                .WithMany(u => u.ReceivedFeedbacks)
                .HasForeignKey(uf => uf.ToUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Customer)
                .WithMany(c => c.Notifications)
                .HasForeignKey(n => n.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Notifications)
                .WithOne(n => n.Customer)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Payment>()
            .HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.SubscriptionPlan)
                .WithMany()
                .HasForeignKey(p => p.SubscriptionPlanName);
        }


    }
}
