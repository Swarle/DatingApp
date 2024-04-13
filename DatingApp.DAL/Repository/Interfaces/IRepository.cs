using DatingApp.DAL.Specification.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.DAL.Infrastructure;

namespace DatingApp.DAL.Repository.Interfaces
{
    public interface IRepository<TEntity>  where TEntity : class 
    {
        public Task CreateAsync(TEntity entity);
        public Task<TEntity?> GetByIdAsync(int id);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity?> GetFirstOrDefaultAsync(ISpecification<TEntity> specification, bool applyTracking = true);
        public Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, bool applyTracking = true);
        public Task<PagedList<TEntity>> GetPagedCollectionAsync(ISpecification<TEntity> specification,
            int? pageNumber = 1, int? pageSize = null, bool applyTracking = true);
        public Task Update(TEntity entity);
        public Task Delete(TEntity entity);
        Task<bool> IsSatisfiedAsync(ISpecification<TEntity> specification);
        public Task SaveChangesAsync();




    }
}
