using System;

namespace ConsoleAppClient.Utilities
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }
}
