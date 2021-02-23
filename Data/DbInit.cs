
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using System;
using customer_api.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace customer_api.Data
{
    public class DbInit
    {
        public static void Initialize(CustomerDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            string path = "./Data/list.json";
            string readText = File.ReadAllText(path,Encoding.UTF8);
            List<Customer> customer = new List<Customer>();
            customer= JsonSerializer.Deserialize<List<Customer>> (readText);
            context.Customers.AddRange(customer);
            context.SaveChanges();
        }
    }
}
