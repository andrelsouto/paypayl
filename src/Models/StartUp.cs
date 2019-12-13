using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PayPalApi.Models
{
    public class StartUp
    {
        public HttpClient Client { get; }

        public StartUp(HttpClient client) 
        {
            client.BaseAddress = new Uri("http://localhost:8082/api/agendamentos/5");
            Client = client;
        }

        public async Task<IEnumerable<ProductModel>> GetAspNetDocsIssues()
        {
            var response = await Client.GetAsync("");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<ProductModel>>(responseStream);
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}