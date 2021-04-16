using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Prism.Mvvm;

namespace PantryPassionGUI.Models
{
    public class Items : BindableBase, INotifyDataErrorInfo
    {
        private string _name;
        private int _quantity;
        private string _date;
        private string _category;

        public Items()
        {
            _date = DateTime.Now.ToLongDateString();
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    List<string> errors = new List<string>
                    {
                        "Navn skal udfyldes."
                    };
                    SetErrors("Name", errors);
                }
                else
                {
                    ClearErrors("Name");
                }
                SetProperty(ref _name, value);
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value < 0)
                {
                    List<string> errors = new List<string>
                    {
                        "Antal kan ikke være negativ."
                    };
                    SetErrors("Quantity", errors);
                }
                else
                {
                    ClearErrors("Quantity");
                }
                SetProperty(ref _quantity, value);
               
            }
        }

        public string Date
        {
            get
            {
                return _date;
            }
            set
            {

                SetProperty(ref _date, value);
            }
        }

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                SetProperty(ref _category, value);
                Debug.WriteLine(Category);
            }
        }


        #region INotifyDataErrorInfo implementation
        public bool HasErrors
        {
            get
            {
                // Indicate whether the entire object is error-free.
                return (errors.Count > 0);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collections.
                return (errors.Values);
            }
            else
            {
                // Provice the error collection for the requested property
                // (if it has errors).
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
    }
}
