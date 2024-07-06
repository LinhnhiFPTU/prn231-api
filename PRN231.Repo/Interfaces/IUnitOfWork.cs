using PRN231.Repo.Models;

namespace PRN231.Repo.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Account> AccountRepository { get; }
    IGenericRepository<Role> RoleRepository { get; }
    void Save();

    Task SaveAsync();
}