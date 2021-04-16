using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using PantryPassionGUI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    
    class FindItemViewModel : BindableBase
    {
        private FindItemWindow FindItemWindow;
        public ObservableCollection<Item> Items { get; set; }

        public FindItemViewModel()
        {
            Item i1 = new Item("Nutella", "42069", 100, "500g");
            Item i2 = new Item("Kyllingebryst", "1337", 10, "1000g");
            Item i3 = new Item("Støvsuger", "666", 3, "69kg");
            Item i4 = new Item("Blomkål", "Blomkål", 9999, "~kg");
            Item i5 = new Item("Glock 9mm", "MLG42066669", 1, "1kg");

            Items = new ObservableCollection<Item>();
            Items.Add(i1);
            Items.Add(i2);
            Items.Add(i3);
            Items.Add(i4);
            Items.Add(i5); 
            
            ViewFilter = (CollectionView)CollectionViewSource.GetDefaultView(Items);
            //ViewFilter.Filter = o => String.IsNullOrEmpty(Filter) || ((string)o).Contains(Filter);
            ViewFilter.Filter = UserFilter;



        }

        private bool UserFilter(object item)
        {
            if (!(String.IsNullOrEmpty(EANFilter)))
                return ((item as Item).EAN.IndexOf(EANFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            else if (!(String.IsNullOrEmpty(NameFilter)))
                return ((item as Item).Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            else return true;
        }

        private ICollectionView ViewFilter;
        private string namefilter;
        
        public string NameFilter
        {
            get { return namefilter; }
            set
            {
                if (value != namefilter)
                {
                    namefilter = value;
                    ViewFilter.Refresh();
                    RaisePropertyChanged("NameFilter");
                }

            }
        }

        private string eanfilter;

        public string EANFilter
        {
            get { return eanfilter; }
            set
            {
                if (value != eanfilter)
                {
                    eanfilter = value;
                    ViewFilter.Refresh();
                    RaisePropertyChanged("EANFilter");
                }

            }
        }

        //ICommand _okButtonCommand;

        //public ICommand OkButtonCommand
        //{
        //    get { return _okButtonCommand ?? (_okButtonCommand = new DelegateCommand(OkButtonExecute)); }
        //}

        //void OkButtonExecute()
        //{
        //    FindItemWindow FIW1 = new FindItemWindow();
        //    FIW1.Close();
        //}

        //private void Cancel(Window window)
        //{
        //    window.Close();
        //}

        //private ICommand _cancelCommand;
        //public ICommand CancelCommand
        //{
        //    get
        //    {
        //        return _cancelCommand ?? (_cancelCommand = new Command.RelayCommand<Window>(
        //            (window) => Cancel(window),
        //            (window) => (true)));
        //    }
        //}
    }


    




}
