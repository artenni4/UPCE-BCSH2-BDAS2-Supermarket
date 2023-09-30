using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Common
{
    /// <summary>
    /// Base exception for all custom exceptions in Core layer
    /// </summary>
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string? message) : base(message)
        {
        }

        public CoreException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
