using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace CryptoFinder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "CryptoFinder by Purcify";
            Console.WriteLine("What crypto would you like to find information about?");
            string reply = Console.ReadLine().ToLower();

            using var client = new HttpClient(); 
            var response = await client.GetAsync("https://api.coincap.io/v2/assets/" + reply);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<ApiResponse>(jsonContent);

                if (jsonObject != null && jsonObject.Data != null)
                {
                    var price = jsonObject.Data.PriceUsd;
                    Console.WriteLine($"Current price of {reply}: ${price}");
                }
                else
                {
                    Console.WriteLine("Failed to parse JSON response.");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }

    public class ApiResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("priceUsd")]
        public decimal PriceUsd { get; set; }
    }
}