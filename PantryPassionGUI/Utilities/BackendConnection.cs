using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleAppClient.Utilities;

namespace PantryPassionGUI.Models
{
    public class BackendConnection
    {
        private static readonly HttpClient Client = new HttpClient();
        private static string _baseUrl = "https://localhost:44328/item";

        public static async Task<Items> CheckBarcode(string barcode)
        {
            string url = _baseUrl += "/" + barcode;

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await Client.SendAsync(request))
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

                    return JsonSerializer.Deserialize<Items>(content, options);
                }
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