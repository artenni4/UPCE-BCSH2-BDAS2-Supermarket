using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.CashBoxes
{
    public enum AssistantLoginFail
    {
        /// <summary>
        /// In case of bad login data
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// In case when employee does not have required role
        /// </summary>
        PermissionDenied
    }
}
