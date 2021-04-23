using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleAppClient.Utilities;

namespace PantryPassionGUI.Models
{
    public class BackendConnection
    {
        static readonly HttpClient client = new HttpClient();
        private string _baseUrl = "https://localhost:44328/item";

        public static async Items CheckBarcode(string barcode)
        {
            string url = _base
            return new Items();
        }

        public static async Task<List<Reservation>> GetReservationsAsync()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, baseUrl))
            using (var response = await client.SendAsync(request))
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == false)
                {
                    throw new ApiException
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content
                    };
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                return JsonSerializer.Deserialize<List<Reservation>>(content, options);
            }
        }



        public void SetNewItem(string name, string category, string barcode)
        {

        }

        public void GetItemQuantity(string name)
        {

        }


        public void SetQuantity(string name, int quantity)
        {

        }
    }
}