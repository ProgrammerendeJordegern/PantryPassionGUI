using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ICollectionView ViewFilter;

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
            CMBBX.Add("Ukendt");

            GetInventoryItems();

        }

        public string GetCategory(int id)
        {
            switch (id)
            {
                case 0:
                    return "Fryser";
                    break;
                case 1:
                    return "Køleskab";
                    break;
                case 2:
                    return "Spisekammer (øvrige)";
                    break;
                case 3:
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
                    switch (inventoryItem.InventoryType)
                    {
                        case 0:
                            FreezerItems.Add(inventoryItem.Item);
                            break;
                        case 1:
                            FridgeItems.Add(inventoryItem.Item);
                            break;
                        case 2:
                            PantryItems.Add(inventoryItem.Item);
                            break;
                        case 3:
                            ShoppingListItems.Add(inventoryItem.Item);
                            break;
                        default:
                            break;
                    }
                    AllItems.Add(inventoryItem.Item);
                    inventoryItem.Category = GetCategory(inventoryItem.InventoryType);
                }

                ViewFilter = (CollectionView)CollectionViewSource.GetDefaultView(AllInventoryItems);

                ViewFilter.Filter = UserFilter;

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

        private bool UserFilter(object item)
        {
            if (CmbbxFilter != "Alle varer")
                return (item as InventoryItem).Category.IndexOf(CmbbxFilter, StringComparison.OrdinalIgnoreCase) >= 0;

            return true;
        }

        private string cmbbxFilter;

        public string CmbbxFilter
        {
            get { return cmbbxFilter; }
            set
            {
                cmbbxFilter = value;
                ViewFilter.Refresh();
                RaisePropertyChanged("CmbbxFilter");
            }
        }
    }
}
