using System;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core
{
    public class TelegramReqestException : Exception
    {
        public readonly RpcRequestError Error;
        public readonly string ErrorMessage;

        public TelegramReqestException(RpcRequestError error, string errorMessage)
        {
            Error = error;
            ErrorMessage = errorMessage;
        }
    }
}