using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PantryPassionGUI.Models
{
    public class Item : BindableBase, INotifyDataErrorInfo
    {
        private string _name;
        private DateTime _date;
        private int _size;
        private string _sizeUnit;
        private string _ean;

        public enum UnitsEnum
        {
            Gram,
            kg,
            Liter,
            ml,
            stk
        }

        public Item(string name, string ean = "", int averageLifespanDays = 0, int itemSize = 0)
        {
            Name = name;
            Ean = ean;
            AverageLifespanDays = averageLifespanDays;
            Size = itemSize;
        }

        public Item()
        {
            _date = DateTime.Now;
        }

        public int ItemId { get; set; }

        public string Ean
        {
            get
            {
                return _ean;
            }
            set
            {
                SetProperty(ref _ean, value);
            }
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

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                //calculate the lifespan for a Item
                AverageLifespanDays = Math.Abs((value.Date - DateTime.Now.Date).Days);
                SetProperty(ref _date, value);
                
            }
        }

        public int AverageLifespanDays { get; set; }
        public int Size 
        {
            get
            {
                return _size;
            }
            set
            {
                SetProperty(ref _size, value);
            }
        }

        public string SizeUnit
        {
            get
            {
                return _sizeUnit;
            }
            set
            {
                SetProperty(ref _sizeUnit, value);
            }
        }

        public int DesiredMinimumAmount { get; set; }


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
