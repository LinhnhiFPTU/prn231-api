using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace PRN231.Repo.Models;

public partial class MyDBContext : DbContext
{
    public MyDBContext()
    {
    }

    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
        var dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if (dbCreater != null)
        {
            if (!dbCreater.CanConnect()) dbCreater.Create();

            if (!dbCreater.HasTables()) dbCreater.CreateTables();
        }
    }

    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<Brand> Brands { get; set; } = null!;
    public virtual DbSet<BrandPartnerMapping> BrandPartnerMappings { get; set; } = null!;
    public virtual DbSet<InventoryItem> InventoryItems { get; set; } = null!;
    public virtual DbSet<Invoice> Invoices { get; set; } = null!;
    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
    public virtual DbSet<InvoiceTemplate> InvoiceTemplates { get; set; } = null!;
    public virtual DbSet<Partner> Partners { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Store> Stores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Account__BrandId__52593CB8");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__RoleId__5165187F");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.TaxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BrandPartnerMapping>(entity =>
        {
            entity.ToTable("BrandPartnerMapping");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.BrandPartnerMappings)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__BrandPart__Brand__4BAC3F29");

            entity.HasOne(d => d.Partner)
                .WithMany(p => p.BrandPartnerMappings)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__BrandPart__Partn__4CA06362");
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.ToTable("InventoryItem");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.UnitName).HasMaxLength(20);

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.InventoryItems)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Inventory__Brand__3C69FB99");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 6)");

            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.TotalAmountWithoutVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalAmountWithoutVAT");

            entity.Property(e => e.TotalDiscountAmount).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.TotalSaleAmount).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.TotalVatamount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalVATAmount");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.InvoiceTemplate)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.InvoiceTemplateId)
                .HasConstraintName("FK__Invoice__Invoice__4316F928");

            entity.HasOne(d => d.Store)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Invoice__StoreId__4222D4EF");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Vatamount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATAmount");

            entity.HasOne(d => d.InventoryItem)
                .WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InventoryItemId)
                .HasConstraintName("FK__InvoiceDe__Inven__46E78A0C");

            entity.HasOne(d => d.Invoice)
                .WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__InvoiceDe__Invoi__45F365D3");
        });

        modelBuilder.Entity<InvoiceTemplate>(entity =>
        {
            entity.ToTable("InvoiceTemplate");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.TemplateName).HasMaxLength(100);

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.InvoiceTemplates)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__InvoiceTe__Brand__3F466844");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.ToTable("Partner");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.ApiUrl)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Address).HasMaxLength(200);

            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.ShortName).HasMaxLength(50);

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.Stores)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Store__BrandId__398D8EEE");
        });

        modelBuilder.Entity<Brand>().HasData(
            new Brand
            {
                Id = 1, Name = "Cafe Express", Code = "CE001", Status = 1, TaxCode = "0123456789",
                Description = "Chuỗi cà phê nhanh chất lượng cao"
            },
            new Brand
            {
                Id = 2, Name = "Tea House", Code = "TH001", Status = 1, TaxCode = "9876543210",
                Description = "Chuỗi trà truyền thống"
            }
        );

        modelBuilder.Entity<Store>().HasData(
            new Store
            {
                Id = 1, Name = "Cafe Express - Lê Lợi", ShortName = "CE-LL", Email = "leloi@cafeexpress.com",
                Phone = "0901234567", Code = "ST001", Status = 1, BrandId = 1, Address = "123 Lê Lợi, Q.1, TP.HCM"
            },
            new Store
            {
                Id = 2, Name = "Tea House - Nguyễn Huệ", ShortName = "TH-NH", Email = "nguyenhue@teahouse.com",
                Phone = "0909876543", Code = "ST002", Status = 1, BrandId = 2, Address = "456 Nguyễn Huệ, Q.1, TP.HCM"
            }
        );

        modelBuilder.Entity<InventoryItem>().HasData(
            new InventoryItem
            {
                Id = 1, Name = "Cà phê đen", Code = "CF001", UnitName = "Ly", UnitPrice = 25000, Vatrate = 10,
                Status = 1, BrandId = 1
            },
            new InventoryItem
            {
                Id = 2, Name = "Cà phê sữa", Code = "CF002", UnitName = "Ly", UnitPrice = 30000, Vatrate = 10,
                Status = 1, BrandId = 1
            },
            new InventoryItem
            {
                Id = 3, Name = "Bánh mì", Code = "BM001", UnitName = "Cái", UnitPrice = 15000, Vatrate = 5, Status = 1,
                BrandId = 1
            },
            new InventoryItem
            {
                Id = 4, Name = "Trà sen", Code = "TS001", UnitName = "Ly", UnitPrice = 28000, Vatrate = 10, Status = 1,
                BrandId = 2
            },
            new InventoryItem
            {
                Id = 5, Name = "Trà sữa", Code = "TS002", UnitName = "Ly", UnitPrice = 32000, Vatrate = 10, Status = 1,
                BrandId = 2
            },
            new InventoryItem
            {
                Id = 6, Name = "Bánh ngọt", Code = "BN001", UnitName = "Cái", UnitPrice = 20000, Vatrate = 5,
                Status = 1, BrandId = 2
            }
        );

        modelBuilder.Entity<InvoiceTemplate>().HasData(
            new InvoiceTemplate
            {
                Id = 1, BrandId = 1, TemplateName = "Mẫu hóa đơn chuẩn Cafe Express", TemplateType = 1, InvoiceType = 1
            },
            new InvoiceTemplate
            {
                Id = 2, BrandId = 1, TemplateName = "Mẫu hóa đơn khuyến mãi Cafe Express", TemplateType = 2,
                InvoiceType = 2
            },
            new InvoiceTemplate
            {
                Id = 3, BrandId = 2, TemplateName = "Mẫu hóa đơn chuẩn Tea House", TemplateType = 1, InvoiceType = 1
            },
            new InvoiceTemplate
            {
                Id = 4, BrandId = 2, TemplateName = "Mẫu hóa đơn đặc biệt Tea House", TemplateType = 3, InvoiceType = 3
            }
        );

        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                Id = 1, InvoiceCode = "INV001", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Type = 1,
                Status = 2, PaymentMethod = 1, StoreId = 1, InvoiceTemplateId = 1, TotalSaleAmount = 65000,
                TotalDiscountAmount = 0, TotalAmountWithoutVat = 59090.91M, TotalVatamount = 5909.09M,
                TotalAmount = 65000
            },
            new Invoice
            {
                Id = 2, InvoiceCode = "INV002", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Type = 1,
                Status = 2, PaymentMethod = 1, StoreId = 2, InvoiceTemplateId = 3, TotalSaleAmount = 88000,
                TotalDiscountAmount = 0, TotalAmountWithoutVat = 80000.00M, TotalVatamount = 8000.00M,
                TotalAmount = 88000
            },
            new Invoice
            {
                Id = 3, InvoiceCode = "INV003", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Type = 2,
                Status = 2, PaymentMethod = 1, StoreId = 1, InvoiceTemplateId = 2, TotalSaleAmount = 80000,
                TotalDiscountAmount = 8000, TotalAmountWithoutVat = 65454.55M, TotalVatamount = 6545.45M,
                TotalAmount = 72000
            }
        );

        modelBuilder.Entity<InvoiceDetail>().HasData(
            new InvoiceDetail
            {
                Id = 1, InvoiceId = 1, InventoryItemId = 1, Quantity = 2, UnitPrice = 25000, Amount = 50000,
                DiscountAmount = 0, Vatamount = 5000, SortOrder = 1, IsPromotion = false, IsMemo = false
            },
            new InvoiceDetail
            {
                Id = 2, InvoiceId = 1, InventoryItemId = 3, Quantity = 1, UnitPrice = 15000, Amount = 15000,
                DiscountAmount = 0, Vatamount = 909.09M, SortOrder = 2, IsPromotion = false, IsMemo = false
            },
            new InvoiceDetail
            {
                Id = 3, InvoiceId = 2, InventoryItemId = 4, Quantity = 2, UnitPrice = 28000, Amount = 56000,
                DiscountAmount = 0, Vatamount = 5600, SortOrder = 1, IsPromotion = false, IsMemo = false
            },
            new InvoiceDetail
            {
                Id = 4, InvoiceId = 2, InventoryItemId = 5, Quantity = 1, UnitPrice = 32000, Amount = 32000,
                DiscountAmount = 0, Vatamount = 3200, SortOrder = 2, IsPromotion = false, IsMemo = false
            },
            new InvoiceDetail
            {
                Id = 5, InvoiceId = 3, InventoryItemId = 1, Quantity = 2, UnitPrice = 25000, Amount = 50000,
                DiscountAmount = 5000, Vatamount = 4500, SortOrder = 1, IsPromotion = true, IsMemo = false
            },
            new InvoiceDetail
            {
                Id = 6, InvoiceId = 3, InventoryItemId = 2, Quantity = 1, UnitPrice = 30000, Amount = 30000,
                DiscountAmount = 3000, Vatamount = 2700, SortOrder = 2, IsPromotion = true, IsMemo = false
            }
        );

        modelBuilder.Entity<Partner>().HasData(
            new Partner
            {
                Id = 1, Name = "Nhà cung cấp cà phê A", Description = "Cung cấp hạt cà phê chất lượng cao",
                ApiUrl = "https://api.nhacungcapa.com",
                Type = 1, Environment = 1, Status = 1
            },
            new Partner
            {
                Id = 2, Name = "Nhà cung cấp trà B", Description = "Cung cấp lá trà thượng hạng",
                ApiUrl = "https://api.nhacungcapb.com",
                Type = 2, Environment = 1, Status = 1
            }
        );

        modelBuilder.Entity<BrandPartnerMapping>().HasData(
            new BrandPartnerMapping
            {
                Id = 1, BrandId = 1, PartnerId = 1, Status = 1, Config = "{\"discountRate\": 5, \"paymentTerm\": 30}"
            },
            new BrandPartnerMapping
            {
                Id = 2, BrandId = 2, PartnerId = 2, Status = 1, Config = "{\"discountRate\": 3, \"paymentTerm\": 45}"
            }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "ADMIN" },
            new Role { Id = 2, Name = "STORE_MANAGER" },
            new Role { Id = 3, Name = "CASHIER" }
        );

        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1, Name = "Admin User", Username = "admin",
                Password = "73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=", Status = 1, RoleId = 1,
                BrandId = 1
            },
            new Account
            {
                Id = 2, Name = "Manager User", Username = "manager",
                Password = "73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=", Status = 1,
                RoleId = 2, BrandId = 2
            },
            new Account
            {
                Id = 3, Name = "Cashier User", Username = "cashier",
                Password = "73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=", Status = 1,
                RoleId = 3, BrandId = 1
            }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}