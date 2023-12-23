using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrameworkDev.Web.Helpers
{
    public interface IRepository<TEntity, TIDType> : IDisposable
    {
        IQueryable<TEntity> GetList();

        Task<IQueryable<TEntity>> GetListAsync();

        IQueryable<TEntity> GetFilteredList(dynamic _params);

        Task<IQueryable<TEntity>> GetFilteredListAsync(dynamic _params);

        TEntity GetByID(TIDType id);

        Task<TEntity> GetByIDAsync(TIDType id);

        TEntity Insert(TEntity vm);

        Task<TEntity> InsertAsync(TEntity vm);

        TEntity Delete(TIDType id);

        Task<TEntity> DeleteAsync(TIDType id);

        TEntity Update(TEntity vm);

        Task<TEntity> UpdateAsync(TEntity vm);

        void Save();

        Task SaveAsync();
    }
}
