using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantryPassionGUI.Models
{
    public class SharedOberserverableCollectionOfInventoryItems
    {
        private static SharedOberserverableCollectionOfInventoryItems instance;
        private static readonly object Lock = new object();
        public ObservableCollection<InventoryItem> SharedInventoryItems { get; set; }
        public event EventHandler<EventArgs> UpdateShoppingList; 

        public static SharedOberserverableCollectionOfInventoryItems Instance()
        {
            lock (Lock)
            {
                if (instance == null)
                {
                    instance = new SharedOberserverableCollectionOfInventoryItems();
                }
                return instance;
            }
        }

        public bool SendUpdateEvent
        {
            set
            {
                UpdateShoppingListFunc(new EventArgs());
                SendUpdateEvent = value;
            }
        }
        private SharedOberserverableCollectionOfInventoryItems()
        {
            SharedInventoryItems = new ObservableCollection<InventoryItem>();
        }

        protected virtual void UpdateShoppingListFunc(EventArgs e)
        {
            UpdateShoppingList?.Invoke(this, e);
        }
    }
}
