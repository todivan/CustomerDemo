using CustomerDemo.Model;

namespace CustomerDemo.DB
{
    public class CustomerRepository : IRepository<Customer>
    {
        private List<Customer> DB = new List<Customer>();

        public Customer Add(Customer entity)
        {
            entity.Id = Guid.NewGuid();
            DB.Add(entity);
            return entity;
        }

        public Customer Delete(Guid id)
        {
            Customer entity = DB.FirstOrDefault(x => x.Id.CompareTo(id) == 0);
            if(entity != null)
            {
                DB.Remove(entity);
            }

            return entity;
        }

        public Customer Get(Guid id)
        {
            return DB.FirstOrDefault(x => x.Id.CompareTo(id) == 0);
        }

        //Example of async
        public async Task<List<Customer>> GetAll()
        {
            await Task.Delay(1000); //Simulate DB request delay
            return DB;
        }

        public Customer Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
