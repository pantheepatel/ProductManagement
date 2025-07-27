namespace ProductManagement.Api.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    // Add other repositories as needed

    public UnitOfWork(TContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (!_repositories.ContainsKey(typeof(TEntity)))
        {
            _repositories[typeof(TEntity)] = new Repository<TEntity>(_context);
        }
        return (IRepository<TEntity>)_repositories[typeof(TEntity)];
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}