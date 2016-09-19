using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public abstract class MTProtoRequest
    {
        protected MTProtoRequest()
        {
            Sended = false;
        }

        public RpcRequestError Error { get; private set; }
        public string ErrorMessage { get; private set; }

        public long MessageId { get; set; }
        public int Sequence { get; set; }

        public bool Dirty { get; set; }

        public bool Sended { get; private set; }
        public DateTime SendTime { get; private set; }
        public bool ConfirmReceived { get; set; }
        public abstract void OnSend(BinaryWriter writer);
        public abstract void OnResponse(BinaryReader reader);
        public abstract void OnException(Exception exception);
        public virtual void OnError(int errorCode, string errorMessage)
        {
            Error = (RpcRequestError)errorCode;
            ErrorMessage = errorMessage;
        }
        public abstract bool Confirmed { get; }
        public abstract bool Responded { get; }

        public virtual void OnSendSuccess()
        {
            SendTime = DateTime.Now;
            Sended = true;
        }

        public virtual void OnConfirm()
        {
            ConfirmReceived = true;
        }

        public bool NeedResend => Dirty || (Confirmed && !ConfirmReceived && DateTime.Now - SendTime > TimeSpan.FromSeconds(3));

        public void ResetError()
        {
            Error = RpcRequestError.None;
            ErrorMessage = null;
        }
    }
}
