using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleAppClient.Utilities;


namespace PantryPassionGUI.Models
{
    public class BackendConnection
    {
        private static readonly HttpClient Client = new HttpClient();
        private static string _baseUrl = "https://localhost:44380";

        public static async Task<Item> CheckBarcode(string barcode)
        {
            string url = _baseUrl + "/item/fromEan?ean=" + barcode;

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

                    return JsonSerializer.Deserialize<Item>(content, options);
                }
            }
        }

        public async void SetNewItem(InventoryItem inventoryItem)
        {
            string url = _baseUrl + "/inventoryItem/createWNewItem";

            var json = JsonSerializer.Serialize(inventoryItem);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await Client.PostAsync(url,httpContent))
                {
                    if (response.IsSuccessStatusCode == false)
                    {
                        throw new ApiException
                        {
                            StatusCode = (int)response.StatusCode,
                        };
                    }
                }
        }

        public void GetItemQuantity(string name)
        {

        }


        public void SetQuantity(string name, int quantity)
        {

        }
    }
}