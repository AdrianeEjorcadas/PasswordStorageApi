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
    }
}
