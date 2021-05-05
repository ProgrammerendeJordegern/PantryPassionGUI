using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private SharedOberserverableCollectionOfInventoryItems()
        {
            SharedInventoryItems = new ObservableCollection<InventoryItem>();
        }
    }
}
