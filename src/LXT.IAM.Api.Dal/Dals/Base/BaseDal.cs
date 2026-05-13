using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Dal.Dals.Base;

public class BaseDal<T> : IBaseDal<T> where T : BaseEntity
{
    protected IAMDbContext _dbContext;

    public BaseDal(IAMDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        return await _dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<int> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity).ConfigureAwait(false);
        return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities).ConfigureAwait(false);
        return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().UpdateRange(entities);
        return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> RemoveAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> RemoveRangeAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
