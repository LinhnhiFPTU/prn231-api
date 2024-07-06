namespace PRN231.Repo.Implements;

public class PaginationResponse<TEntity> where TEntity : class
{
    public IEnumerable<TEntity> Entities { get; set; } = new List<TEntity>();
    public int TotalPage { get; set; }
}