using System;
using System.Diagnostics;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public abstract class MtProtoRequest
    {
        protected abstract uint requestCode { get; }

        protected MtProtoRequest()
        {
            Sent = false;
        }

        public RpcRequestError Error { get; private set; }
        public string ErrorMessage { get; private set; }

        public long MessageId { get; set; }
        public int Sequence { get; set; }

        public bool Dirty { get; set; }

        public bool Sent { get; private set; }
        public DateTime SendTime { get; private set; }
        public bool ConfirmReceived { get; set; }
        public abstract void OnSend(BinaryWriter writer);
        public abstract void OnResponse(BinaryReader reader);
        public virtual void OnError(int errorCode, string errorMessage)
        {
            Error = (RpcRequestError)errorCode;
            ErrorMessage = errorMessage;
        }

        public virtual bool isContentMessage => true;

        public virtual void OnSendSuccess()
        {
            SendTime = DateTime.Now;
            Sent = true;
        }

        public virtual void OnConfirm()
        {
            ConfirmReceived = true;
        }

        public bool NeedResend => Dirty || (isContentMessage && !ConfirmReceived && DateTime.Now - SendTime > TimeSpan.FromSeconds(3));

        public void ResetError()
        {
            Error = RpcRequestError.None;
            ErrorMessage = null;
        }

        public void ThrowIfHasError()
        {
            if (Error != RpcRequestError.None)
            {
                Debug.WriteLine($"Throwing exception for request {this.GetType().Name} because of error {Error} - {ErrorMessage}");
                throw new TelegramReqestException(Error, ErrorMessage);
            }
        }
    }
}
