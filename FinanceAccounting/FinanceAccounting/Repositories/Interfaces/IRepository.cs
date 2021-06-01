using System.Collections.Generic;
using FinanceAccounting.Models.Interfaces;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IRepository<Model> where Model : IEntity
    {
        Model Get(int id);

        public List<Model> GetAll();

        void Delete(Model model);

        void Update(Model model);

        void Add(Model model);

        void Save();
    }
}