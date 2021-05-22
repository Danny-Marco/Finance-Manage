namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IRepository<Model> where Model : class
    {
        public void Create(Model model);
    }
}