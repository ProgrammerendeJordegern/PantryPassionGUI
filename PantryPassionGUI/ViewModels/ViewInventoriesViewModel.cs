using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class ViewInventoriesViewModel : BindableBase
    {
        public ObservableCollection<string> CMBBX { get; set; }

        public ObservableCollection<InventoryItem> AllInventoryItems { get; set; }
        public ObservableCollection<Item> AllItems { get; set; }
        public ObservableCollection<Item> FridgeItems { get; set; }
        public ObservableCollection<Item> FreezerItems { get; set; }
        public ObservableCollection<Item> PantryItems { get; set; }

        public ObservableCollection<Item> ShoppingListItems { get; set; }

        public BackendConnection BackendConn { get; set; }

        public ViewInventoriesViewModel()
        {
            CMBBX = new ObservableCollection<string>();
            AllInventoryItems = new ObservableCollection<InventoryItem>();
            AllItems = new ObservableCollection<Item>();
            FridgeItems = new ObservableCollection<Item>();
            FreezerItems = new ObservableCollection<Item>();
            PantryItems = new ObservableCollection<Item>();
            ShoppingListItems = new ObservableCollection<Item>();
            BackendConn = new BackendConnection();

            CMBBX.Add("Alle varer");
            CMBBX.Add("Køleskab");
            CMBBX.Add("Fryser");
            CMBBX.Add("Spisekammer (øvrige)");
            CMBBX.Add("Indkøbsliste");

            GetInventoryItems();

            

            //MessageBox.Show(AllInventoryItems.Count.ToString());
        }

        //public string Category
        //{
        //    get { return GetCategory(); }
        //}

        public string GetCategory(int id)
        {
            switch (id)
            {
                case 1:
                    return "Fryser";
                    break;
                case 2:
                    return "Køleskab";
                    break;
                case 3:
                    return "Spisekammer (øvrige)";
                    break;
                case 4:
                    return "Indkøbsliste";
                    break;
                default:
                    return "Ukendt";
                    break;
            }
        }

        public async void GetInventoryItems()
        {
            try
            {
                AllInventoryItems = await BackendConn.GetInventory();

                foreach (var inventoryItem in AllInventoryItems)
                {
                    switch (inventoryItem.InventoryId)
                    {
                        case 1:
                            FreezerItems.Add(inventoryItem.Item);
                            break;
                        case 2:
                            FridgeItems.Add(inventoryItem.Item);
                            break;
                        case 3:
                            PantryItems.Add(inventoryItem.Item);
                            break;
                        case 4:
                            ShoppingListItems.Add(inventoryItem.Item);
                            break;
                        default:
                            break;
                    }
                    AllItems.Add(inventoryItem.Item);
                    inventoryItem.Category = GetCategory(inventoryItem.InventoryId);
                }

            }
            catch (ApiException exception)
            {
                ItemNotFound(exception.StatusCode);
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }
            finally
            {
                RaisePropertyChanged("AllInventoryItems");
            }
        }

        private void ItemNotFound(int statusCode)
        {
            MessageBox.Show($"Fejl {statusCode}\nVare belv ikke fundet i systemet!\nIndtast venlist selv vares informationer", "Error!");
        }

    }

}
