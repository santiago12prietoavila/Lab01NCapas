﻿

using DAL;
using Entities.Models;
using System.Linq.Expressions;

//CreateAsync().GetAwaiter().GetResult();
RetreiveAsync().GetAwaiter().GetResult();


//Crear un objeto
static async Task CreateAsync()
{
    //Add customer
    Customer customer = new Customer()
    {
        FirstName = "Santiago",
        LastName = "Avila",
        City = "Bogota",
        Country = "Colombia",
        Phone = "321554644"
    };
    using(var repository = RepositoryFactory.CreateRepository()) 
    {
        try
        {
            var createdCustomer = await repository.CreateAsync(customer);
            Console.WriteLine($"Added Customer: {createdCustomer.LastName} {createdCustomer.FirstName}");

        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error:{ex.Message}");
        }
    }

}

static async Task RetreiveAsync()
{
    using (var repository = RepositoryFactory.CreateRepository())
    {
        try
        {
            Expression<Func<Customer, bool>> criteria = c => c.FirstName == "Santiago" && c.LastName == "Avila";
            var customer = await repository.RetreiveAsync(criteria);
            if (customer != null)
            {
                Console.WriteLine($"Retrived customer: {customer.FirstName}\t{customer.LastName}\t City: {customer.City}\t Country: {customer.Country}");
            }
            else
            {
                Console.WriteLine("Customer not exist");
            }    
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}