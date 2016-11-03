using System;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core
{
    public class TelegramReqestException : Exception
    {
        public readonly RpcRequestError error;
        public readonly string errorMessage;

        public TelegramReqestException(RpcRequestError error, string errorMessage) : base($"{error} - {errorMessage}")
        {
            this.error = error;
            this.errorMessage = errorMessage;
        }
    }
}