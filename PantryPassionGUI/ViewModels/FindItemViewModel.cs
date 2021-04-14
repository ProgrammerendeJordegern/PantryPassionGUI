using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PantryPassionGUI.Models;

namespace PantryPassionGUI.ViewModels
{
    
    class FindItemViewModel
    {
        public FindItemViewModel()
        {
            Item i1 = new Item("Nutella", "42069", 100, "500g");
            Item i2 = new Item("Kyllingebryst", "1337", 10, "1000g");
            Item i3 = new Item("Støvsuger", "666", 3, "69kg");

            Items = new ObservableCollection<Item>();
            Items.Add(i1);
            Items.Add(i2);
            Items.Add(i3);

            
        }

        private FindItem FindItemWindow;

        public ObservableCollection<Item> Items { get; set; }

    }
}
