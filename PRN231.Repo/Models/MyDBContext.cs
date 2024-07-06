using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace PRN231.Repo.Models;

public partial class MyDBContext : DbContext
{
    private readonly Guid _adminRoleId;
    private readonly Guid _customerRoleId;
    private readonly Guid _staffRoleId;

    public MyDBContext()
    {
    }

    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
        _adminRoleId = Guid.NewGuid();
        _staffRoleId = Guid.NewGuid();
        _customerRoleId = Guid.NewGuid();

        var dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if (dbCreater != null)
        {
            if (!dbCreater.CanConnect()) dbCreater.Create();

            if (!dbCreater.HasTables()) dbCreater.CreateTables();
        }
    }

    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;

    private Account[] GetSampleAccounts()
    {
        var sha256 = SHA256.Create();
        return new Account[]
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "The Admin",
                Username = "admin",
                Password = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("12345678"))),
                RoleId = _adminRoleId,
                Status = "ACTIVE"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "The Staff",
                Username = "staff",
                Password = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("12345678"))),
                RoleId = _staffRoleId,
                Status = "ACTIVE"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "The Customer",
                Username = "customer",
                Password = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("12345678"))),
                RoleId = _customerRoleId,
                Status = "ACTIVE"
            }
        };
    }

    private Role[] GetSampleRoles()
    {
        return new Role[]
        {
            new()
            {
                Id = _adminRoleId,
                Name = "ADMIN",
                Status = "ACTIVE"
            },
            new()
            {
                Id = _staffRoleId,
                Name = "STAFF",
                Status = "ACTIVE"
            },
            new()
            {
                Id = _customerRoleId,
                Name = "CUSTOMER",
                Status = "ACTIVE"
            }
        };
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E40D15494F")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Password).HasMaxLength(255);

            entity.Property(e => e.Status).HasMaxLength(50);

            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>().HasData(
            GetSampleRoles()
        );

        modelBuilder.Entity<Account>().HasData(
            GetSampleAccounts()
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}