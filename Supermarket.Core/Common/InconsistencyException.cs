using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Common
{
    internal class InconsistencyException : CoreException
    {
        public InconsistencyException()
        {
        }

        public InconsistencyException(string? message) : base(message)
        {
        }

        public InconsistencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
