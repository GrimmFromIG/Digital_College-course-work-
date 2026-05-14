using System;

namespace DigitalCollege.BLL.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message) { }
    }
}