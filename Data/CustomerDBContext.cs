using customer_api.Model;
using Microsoft.EntityFrameworkCore;

namespace customer_api.Data
{
    public class CustomerDBContext:DbContext
    {
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options)
        : base(options)
        {
        }
        public DbSet<Customer> Customers { set; get; } 
    }
}
