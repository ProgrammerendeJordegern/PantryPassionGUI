using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PantryPassionGUI.Models;

namespace PantryPassionGUI.ViewModels
{
    public interface IFindItemViewModel
    {
        int CurrentIndex { get; set; }
        Item CurrentItem { get; set; }
        ICommand OkCommand { get; }
        ICommand ScanEANCommand { get; }
        ObservableCollection<Item> Items { get; set; }
        string EANFilter { get; set; }
        string NameFilter { get; set; }
    }
}
