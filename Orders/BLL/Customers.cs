using BLL.Exceptions;
using DAL;
using Entities. Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Customers
    {
        public async Task<Customer> CreateAsync(Customer customer)
        {
            Customer customerResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre del cliente existe
                Customer customerSearch = await repository.RetreiveAsync<Customer>(c => c.FirstName == customer.FirstName);
                if (customerSearch == null)
                {
                    // No existe, podemos crearlo
                    customerResult = await repository.CreateAsync(customer);
                }
                else
                {
                    // Podríamos aquí lanzar una excepción
                    // Para notificar que el cliente ya existe
                    // Podríamos incluso crear una capa de excepciones
                    // personalizadas y consumirlas desde otras capas.
                    CustomerExceptions.ThrowCustomerAlreadyExistsException(customerSearch.FirstName, customerSearch.LastName);
                }
            }
            return customerResult;
        }

        public async Task<Customer> RetreiveByIDAsync(int id)
        {
            Customer result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Customer customer = await repository.RetreiveAsync<Customer>(c => c.Id == id);

                // Check if customer was found
                if (customer == null)
                {
                    CustomerExceptions.ThrowInvalidCustomerIdException(id);

                }
                return customer!;
            }

        }

        public async Task<List<Customer>> RetrieveAllAsync()
        {
            List<Customer> Result = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Define el criterio de filtro para obtener todos los clientes.

                Expression<Func<Customer, bool>> allCustomersCriteria = x => true;

                Result = await r.FilterAsync<Customer>(allCustomersCriteria);
            }
            return Result;
        }
        public async Task<bool> UpdateAsync(Customer customer)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del cliente no exista con un ID diferente
                Customer customerSearch = await repository.RetreiveAsync<Customer>(c => c.FirstName == customer.FirstName && c.Id != customer.Id);
                if (customerSearch == null)
                {
                    // El cliente no existe, podemos actualizarlo
                    result = await repository.UpdateAsync(customer);
                }
                else
                {
                    // Lanzamos una excepción para indicar que el cliente ya existe
                    CustomerExceptions.ThrowCustomerAlreadyExistsException(customerSearch.FirstName, customer.LastName);
                }
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool Result = false;
            // Buscar un cliente para ver si tiene Orders (Ordenes de Compra)
            var customer = await RetreiveByIDAsync(id);
            if (customer != null)
            {

                // Eliminar el cliente
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    Result = await repository.DeleteAsync(customer);
                }

            }
            else
            {
                // Podemos implementar alguna lógica
                // para indicar que el producto no existe 
                CustomerExceptions.ThrowInvalidCustomerIdException(id);
            }
            return Result;
        }
    }



    
}
