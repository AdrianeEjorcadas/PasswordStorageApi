﻿using Microsoft.EntityFrameworkCore;
using UserManagementApi.Models;

namespace UserManagementApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserCredentialModel> Users { get; set; }
        public DbSet<UserPasswordResetModel> UserPasswordResets { get; set; }
        public DbSet<AuthenticationTokenModel> AuthenticationTokens { get; set; }
        public DbSet<RolesModel> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserCredentialModel>().HasQueryFilter(e => !e.IsDeleted && e.IsEmailConfirmed);
            modelBuilder.Entity<UserPasswordResetModel>().HasQueryFilter(e => !e.IsUsed);
            modelBuilder.Entity<AuthenticationTokenModel>().HasQueryFilter(e => !e.IsRevoked);

            // make table fields unique
            modelBuilder.Entity<RolesModel>().HasIndex(e => e.RoleName).IsUnique();

            // composite fields
            modelBuilder.Entity<UserRolesModel>()
                .HasKey(e => new { e.RoleId, e.UserId });

            // foreign key fields
            //modelBuilder.Entity<UserRolesModel>()
            //    .HasOne(u => u.User)
            //    .WithMany(r => r.Role)
        }

        public override int SaveChanges()
        {
            CreatedAt();
            UpdatedAt();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreatedAt();
            UpdatedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        // Set initial value of properties upon creation of record
        private void CreatedAt()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            DateTime currentTime = DateTime.UtcNow;

            foreach (var entry in entries) 
            {
                if(entry.Entity is  UserCredentialModel user)
                {
                    user.Id = Guid.NewGuid();
                    user.IsActive = true;
                    user.IsDeleted = false;
                    user.IsEmailConfirmed = false;
                    user.ConfirmationTokenExpiration = currentTime.AddDays(1);
                    user.CreatedAt = currentTime;
                }
                else if(entry.Entity is UserPasswordResetModel passwordReset)
                {
                    passwordReset.TokenId = Guid.NewGuid();
                    passwordReset.ExpirationDateTime = currentTime.AddMinutes(10);
                }
                else if(entry.Entity is AuthenticationTokenModel authenticationToken)
                {
                    authenticationToken.Id = Guid.NewGuid();
                    authenticationToken.CreatedAt = currentTime;
                    authenticationToken.AuthTokenExpiration = currentTime.AddMinutes(30);
                    authenticationToken.RefreshTokenExpiration = currentTime.AddDays(3);
                }
            }
        }

        // Set value of properties of model upon updating of record
        private void UpdatedAt()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            DateTime currentTime = DateTime.UtcNow;

            foreach(var entry in entries)
            {
                if (entry.Entity is UserCredentialModel user)
                {
                    user.UpdatedAt = currentTime;
                }
                else if (entry.Entity is UserPasswordResetModel userPasswordReset) 
                {
                    userPasswordReset.IsUsed = true;
                }
            }
        }

    }
}
