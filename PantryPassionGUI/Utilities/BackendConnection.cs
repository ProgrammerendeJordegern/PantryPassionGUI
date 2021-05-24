using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities.Interfaces;

namespace PantryPassionGUI.Utilities
{
    public class BackendConnection : IBackendConnection
    {
        //HttpClient in System.Net.Http
        private readonly HttpClient _client;

        //Base url to azure web service
        private static string _baseUrl = "https://pantrypassion-auecei4prj4gr3.azurewebsites.net/api";

        public BackendConnection()
        {
            _client = new HttpClient();

            //Get the JWT Token from Globals, and set it in the header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.AccessJWTToken);
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

        public async Task<ObservableCollection<Item>> GetListOfItems()
        {
            string url = _baseUrl + "/Item";

            return await GetInformationFromBackendServer<ObservableCollection<Item>>(url);
        }

        public async Task<ObservableCollection<InventoryItem>> GetInventoryItemListByType(int inventoryType)
        {
            string url = _baseUrl + "/Inventory/" + inventoryType;

            return await GetInformationFromBackendServer<ObservableCollection<InventoryItem>>(url);
        }

        //Inspired by code from https://johnthiriet.com/efficient-api-calls/
        //Changed to use JSON serializer in System.Text.Json
        private async Task<T> GetInformationFromBackendServer<T>(string url)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await _client.SendAsync(request))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode == false)
                    {
                        //Throw exception if a fail happened
                        throw new ApiException
                        {
                            StatusCode = (int)response.StatusCode,
                        };
                    }

                    //create options for JSON object deserializer
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

            //If the Item already exists in the database it don´t need all information about the inventoryItem
            if (itemExistsInDatabase)
            {
                url = _baseUrl + "/InventoryItem/existingItem/" + type;
                CreateExistingItem data = new CreateExistingItem();

                data.ItemId = inventoryItem.Item.ItemId;
                data.Amount = inventoryItem.Amount;
                informationToSend = data;
            }
            else
            {
                url = _baseUrl + "/InventoryItem/newItem/" + type;
                inventoryItem.InventoryType = 3;
                informationToSend = inventoryItem;
            }

            return await SendInformationToBackendServer(HttpMethod.Post, url, informationToSend);
        }

        //Inspired by code from https://johnthiriet.com/efficient-api-calls/
        //Changed to use JSON serializer in System.Text.Json
        private async Task<int> SendInformationToBackendServer(HttpMethod httpMethodType, string url, object informationToSend)
        {
            using (var request = new HttpRequestMessage(httpMethodType, url))
            {
                //create options for JSON object creation
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                //Generate JSON object with options
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
                            //Throw exception if a fail happened
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

        public async Task<int> SetQuantity(InventoryItem inventoryItem)
        {
            string url = _baseUrl + "/InventoryItem";

            //If the Amount of a inventoryItem is 0 it has to be deleted in the database 
            if (inventoryItem.Amount == 0)
            {
                url = url + "/" + inventoryItem.Item.ItemId + "/" + inventoryItem.DateAdded.ToString("s");

                return await SendInformationToBackendServer(HttpMethod.Delete, url, new object());
            }

            return await SendInformationToBackendServer(HttpMethod.Put, url, inventoryItem);
        }

        public async Task<int> EditItem(Item item)
        {
            string url = _baseUrl + "/Item";

            return await SendInformationToBackendServer(HttpMethod.Put, url, item);
        }

        public async Task<int> DeleteShoppingList()
        {
            string url = _baseUrl + "/Inventory/allContent/" + 3;

            return await SendInformationToBackendServer(HttpMethod.Delete, url, new object());
        }
    }
}