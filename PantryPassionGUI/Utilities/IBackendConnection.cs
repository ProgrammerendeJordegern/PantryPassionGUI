using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PantryPassionGUI.Models;

namespace PantryPassionGUI.Utilities
{
    public interface IBackendConnection
    {
        Task<Item> CheckBarcode(string barcode);
        Task<Item> GetItemByName(string name);
        Task<Item> GetItemById(int id);
        Task<ObservableCollection<InventoryItem>> GetListOfInventoryItems(int itemId);
        Task<ObservableCollection<Item>> GetListOfItems();
        Task<ObservableCollection<InventoryItem>> GetInventoryItemListByType(int inventoryType);
        Task<int> SetNewItem(InventoryItem inventoryItem, bool itemExistsInDatabase);
        Task<int> SetQuantity(InventoryItem inventoryItem);
        Task<int> DeleteShoppingList();
        Task<int> EditItem(Item item);
    }
}
