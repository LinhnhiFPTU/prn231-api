using System.ComponentModel.DataAnnotations;
using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;

namespace PRN231.Repo.Implements;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly MyDBContext context;

    private IGenericRepository<Account> accountRepository;

    private bool disposed;
    private IGenericRepository<Role> roleRepository;
    private IGenericRepository<Brand> brandRepository;
    private IGenericRepository<BrandPartnerMapping> brandPartnerMappingRepository;
    private IGenericRepository<InventoryItem> inventoryItemRepository;
    private IGenericRepository<Invoice> invoiceRepository;
    private IGenericRepository<InvoiceDetail> invoiceDetailRepository;
    private IGenericRepository<InvoiceTemplate> invoiceTemplateRepository;
    private IGenericRepository<Partner> partnerRepository;
    private IGenericRepository<Store> storeRepository;


    public UnitOfWork(MyDBContext context)
    {
        this.context = context;
    }

    public IGenericRepository<Account> AccountRepository
    {
        get { return accountRepository ??= new GenericRepository<Account>(context); }
    }

    public IGenericRepository<Role> RoleRepository
    {
        get { return roleRepository ??= new GenericRepository<Role>(context); }
    }

    public IGenericRepository<Brand> BrandRepository
    {
        get { return brandRepository ??= new GenericRepository<Brand>(context); }
    }

    public IGenericRepository<BrandPartnerMapping> BrandPartnerMappingRepository
    {
        get { return brandPartnerMappingRepository ??= new GenericRepository<BrandPartnerMapping>(context); }
    }

    public IGenericRepository<InventoryItem> InventoryItemRepository
    {
        get { return inventoryItemRepository ??= new GenericRepository<InventoryItem>(context); }
    }

    public IGenericRepository<Invoice> InvoiceRepository
    {
        get { return invoiceRepository ??= new GenericRepository<Invoice>(context); }
    }

    public IGenericRepository<InvoiceDetail> InvoiceDetailRepository
    {
        get { return invoiceDetailRepository ??= new GenericRepository<InvoiceDetail>(context); }
    }

    public IGenericRepository<InvoiceTemplate> InvoiceTemplateRepository
    {
        get { return invoiceTemplateRepository ??= new GenericRepository<InvoiceTemplate>(context); }
    }

    public IGenericRepository<Partner> PartnerRepository
    {
        get { return partnerRepository ??= new GenericRepository<Partner>(context); }
    }

    public IGenericRepository<Store> StoreRepository
    {
        get { return storeRepository ??= new GenericRepository<Store>(context); }
    }

    public void Save()
    {
        var validationErrors = context.ChangeTracker.Entries<IValidatableObject>()
            .SelectMany(e => e.Entity.Validate(null))
            .Where(e => e != ValidationResult.Success)
            .ToArray();
        if (validationErrors.Any())
        {
            var exceptionMessage = string.Join(Environment.NewLine,
                validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
            throw new Exception(exceptionMessage);
        }

        context.SaveChanges();
    }

    public async Task SaveAsync()
    {
        var validationErrors = context.ChangeTracker.Entries<IValidatableObject>()
            .SelectMany(e => e.Entity.Validate(null))
            .Where(e => e != ValidationResult.Success)
            .ToArray();
        if (validationErrors.Any())
        {
            var exceptionMessage = string.Join(Environment.NewLine,
                validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
            throw new Exception(exceptionMessage);
        }

        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing) context.Dispose();
        disposed = true;
    }
}