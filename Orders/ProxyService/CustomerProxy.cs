using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyService.Interfaces;
using System.Net.Http.Headers;
using Entities.Models;
using System.Text.Json;
namespace ProxyService
{
    public class CustomerProxy : ICustomerProxy
    {
        private readonly HttpClient _httpClient;

        public CustomerProxy()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7045/api/customer") //Asegurte de que esta URL coincida con tu URL
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Customer>>(json,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                // throw; // Opción para propagar la excepción
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                // Realiza una solicitud HTTP GET a la URL con el ID del cliente
                var response = await _httpClient.GetAsync($"customers/{id}");
                response.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta como JSON
                var json = await response.Content.ReadAsStringAsync();

                // Deserializa el JSON en un objeto Customer
                return JsonSerializer.Deserialize<Customer>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            try
            {
                // Serializa el objeto Customer a formato JSON
                var json = JsonSerializer.Serialize(customer);

                // Crea un contenido HTTP con el JSON serializado
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Envía una solicitud HTTP POST al servidor
                var response = await _httpClient.PostAsync("", content);

                // Verifica que la respuesta HTTP sea exitosa
                response.EnsureSuccessStatusCode();

                // Lee la respuesta JSON del servidor
                var responseJson = await response.Content.ReadAsStringAsync();

                // Deserializa la respuesta JSON en un objeto Customer
                return JsonSerializer.Deserialize<Customer>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateAsync(int id, Customer customer)
        {
            try
            {
                // Serializa el objeto Customer a formato JSON
                var json = JsonSerializer.Serialize(customer);

                // Crea un contenido HTTP con el JSON serializado
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Envía una solicitud HTTP PUT al servidor con la URL construida dinámicamente
                var response = await _httpClient.PutAsync($"{id}", content);

                // Devuelve true si la solicitud fue exitosa, false en caso contrario
                return response.IsSuccessStatusCode;
            }
            catch (global::System.Exception)
            {
                // Propaga la excepción al llamador del método
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // Envía una solicitud HTTP DELETE al servidor con la URL construida dinámicamente
                var response = await _httpClient.DeleteAsync($"{id}");

                // Devuelve true si la solicitud fue exitosa, false en caso contrario
                return response.IsSuccessStatusCode;
            }
            catch (global::System.Exception)
            {
                // Propaga la excepción al llamador del método
                throw;
            }
        }
    }
}
