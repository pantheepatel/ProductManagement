using ProductManagement.Api.Repository;

namespace ProductManagement.Api.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<TRepository> GetRepository<TRepository>() where TRepository : class;
    Task<int> CompleteAsync();
}