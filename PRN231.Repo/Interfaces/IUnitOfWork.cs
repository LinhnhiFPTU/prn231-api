using PRN231.Repo.Models;

namespace PRN231.Repo.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Account> AccountRepository { get; }
    IGenericRepository<Role> RoleRepository { get; }
    IGenericRepository<Invoice> InvoiceRepository { get; }
    IGenericRepository<InvoiceDetail> InvoiceDetailRepository { get; }
    IGenericRepository<InvoiceTemplate> InvoiceTemplateRepository { get; }
    IGenericRepository<InventoryItem> InventoryItemRepository { get; }
    IGenericRepository<BrandPartnerMapping> BrandPartnerMappingRepository { get; }
    IGenericRepository<Brand> BrandRepository { get; }
    IGenericRepository<Partner> PartnerRepository { get; }
    IGenericRepository<Store> StoreRepository { get; }
    void Save();

    Task SaveAsync();
}