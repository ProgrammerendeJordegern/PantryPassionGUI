using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Automation;
using PantryPassionGUI.Models;

namespace PantryPassionGUI.Utilities
{
    public class BackendConnection
    {
        private static readonly HttpClient Client = new HttpClient();
        private static string _baseUrl = "https://localhost:44380";

        class CreateExistingItem
        {
            public int ItemId { get; set; }
            public int Amount { get; set; }
        }

        public static async Task<Item> CheckBarcode(string barcode)
        {
            string url = _baseUrl + "/item/fromEan?ean=" + barcode;

            return await GetItemInformation<Item>(url);
        }

        //Virker ikke helt på backend delen!!!!
        public static async Task<Item> GetItemByName(string name)
        {
            string url = _baseUrl + "/item/fromName?name=" + name;

            return await GetItemInformation<Item>(url);
        }

        public static async Task<T> GetItemInformation<T>(string url)
        {
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
                        };
                    }

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    return JsonSerializer.Deserialize<T>(content, options);
                }
            }
        }

        public static async Task<int> SetNewItem(InventoryItem inventoryItem, bool itemExistsInDatabase)
        {
            int type = inventoryItem.InventoryType;
            string url = "";
            object informationToSend = new object();

            if (itemExistsInDatabase)
            {
                url = _baseUrl + "/inventoryitem/createwexistingitem?userId=1&type=" + type;
                CreateExistingItem data = new CreateExistingItem();

                data.ItemId = inventoryItem.Item.ItemId;
                data.Amount = inventoryItem.Amount;
                informationToSend = data;
            }
            else
            {
                url = _baseUrl + "/inventoryItem/createWNewItem?userId=1&type=" + type;
                informationToSend = inventoryItem;
            }

            return await SetItemInformation(url, informationToSend);
        }

        private static async Task<int> SetItemInformation(string url, object informationToSend)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(informationToSend, options);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    using (var response = await Client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode == false)
                        {
                            throw new ApiException
                            {
                                StatusCode = (int)response.StatusCode,
                            };
                        }

                        return (int)response.StatusCode;
                    }
                }
            }
        }

        public void GetItemQuantity(string name)
        {

        }

        //virker ikke endnu!!!!!!!!!!!!!!!!!!!
        public static async Task<int> SetQuantity(InventoryItem inventoryItem)
        {
            string url = _baseUrl + "/inventoryItem/edit";

            CreateExistingItem data = new CreateExistingItem();

            data.ItemId = inventoryItem.Item.ItemId;
            data.Amount = inventoryItem.Amount;
            object informationToSend = data;

            return await SetItemInformation(url, informationToSend);
        }
    }
}