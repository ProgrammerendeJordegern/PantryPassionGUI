﻿using System.Collections.Generic;
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
            int type = inventoryItem.InventoryType;
            string url = _baseUrl + "/inventoryItem/createWNewItem?userId=1&type=" + type;

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(inventoryItem, options);
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
                    }
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