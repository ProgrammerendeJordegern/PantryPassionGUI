using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class ViewInventoriesViewModel : BindableBase
    {
        public ObservableCollection<string> CMBBX { get; set; }

        public ObservableCollection<InventoryItem> AllItems { get; set; }
        public ObservableCollection<InventoryItem> FridgeItems { get; set; }
        public ObservableCollection<InventoryItem> FreezerItems { get; set; }
        public ObservableCollection<InventoryItem> PantryItems { get; set; }

        public ObservableCollection<InventoryItem> ShoppingListItems { get; set; }

        private BackendConnection backendConn;

        public ViewInventoriesViewModel()
        {
            CMBBX = new ObservableCollection<string>();
            AllItems = new ObservableCollection<InventoryItem>();
            FridgeItems = new ObservableCollection<InventoryItem>();
            FreezerItems = new ObservableCollection<InventoryItem>();
            PantryItems = new ObservableCollection<InventoryItem>();
            ShoppingListItems = new ObservableCollection<InventoryItem>();
            backendConn = new BackendConnection();

            CMBBX.Add("Alle varer");
            CMBBX.Add("Køleskab");
            CMBBX.Add("Fryser");
            CMBBX.Add("Spisekammer (øvrige)");
            CMBBX.Add("Indkøbsliste");

            FridgeItems = await backendConn.GetInventory()

        }

    }
}
