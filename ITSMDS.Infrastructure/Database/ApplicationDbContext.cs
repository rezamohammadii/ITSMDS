using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITSMDS.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) 
    {
        
    }
    public ApplicationDbContext()
    {
        
    }
    #region Entities

    public DbSet<User> Users { get; set; }
    public DbSet<ServerEntity> Servers { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Port> Ports { get; set; }
    public DbSet<PortService> PortServices{ get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    #endregion
    #region TableConfig
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Schemas

        modelBuilder.Entity<ServerEntity>()
          .HasKey(e => e.Id);
        modelBuilder.Entity<ServerEntity>(entity =>
        {
            entity.Property(x => x.ServerName).HasMaxLength(150).HasColumnName("server_name");
            entity.Property(x => x.IpAddress).HasMaxLength(50).HasColumnName("ip_address");
            entity.Property(x => x.Location).HasMaxLength(150).HasColumnName("physical_location");
            entity.Property(x => x.RAM).HasColumnName("ram_size").HasComment("base on GB");
            entity.Property(x => x.StorageSize).HasColumnName("storage_size").HasComment("base on GB");
            entity.Property(x => x.CPU).HasMaxLength(50).HasColumnName("cpu_size").HasComment("base on GB");
            entity.Property(x => x.StorageType).HasColumnName("storage_type");
            entity.Property(x => x.Status).HasColumnName("status");
            entity.Property(x => x.StartDate).HasColumnName("start_date");
            entity.Property(x => x.MainBoardModel).HasMaxLength(50).HasColumnName("mainboard_model");
            entity.Property(x => x.OS).HasMaxLength(50).HasColumnName("os_name");
            entity.Property(x => x.DepartmentId).HasColumnName("department_id");
            entity.Property(x => x.IsDeleted).HasColumnName("is_de;eted").HasDefaultValue(false);
        });



        modelBuilder.Entity<User>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.PersonalCode).HasColumnName("personali_code");
            entity.Property(x => x.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
            entity.Property(x => x.IsActive).HasDefaultValue(true).HasColumnName("is_active");
            entity.Property(x => x.Email).HasMaxLength(255).HasColumnName("email_address");
            entity.Property(x => x.FirstName).HasMaxLength(155).HasColumnName("first_name");
            entity.Property(x => x.LastName).HasMaxLength(155).HasColumnName("last_name");
            entity.Property(x => x.TeamName).HasMaxLength(155).HasColumnName("team_name");
            entity.Property(x => x.Password).HasMaxLength(-1).HasColumnName("password");
            entity.Property(x => x.UserName).HasMaxLength(255).HasColumnName("user_name");
            entity.Property(x => x.HashId).HasMaxLength(255).HasDefaultValue(Guid.NewGuid().ToString()).HasColumnName("hash_Id");
            entity.Property(x => x.PhoneNumber).HasColumnName("phone_number");
            entity.Property(x => x.CreateDate).HasDefaultValue(DateTimeOffset.UtcNow).HasColumnName("create_date");
            entity.Property(x => x.HashId).HasMaxLength(255);
            entity.Property(x => x.ModifiedTime).HasDefaultValue(DateTimeOffset.UtcNow).HasColumnName("modidied_time");
            entity.HasIndex(x => new { x.IsActive, x.IsDeleted }).HasDatabaseName("IX_User_ActiveDeleted");

        });

        modelBuilder.Entity<ServiceEntity>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).HasColumnName("service_name");
            entity.Property(x => x.CriticalityScore).HasMaxLength(2).HasColumnName("criticality_score");
            entity.Property(x => x.Version).HasMaxLength(50).HasColumnName("version");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(x => x.OwnerName).HasMaxLength(100).HasColumnName("owner_name");
            entity.Property(x => x.Name).HasMaxLength(100).HasColumnName("service_name");
            entity.Property(x => x.LocalLocation).HasMaxLength(100).HasColumnName("local_location");
            entity.Property(x => x.DepartmentIdentifire).HasMaxLength(255).HasDefaultValue(Guid.NewGuid().ToString()).HasColumnName("department_id");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.Property(x => x.PortNumber).HasMaxLength(6).HasColumnName("port_number").IsRequired();
            entity.Property(x => x.RiskLevel).HasMaxLength(1).HasColumnName("risk_level");
            entity.Property(x => x.Description).HasMaxLength(255).HasColumnName("description");
            entity.Property(x => x.Protocol).HasMaxLength(10).HasColumnName("protocol");

        });


        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).HasColumnName("role_name");
            entity.Property(x => x.Description).HasMaxLength(-1).HasColumnName("descripption");
            entity.Property(x => x.CreateDate).HasDefaultValue(DateTime.UtcNow).HasColumnName("create_date");
            entity.Property(x => x.ModifiedTime).HasDefaultValue(DateTime.UtcNow).HasColumnName("modidied_time");


        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).HasColumnName("permission_name");
            entity.Property(x => x.Description).HasMaxLength(-1).HasColumnName("descripption");
            entity.Property(x => x.CreateDate).HasDefaultValue(DateTime.UtcNow).HasColumnName("create_date");
            entity.Property(x => x.ModifiedTime).HasDefaultValue(DateTime.UtcNow).HasColumnName("modidied_time");


        });

        #endregion

        #region TableRelation

        // Relation between Role and User table (many to many)

        modelBuilder.Entity<UserRole>()
       .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // Relation between Role and Permission table (many to many)

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);


        // Relation between Department and Server table (one to many)

        modelBuilder.Entity<Department>()
            .HasMany(s => s.Servers)
            .WithOne(x => x.Department)
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired(false);

        // Relation between Server and Service table (one to many)

        modelBuilder.Entity<ServerEntity>()
            .HasMany(x => x.Services)
            .WithOne(x => x.Server)
            .HasForeignKey(x => x.ServerId)
            .IsRequired();

        // Relation between Port and Service table (many to many)

        modelBuilder.Entity<PortService>()
      .HasKey(ur => new { ur.PortId, ur.ServiceId });

        modelBuilder.Entity<PortService>()
            .HasOne(ur => ur.Port)
            .WithMany(u => u.PortServices)
            .HasForeignKey(ur => ur.PortId);

        modelBuilder.Entity<PortService>()
            .HasOne(ur => ur.Service)
            .WithMany(r => r.PortServices)
            .HasForeignKey(ur => ur.ServiceId);

        #endregion
    }
    #endregion

}
