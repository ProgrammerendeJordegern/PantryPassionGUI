﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class ShoppingListViewModel : BindableBase
    {
        public ObservableCollection<InventoryItem> ItemsList { get; set; }

        private BackendConnection _backendConnection;
        private ICommand _autoGenerateListCommand;
        private ICommand _addItemToListCommand;
        private ICommand _clearListCommand;
        private ICommand _addItemsOnListToOwnedItemsCommand;
        private ICommand _updateListCommand;
        private ICommand _cancelCommand;

        public ShoppingListViewModel()
        {
            _backendConnection = new BackendConnection();
        }

        public ICommand AutoGenerateListCommand
        {
            get
            {
                return _autoGenerateListCommand ??= new DelegateCommand(AutoGenerateListHandler);
            }
        }

        private void AutoGenerateListHandler()
        {
            //Fra præferencer
        }

        public ICommand AddItemToListCommand
        {
            get
            {
                return _addItemToListCommand ??= new DelegateCommand(AddItemToListHandler);
            }
        }

        private void AddItemToListHandler()
        {
            //Brug find funktion fra finditemview
            FindItemWindow findItemView = new FindItemWindow();
            //findItemView.ShowDialog();

            
        }

        private InventoryItem _currentItem = null;
        public InventoryItem CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                SetProperty(ref _currentItem, value);
            }
        }

        private int _currentIndex = -1;
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                SetProperty(ref _currentIndex, value);
            }
        }

        public ICommand ClearListCommand
        {
            get
            {
                return _clearListCommand ??= new DelegateCommand(ClearListHandler);
            }
        }

        private void ClearListHandler()
        {
            //Clear list in view
            ItemsList.Clear();

            //Clear list in db

        }

        public ICommand AddItemsOnListToOwnedItemsCommand
        {
            get
            {
                return _addItemsOnListToOwnedItemsCommand ??= new DelegateCommand(AddItemsOnListToOwnedItemsHandler);
            }
        }

        private void AddItemsOnListToOwnedItemsHandler()
        {
            //Tjek om varen allerede findes i ejede vare - true = tilføj antallet til det nuværende antal

            //Hvis varen ikke allerede findes i ejede vare tjek db om varen findes der - true = tilføj antal og kategori

            //Hvis varen ikke finde nogen steder skal der så tilføjes stregkode og kategori??
        }

        public ICommand UpdateListCommand
        {
            get
            {
                return _updateListCommand ??= new DelegateCommand(UpdateListHandler);
            }
        }

        private void UpdateListHandler()
        {
            //_backendConnection.SendInformationToBackendServer("Test", "Test", "Test");
            //Update to db

            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(CancelHandler);
            }
        }

        private void CancelHandler()
        {
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }
    }
}
