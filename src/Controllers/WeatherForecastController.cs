using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayPalApi.Models;
using BraintreeHttp;
using PayPalCheckoutSdk.Orders;

namespace PayPalApi.Controllers
{
    [ApiController]
    [Route("api/teste")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("paypal")]
        public async Task<string> paypal() {

            HttpResponse response;

            var order = new OrderRequest() {
                Intent = "AUTHORIZE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                new PurchaseUnitRequest()
                {
                    Amount = new AmountWithBreakdown()
                    {
                        CurrencyCode = "BRL",
                        Value = "100.00"
                    }
                }
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = "https://www.example.com",
                    CancelUrl = "https://www.example.com"
                }
            };

         var request = new OrdersCreateRequest();
         request.Prefer("return=representation");
         request.RequestBody(order);
         response = await PayPalClient.client().Execute(request);
         var statusCode = response.StatusCode;
         Order result = response.Result<Order>();
         Console.WriteLine("Status: {0}", result.Status);
         Console.WriteLine("Order Id: {0}", result.Id);
         Console.WriteLine("Intent: {0}", result.Intent);
         Console.WriteLine("Links:");
         foreach (LinkDescription link in result.Links)
            {
                 Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }
            return PayPalClient.ObjectToJSONString(result);
        }

        // [HttpGet("paypal")]
        // public async Task<HttpResponse> paypal() {

        //     HttpResponse response;

        //     var payer = new Payer()
        // }

        
        public async static Task<HttpResponse> captureOrder(string id)
        {
            var request = new OrdersCaptureRequest(id);
            request.RequestBody(new OrderActionRequest());
            HttpResponse response = await PayPalClient.client().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Capture Id: {0}", result.Id);
            return response;
        }

        [HttpGet("paypal/{id}")]
        public async Task<String> GetOrder(string id, bool debug = false)
        {
        OrdersGetRequest request = new OrdersGetRequest(id);
        //3. Call PayPal to get the transaction
        var response = await PayPalClient.client().Execute(request);
        //4. Save the transaction in your database. Implement logic to save transaction to your database for future reference.
        var result = response.Result<Order>();
        Console.WriteLine("Retrieved Order Status");
        Console.WriteLine("Status: {0}", result.Status);
        Console.WriteLine("Order Id: {0}", result.Id);
        Console.WriteLine("Intent: {0}", result.Intent);
        Console.WriteLine("Links:");
        foreach (LinkDescription link in result.Links)
        {
            Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
        }
        AmountWithBreakdown amount = result.PurchaseUnits[0].Amount;
        Console.WriteLine("Total Amount: {0} {1}", amount.CurrencyCode, amount.Value);

        return PayPalClient.ObjectToJSONString(response.Result<Order>());
        }
        
    }
}
