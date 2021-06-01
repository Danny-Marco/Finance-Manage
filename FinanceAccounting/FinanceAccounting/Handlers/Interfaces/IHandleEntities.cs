
namespace FinanceAccounting.Observer.Interfaces
{
    // public interface IHandleEntities<T> where T : IEntity
    public interface IHandleEntities
    {
        void Insert();
        
        void DeleteRemoved();
        
        void UpdateDirty();
    }
}