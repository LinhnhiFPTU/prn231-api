using System.Linq.Expressions;

namespace PRN231.Repo.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? pageIndex = null,
        int? pageSize = null);

    TEntity GetById(object id);

    void Insert(TEntity entity);

    void Delete(object id);

    void Delete(TEntity entityToDelete);

    void Update(TEntity entityToUpdate);
}