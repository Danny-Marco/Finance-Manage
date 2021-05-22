using System.Collections.Generic;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IRepository<Model> where Model : class
    {
        Model Get(int id);
        
        public List<Model> GetAll();

        void Delete(Model model);

        void Update(Model model);
    }
}