using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class SetLayerAndInitConnectionRequest : MTProtoRequest
    {
        private readonly int apiId;
        private readonly int layer;

        public ConfigConstructor config { get; private set; }

        public SetLayerAndInitConnectionRequest(int apiId, int layer)
        {
            this.apiId = apiId;
            this.layer = layer;
        }

        protected override uint requestCode => 0;

        public override void OnSend(BinaryWriter writer)
        {
            // invokeWithLayer request
            writer.Write(0xda9b0d0d);
            writer.Write(layer);

            // initConnection request
            writer.Write(0x69796de9); 
            writer.Write(apiId); 
            Serializers.String.Write(writer, "WinPhone Emulator"); // device model
            Serializers.String.Write(writer, "WinPhone 8.0"); // system version
            Serializers.String.Write(writer, "1.0-SNAPSHOT"); // app version
            Serializers.String.Write(writer, "en"); // lang code

            // getConfig request
            writer.Write(0xc4f9186b); 
        }

        public override void OnResponse(BinaryReader reader)
        {
            config = TLObject.Read<Config>(reader) as ConfigConstructor;
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Responded => true;
        public override bool Confirmed => true;
    }
}
