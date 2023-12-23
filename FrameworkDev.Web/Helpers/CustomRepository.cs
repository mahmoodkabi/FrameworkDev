using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Helpers
{
    public class CustomRepository<TEntity, TIDType> : IRepository<TEntity, TIDType> where TEntity : new()
    {
        public readonly FrameworkDevEntities context = new FrameworkDevEntities();

        public string Username { get => context.Username; set => context.Username = value; }

        public string IP { get => context.IP; set => context.IP = value; }

        public CustomRepository() { }

        public virtual IQueryable<TEntity> GetList()
        {
            return null;
        }

        public virtual IQueryable<TEntity> GetFilteredList(dynamic _params)
        {
            return null;
        }

        public virtual IQueryable<TEntity> GetListByRelId(TIDType _relId)
        {
            return null;
        }

        public virtual IQueryable<VM_Lookup> GetLookupList()
        {
            return null;
        }

        public virtual IQueryable<VM_Tree> GetTreeList(int? parentId, Dictionary<string, object> _params)
        {
            return null;
        }

        public virtual TEntity GetByID(TIDType id)
        {
            return new TEntity();
        }

        public virtual TEntity Insert(TEntity vm)
        {
            return new TEntity();
        }

        public virtual TEntity Delete(TIDType id)
        {
            return new TEntity();
        }

        public virtual TEntity Update(TEntity vm)
        {
            return new TEntity();
        }

        public virtual TEntity Delete(TEntity vm)
        {
            return new TEntity();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        #region Async

        public Task<IQueryable<TEntity>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        public Task<IQueryable<TEntity>> GetFilteredListAsync(dynamic _params)
        {
            return Task.Run(() => (IQueryable<TEntity>)GetFilteredList(_params));
        }

        public Task<IQueryable<TEntity>> GetListByRelIdAsync(TIDType _relId)
        {
            return Task.Run(() => GetListByRelId(_relId));
        }

        public Task<IQueryable<VM_Lookup>> GetLookupListAsync()
        {
            return Task.Run(() => GetLookupList());
        }

        public Task<IQueryable<VM_Tree>> GetTreeListAsync(int? parentId, Dictionary<string,object> _params)
        {
            return Task.Run(() => GetTreeList(parentId, _params));
        }

        public Task<TEntity> GetByIDAsync(TIDType id)
        {
            return Task.Run(() => GetByID(id));
        }

        public Task<TEntity> InsertAsync(TEntity vm)
        {
            return Task.Run(() => Insert(vm));
        }

        public Task<TEntity> DeleteAsync(TIDType id)
        {
            return Task.Run(() => Delete(id));
        }

        public Task<TEntity> UpdateAsync(TEntity vm)
        {
            return Task.Run(() => Update(vm));
        }

        public Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        #endregion Async

        #region IDisposable Support

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
