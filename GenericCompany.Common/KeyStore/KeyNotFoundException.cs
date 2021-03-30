using System;

namespace GenericCompany.Common.KeyStore
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string message) : base(message)
        {
        }
    }
}
