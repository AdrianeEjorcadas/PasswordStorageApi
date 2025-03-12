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

        //Global filter for deleted item
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PlatformModel>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PasswordModel>().HasQueryFilter(e => !e.IsDeleted);
            //modelBuilder.Entity<AuditLogModel>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Override SaveChanges to call CreatedAt(), UpdatedAt(), and DeletedAt() method
        public override int SaveChanges()
        {
            CreatedAt();
            UpdatedAt();
            //DeletedAt();
            return base.SaveChanges();
        }

        // Override SaveChangesAsync to call CreatedAt(), UpdatedAt()
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreatedAt();
            UpdatedAt();
            //DeletedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        // Set the CreatedAt property for the entities
        private void CreatedAt()
        {
            // Get the entities that are being added
            var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);

            // Get the current UTC time
            DateTime currentUtcTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is PlatformModel platform)
                {
                    platform.CreatedAt = currentUtcTime;
                    platform.IsDeleted = false;
                }
                else if (entry.Entity is PasswordModel password)
                {
                    password.CreatedAt = currentUtcTime;
                    password.IsDeleted = false;
                    password.IsExpired = false;
                }
                else if (entry.Entity is UserModel user)
                {
                    user.CreatedAt = currentUtcTime;
                    user.IsDeleted = false;
                    user.IsActive = true;
                }
                //else if(entry.Entity is AuditLogModel auditLog)
                //{
                //    auditLog.CreatedAt = currentUtcTime;
                //    //auditLog.TimeStamp = ((DateTimeOffset)currentUtcTime).ToUnixTimeSeconds();
                //    //auditLog.IsDeleted = false;
                //}
            }
        }

        //Set the UpdatedAt property for the entities
        public void UpdatedAt()
        {
            // Get the entities that are being modified
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            // Get the current UTC time
            DateTime currentUtcTime = DateTime.UtcNow;

           foreach(var entry in entries)
            {
                if (entry.Entity is UserModel user)
                {
                    user.UpdatedAt = currentUtcTime;
                }
                else if(entry.Entity is PlatformModel platform)
                {
                    platform.UpdatedAt = currentUtcTime;
                }
                else if(entry.Entity is PasswordModel password)
                {
                    password.UpdatedAt = currentUtcTime;
                }
            }
        }

        // Set the DeletedAt property for the entities
        //public void DeletedAt()
        //{
        //    var entries = ChangeTracker.Entries()
        //        .Where(e => e.State == EntityState.Deleted);

        //    // Get the current UTC time
        //    DateTime currentUtcTime = DateTime.UtcNow;

        //    foreach(var entry in entries)
        //    {
        //        entry.State = EntityState.Modified;

        //        if(entry.Entity is UserModel user)
        //        {
        //            //user.IsDeleted = true;
        //            user.DeletedAt = currentUtcTime;
        //        }
        //        else if (entry.Entity is PlatformModel platform)
        //        {
        //            //platform.IsDeleted = true;
        //            platform.DeletedAt = currentUtcTime;
        //        }
        //        else if(entry.Entity is PasswordModel password)
        //        {
        //            //password.IsDeleted = true;
        //            password.DeletedAt = currentUtcTime;
        //        } 
        //        else if(entry.Entity is AuditLogModel auditLog)
        //        {
        //            //auditLog.IsDeleted = true;
        //            auditLog.DeletedAt = currentUtcTime;
        //        }
        //    }
        //}

    }
}
