using BLL;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using SLC;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using BLL.Exceptions;

namespace Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase, ICustomerService
    {
        private readonly Customers _bll; //Dependency injection for better testability

        public CustomerController(Customers bll)
        {
            _bll = bll;
        }
       
        //GET : api/<CustomerController>
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            try
            {
                var result = await _bll.RetrieveAllAsync();
                return Ok(result); // Use IActionResult for more flexibility (200 OK)
            }
            catch (CustomerExceptions ex) // Catch specific business logic exceptions
            {
                return BadRequest(ex.Message); // Return 400 Bad Request with error message
            }
            catch (Exception ex) // Catch unhandled exceptions for logging and generic error response
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        //GET api/<CustomerController>/5
        [HttpGet("{id}", Name = "RetrieveAsync")]
        public async Task<ActionResult<Customer>> RetrieveAsync(int id)
        {
            try
            {
                var customer = await _bll.RetreiveByIDAsync(id);

                if (customer == null)
                {
                    return NotFound("Customer not found."); // Use NotFound result for missing resources
                }

                return Ok(customer);
            }
            catch (CustomerExceptions ce)
            {
                return BadRequest(ce.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        // POST: api/<CustomerController>
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateAsync([FromBody] Customer toCreate)
        {
            try
            {
                var customer = await _bll.CreateAsync(toCreate);
                return CreatedAtRoute("RetrieveAsync", new { id = customer.Id }, customer);
            }
            catch (CustomerExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        // PUT api/<CustomerController>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Customer toUpdate)
        {
            // Asigna el ID recibido a la entidad a actualizar
            toUpdate.Id = id;

            try
            {
                // Llama a la lógica de negocio para actualizar el cliente
                var result = await _bll.UpdateAsync(toUpdate);

                // Verifica si la actualización fue exitosa
                if (!result)
                {
                    // Si no se actualizó, devuelve un mensaje informativo
                    return NotFound("Customer not found or update failed.");
                }

                // Si se actualizó correctamente, devuelve un NoContent
                return NoContent();
            }
            catch (CustomerExceptions ex)
            {
                // Maneja excepciones específicas de clientes
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Maneja excepciones generales
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _bll.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Customer not found or deletion failed."); // Mensaje informativo si la eliminación falla
                }
                return NoContent(); // NoContent indica una eliminación exitosa sin contenido a retornar
            }
            catch (CustomerExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }


}
