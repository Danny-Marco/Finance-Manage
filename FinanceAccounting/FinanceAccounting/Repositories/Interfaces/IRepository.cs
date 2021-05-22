namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IRepository<Model> where Model : class
    {
        Model Get(int id);

        void Create(Model model);

        void Delete(int modelId);

        void Update(Model model);
    }
}