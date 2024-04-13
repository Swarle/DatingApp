using DatingApp.DAL.Context;
using DatingApp.DAL.Extensions;
using DatingApp.DAL.Infrastructure;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(TEntity entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, bool applyTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            
            if (!applyTracking)
                query = query.AsNoTracking();
            
            return await query.ApplySpecification(specification).ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPagedCollectionAsync(ISpecification<TEntity> specification,
            int? pageNumber, int? pageSize = null, bool applyTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            var totalCollectionCount = await query.ApplySpecification(specification).CountAsync();

            var page = pageNumber ?? 1;
            var size = pageSize ?? totalCollectionCount;

            query = query.ApplySpecification(specification)
                .Skip((page - 1) * size)
                .Take(size);
            
            if (!applyTracking)
                query = query.AsNoTracking();

            var data = await query.ToListAsync();

            return new PagedList<TEntity>(data, totalCollectionCount, page, size);
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(ISpecification<TEntity> specification, bool applyTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            
            if (!applyTracking)
                query = query.AsNoTracking();
            
            return await query.ApplySpecification(specification).SingleOrDefaultAsync();
        }

        public async Task Update(TEntity entity)
        {
            await Task.Run(() => _dbSet.Update(entity));
        }
        
        public async Task<bool> IsSatisfiedAsync(ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> query = _dbSet;
            return await query.ApplySpecification(specification).AnyAsync();
        }
        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        
    }
}
