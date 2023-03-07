using Microsoft.EntityFrameworkCore;
using OrderRecordSystemAPI.Interfaces;
using OrderRecordSystemAPI.Models;

namespace OrderRecordSystemAPI
{
    public class AppDbContext : DbContext
    {
        private string _username = "Anonymous";

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ServiceOrderItem> ServiceOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        }

        public override int SaveChanges()
        {
            SaveChangesHelper();
            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync(string username)
        {
            _username = username;
            SaveChangesHelper();
            return base.SaveChangesAsync();
        }

        private void SaveChangesHelper()
        {
            var addedEntitiesMs = ChangeTracker.Entries().Where(a => a.State == EntityState.Added).AsEnumerable().Select(a => a.Entity).OfType<IRequiredProperties>().ToList();
            var modifiedEntitiesMs = ChangeTracker.Entries().Where(a => a.State == EntityState.Modified).AsEnumerable().Select(a => a.Entity).OfType<IRequiredProperties>().ToList();
            var deletedEntitiesMs = ChangeTracker.Entries().Where(a => a.State == EntityState.Deleted).AsEnumerable().Select(a => a.Entity).OfType<IRequiredProperties>().ToList();

            foreach (var item in addedEntitiesMs)
            {
                var now = DateTimeOffset.Now;
                item.CreatedAt = now;
                item.UpdatedAt = now;
                item.ModifiedBy = _username;
            }

            foreach (var item in modifiedEntitiesMs)
            {
                item.UpdatedAt = DateTimeOffset.Now;
                item.ModifiedBy = _username;
            }

            foreach (var item in deletedEntitiesMs)
            {
                item.UpdatedAt = DateTimeOffset.Now;
                item.ModifiedBy = _username;
            }
        }
    }
}
