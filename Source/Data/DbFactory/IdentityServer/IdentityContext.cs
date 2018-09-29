
using Microsoft.EntityFrameworkCore;
using Shared.Model.DB.Application;
using Shared.Model.DB.Identity;
using System.Data.Common;

namespace Data.DbFactory.IdentityServer
{
    public class IdentityContext : DbContext
    {
        internal IdentityContext(DbConnection dbConnection)
            : base(new DbContextOptionsBuilder<IdentityContext>().UseSqlServer(dbConnection).Options) { }

        public IdentityContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ApiClaims> ApiClaims { get; set; }
        public virtual DbSet<ApiResources> ApiResources { get; set; }
        public virtual DbSet<ApiScopeClaims> ApiScopeClaims { get; set; }
        public virtual DbSet<ApiScopes> ApiScopes { get; set; }
        public virtual DbSet<ApiSecrets> ApiSecrets { get; set; }
        public virtual DbSet<ClientClaims> ClientClaims { get; set; }
        public virtual DbSet<ClientCorsOrigins> ClientCorsOrigins { get; set; }
        public virtual DbSet<ClientGrantTypes> ClientGrantTypes { get; set; }
        public virtual DbSet<ClientIdPrestrictions> ClientIdPrestrictions { get; set; }
        public virtual DbSet<ClientPostLogoutRedirectUris> ClientPostLogoutRedirectUris { get; set; }
        public virtual DbSet<ClientProperties> ClientProperties { get; set; }
        public virtual DbSet<ClientRedirectUris> ClientRedirectUris { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<ClientScopes> ClientScopes { get; set; }
        public virtual DbSet<ClientSecrets> ClientSecrets { get; set; }
        public virtual DbSet<IdentityClaims> IdentityClaims { get; set; }
        public virtual DbSet<IdentityResources> IdentityResources { get; set; }
        public virtual DbSet<PersistedGrants> PersistedGrants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiClaims>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiClaims)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiResources>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ApiScopeClaims>(entity =>
            {
                entity.HasIndex(e => e.ApiScopeId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiScope)
                    .WithMany(p => p.ApiScopeClaims)
                    .HasForeignKey(d => d.ApiScopeId);
            });

            modelBuilder.Entity<ApiScopes>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiScopes)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiSecrets>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Type).HasMaxLength(250);

                entity.Property(e => e.Value).HasMaxLength(2000);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiSecrets)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ClientClaims>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientClaims)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientCorsOrigins>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCorsOrigins)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientGrantTypes>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.GrantType)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientGrantTypes)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientIdPrestrictions>(entity =>
            {
                entity.ToTable("ClientIdPRestrictions");

                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientIdPrestrictions)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUris>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.PostLogoutRedirectUri)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientPostLogoutRedirectUris)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientProperties>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientProperties)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientRedirectUris>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.RedirectUri)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientRedirectUris)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .IsUnique();

                entity.Property(e => e.BackChannelLogoutUri).HasMaxLength(2000);

                entity.Property(e => e.ClientClaimsPrefix).HasMaxLength(200);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.Property(e => e.ClientUri).HasMaxLength(2000);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FrontChannelLogoutUri).HasMaxLength(2000);

                entity.Property(e => e.LogoUri).HasMaxLength(2000);

                entity.Property(e => e.PairWiseSubjectSalt).HasMaxLength(200);

                entity.Property(e => e.ProtocolType)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ClientScopes>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Scope)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientScopes)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientSecrets>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Type).HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientSecrets)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<IdentityClaims>(entity =>
            {
                entity.HasIndex(e => e.IdentityResourceId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.IdentityResource)
                    .WithMany(p => p.IdentityClaims)
                    .HasForeignKey(d => d.IdentityResourceId);
            });

            modelBuilder.Entity<IdentityResources>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<PersistedGrants>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.HasIndex(e => new { e.SubjectId, e.ClientId, e.Type });

                entity.Property(e => e.Key)
                    .HasMaxLength(200)
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.SubjectId).HasMaxLength(200);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
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
