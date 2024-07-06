using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    internal MyDBContext Context;
    internal DbSet<TEntity> DbSet;

    public GenericRepository(MyDBContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual void Delete(object id)
    {
        var entityToDelete = DbSet.Find(id);
        Delete(entityToDelete);
    }

    public virtual TEntity GetById(object id)
    {
        return DbSet.Find(id);
    }

    public virtual void Insert(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached) DbSet.Attach(entityToDelete);
        DbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public void Reset()
    {
        // Reset to the original data
        Context.Dispose();
    }

    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? pageIndex = null,
        int? pageSize = null
    )
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null) query = orderBy(query);

        if (pageIndex.HasValue && pageSize.HasValue)
        {
            var validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
            var validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

            query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
        }

        return query.ToList();
    }
}