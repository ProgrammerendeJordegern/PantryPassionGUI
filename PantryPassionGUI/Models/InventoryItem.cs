using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PantryPassionGUI.Models
{
    public class InventoryItem : BindableBase, INotifyDataErrorInfo
    {
        private int _amount;
        private string _category;

        public int InventoryId { get; set; }
        public int InventoryType { get; set; }
        public Item Item { get; set; }

        public int Amount
        {
            get
            {
                return _amount;
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
                SetProperty(ref _amount, value);

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
                switch (_category)
                {
                    case "Alle vare":
                        InventoryType = 0;
                        break;
                    case "Køleskab":
                        InventoryType = 1;
                        break;
                    case "Fryser":
                        InventoryType = 2;
                        break;
                    case "Spisekammer (øvrige vare)":
                        InventoryType = 3;
                        break;
                }
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
