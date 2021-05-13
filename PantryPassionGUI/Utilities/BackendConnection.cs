using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly HttpClient _client;
        private static string _baseUrl = "https://localhost:44380/api";
        //private string _accessToken = "test";

        public BackendConnection()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.AccessJWTToken);
        }

        class CreateExistingItem
        {
            public int ItemId { get; set; }
            public int Amount { get; set; }
        }

        public async Task<Item> CheckBarcode(string barcode)
        {
            string url = _baseUrl + "/item/byEan/" + barcode;

            return await GetInformationFromBackendServer<Item>(url);
        }

        public async Task<Item> GetItemByName(string name)
        {
            string url = _baseUrl + "/item/byName?name=" + name;

            return await GetInformationFromBackendServer<Item>(url);
        }

        public async Task<Item> GetItemById(int id)
        {
            string url = _baseUrl + "/item/byId/" + id;

            return await GetInformationFromBackendServer<Item>(url);
        }

        public async Task<ObservableCollection<InventoryItem>> GetInventory()
        {
            string url = _baseUrl + "/Inventory";

            return await GetInformationFromBackendServer<ObservableCollection<InventoryItem>>(url);
        }

        public async Task<ObservableCollection<InventoryItem>> GetListOfInventoryItems(int itemId)
        {

            string url = _baseUrl + "/InventoryItem/" + itemId;
            return await GetInformationFromBackendServer<ObservableCollection<InventoryItem>>(url);
        }

        public async Task<ObservableCollection<Item>> GetListOfItems(int userId)
        {
            string url = _baseUrl + "/Item";

            return await GetInformationFromBackendServer<ObservableCollection<Item>>(url);
        }

        public async Task<T> GetInformationFromBackendServer<T>(string url)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await _client.SendAsync(request))
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

        public async Task<int> SetNewItem(InventoryItem inventoryItem, bool itemExistsInDatabase)
        {
            int type = inventoryItem.InventoryType;
            string url = "";
            object informationToSend = new object();

            if (itemExistsInDatabase)
            {
                url = _baseUrl + "/InventoryItem/existingItem/1/" + type;
                CreateExistingItem data = new CreateExistingItem();

                data.ItemId = inventoryItem.Item.ItemId;
                data.Amount = inventoryItem.Amount;
                informationToSend = data;
            }
            else
            {
                url = _baseUrl + "/InventoryItem/newItem/1/" + type;
                informationToSend = inventoryItem;
            }

            return await SendInformationToBackendServer(HttpMethod.Post, url, informationToSend);
        }

        private async Task<int> SendInformationToBackendServer(HttpMethod httpMethodType,string url, object informationToSend)
        {
            using (var request = new HttpRequestMessage(httpMethodType, url))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(informationToSend, options);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    using (var response = await _client
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

        //virker ikke hvis Amount er 0!!!! Backend delen kan ikke håntere det!!!
        public async Task<int> SetQuantity(InventoryItem inventoryItem)
        {
            string url = _baseUrl + "/InventoryItem";

            if (inventoryItem.Amount == 0)
            {
                url = url + "/" + inventoryItem.Item.ItemId + "/" + inventoryItem.DateAdded.ToString("s");

                return await SendInformationToBackendServer(HttpMethod.Delete, url, new object());
            }

            return await SendInformationToBackendServer(HttpMethod.Put, url, inventoryItem);
        }
    }
}