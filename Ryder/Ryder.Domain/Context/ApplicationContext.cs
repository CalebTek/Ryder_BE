using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Common;
using Ryder.Domain.Entities;

namespace Ryder.Domain.Context
{
    public class ApplicationContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageThread> MessageThreads { get; set; }
        public DbSet<MessageThreadParticipant> MessageThreadParticipants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<RequestStatus>RequestStatuses { get; set; }

        public Task GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<BaseEntity>())
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        item.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        item.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), (type) =>
            {
                return !string.IsNullOrEmpty(type.Namespace) && !type.IsInterface && !type.IsAbstract
                       && type.BaseType != null && typeof(IEntityTypeConfiguration<>).IsAssignableFrom(type);
            });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(double) || p.ClrType == typeof(double?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            modelBuilder.Entity<Card>().HasIndex(x => x.AppUserId);
            modelBuilder.Entity<Message>().HasIndex(x => x.MessageThreadId);
            modelBuilder.Entity<Message>().HasIndex(x => x.SenderId);
            modelBuilder.Entity<MessageThread>().HasIndex(x => x.PinnedMessageId);
            modelBuilder.Entity<MessageThread>().HasIndex(x => x.LastMessageId);
            modelBuilder.Entity<MessageThreadParticipant>().HasOne(x => x.MessageThread);
            modelBuilder.Entity<MessageThreadParticipant>().HasIndex(x => x.AppUserId);
            modelBuilder.Entity<Order>().HasOne(x => x.PickUpLocation);
            modelBuilder.Entity<Order>().HasOne(x => x.DropOffLocation);

            modelBuilder.Entity<Order>().HasIndex(x => x.RiderId);
            modelBuilder.Entity<Payment>().HasIndex(x => x.OrderId);
            modelBuilder.Entity<Rider>().HasIndex(x => x.AppUserId);
            modelBuilder.Entity<RequestStatus>().HasIndex(x => x.RiderId);
            modelBuilder.Entity<RequestStatus>().HasIndex(x => x.OrderId);

            base.OnModelCreating(modelBuilder);
        }
    }
}