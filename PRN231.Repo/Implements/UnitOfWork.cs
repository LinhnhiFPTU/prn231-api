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