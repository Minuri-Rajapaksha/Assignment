using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shared.Model.DB.Application;

namespace Data.DbFactory.Application
{
    public partial class AdraContext : DbContext
    {
        internal AdraContext(DbConnection dbConnection)
            : base(new DbContextOptionsBuilder<AdraContext>().UseSqlServer(dbConnection).Options) { }

        public AdraContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountBalance> AccountBalance { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<User> User { get; set; }      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.AccountName)
                    .HasName("UQ__Account__406E0D2E8A842AA5")
                    .IsUnique();

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__Created__440B1D61");
            });

            modelBuilder.Entity<AccountBalance>(entity =>
            {
                entity.HasIndex(e => new { e.AccountId, e.PeriodId })
                    .HasName("UQ_AccountBalance")
                    .IsUnique();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountBalance)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountBa__Accou__4AB81AF0");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AccountBalance)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountBa__Creat__4CA06362");

                entity.HasOne(d => d.Period)
                    .WithMany(p => p.AccountBalance)
                    .HasForeignKey(d => d.PeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountBa__Perio__4BAC3F29");
            });

            modelBuilder.Entity<Period>(entity =>
            {
                entity.HasIndex(e => e.Discription)
                    .HasName("UQ__Period__AC417D3BC5290CAD")
                    .IsUnique();

                entity.Property(e => e.Discription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Period1)
                    .HasColumnName("Period")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Discription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasIndex(e => new { e.PermissionId, e.RoleId })
                    .HasName("UQ_RolePermission")
                    .IsUnique();

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RolePermi__Permi__3C69FB99");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RolePermi__RoleI__3D5E1FD2");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.Discription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__User__C9F28456A934F3EA")
                    .IsUnique();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
