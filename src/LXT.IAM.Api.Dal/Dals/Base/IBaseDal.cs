using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Storage.Entity.Base;

namespace LXT.IAM.Api.Dal.Dals.Base;

public interface IBaseDal<T> : IScopedDependency where T : BaseEntity
{
    Task<T?> GetByIdAsync(long id);
    Task<int> AddAsync(T entity);
    Task<int> AddRangeAsync(IEnumerable<T> entities);
    Task<int> UpdateAsync(T entity);
    Task<int> UpdateRangeAsync(IEnumerable<T> entities);
    Task<int> RemoveAsync(T entity);
    Task<int> RemoveRangeAsync(IEnumerable<T> entities);
}
