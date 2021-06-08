using System.Collections.Generic;
using FinanceAccounting.Models.Interfaces;

namespace FinanceAccounting.Observer.Interfaces
{
    public interface IHandleEntities<T> where T : IEntity
    {
        void RegisterNew(T entity, ref bool isAdded);

        void RegisterDelete(T entity);

        void RegisterDirty(T entity);

        void DisposeNew(T entity);

        void DisposeDirty(T entity);

        void DisposeRemoved(T entity);
        
        void Insert();
        
        void DeleteRemoved();
        
        void UpdateDirty();
        
        void AddToStored(T entity);
        
        void AddToStored(List<T> entities);
        
        T GetById(int id);
    }
}