using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Models;

namespace PasswordStorageApi.Data
{
    public class PasswordStorageDbContext : DbContext
    {
        public PasswordStorageDbContext(DbContextOptions<PasswordStorageDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<PasswordModel> Passwords { get; set; }
        public DbSet<PlatformModel> Platforms { get; set; }
        public DbSet<AuditLogModel> AuditLogs { get; set; }

        public override int SaveChanges()
        {
            CreatedAt();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreatedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void CreatedAt()
        {
            var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);

            // Get the current UTC time
            DateTime currentUtcTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is PlatformModel platform)
                {
                    platform.CreatedAt = currentUtcTime;
                }
                else if (entry.Entity is PasswordModel password)
                {
                    password.CreatedAt = currentUtcTime;
                }
                else if (entry.Entity is UserModel user)
                {
                    user.CreatedAt = currentUtcTime;
                }
                else if(entry.Entity is AuditLogModel auditLog)
                {
                    auditLog.CreatedAt = currentUtcTime;
                    auditLog.UnixTimeStamp = ((DateTimeOffset)currentUtcTime).ToUnixTimeSeconds();
                }
            }
        }

    }
}
