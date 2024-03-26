using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository.Interfaces
{
    public interface IRepository<TEntity>  where TEntity : class 
    {
        public Task CreateAsync(TEntity entity);
        public Task<TEntity?> GetByIdAsync(int id);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task Update(TEntity entity);
        public Task Delete(TEntity entity);
        public Task SaveChangesAsync();
    }
}
