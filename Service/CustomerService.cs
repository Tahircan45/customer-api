using AutoMapper;
using customer_api.Data;
using customer_api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer_api.Service
{
    public class CustomerService
    {
        private readonly CustomerDBContext _context;

        public CustomerService(CustomerDBContext context, IMapper mapper)
        {
            _context = context;

        }
        public async Task<List<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<List<Customer>> GetAll(int pageNumber,int pageSize)
        {
            return await _context.Customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<Customer> Get(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
        public async Task<bool> Update(int id, Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<int> Add(Customer customer)
        {       
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }
        public async Task<bool> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> TotalRecord()
        {
            return await _context.Customers.CountAsync();
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
