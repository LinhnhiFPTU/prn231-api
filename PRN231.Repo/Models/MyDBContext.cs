using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231.Repo.Models
{
    public partial class MyDBContext : DbContext
    {
        public MyDBContext()
        {
        }

        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        {
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

                entity.Property(e => e.TemplateName).HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.InvoiceTemplates)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__InvoiceTe__Brand__3F466844");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner");

                entity.Property(e => e.ApiUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
