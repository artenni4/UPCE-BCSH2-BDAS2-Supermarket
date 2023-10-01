using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.CashBoxes
{
    public class AssistantLoginResult
    {
        private AssistantLoginResult(bool isSuccess, AssistantLoginFail? error) 
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public AssistantLoginFail? Error { get; }

        public static AssistantLoginResult Fail(AssistantLoginFail error) => new(false, error);
        public static AssistantLoginResult Success() => new(true, error: null);
    }
}
