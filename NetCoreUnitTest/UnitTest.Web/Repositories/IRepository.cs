using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTest.Web.Repositories
{
    public partial interface IRepository<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
