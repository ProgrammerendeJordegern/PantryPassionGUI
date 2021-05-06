using System;

namespace PantryPassionGUI.Utilities
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
    }
}
