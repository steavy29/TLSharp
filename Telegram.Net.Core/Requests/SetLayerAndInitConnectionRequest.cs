using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public abstract class QueryWrapper<TRequestType> : MtProtoRequest where TRequestType: MtProtoRequest
    {
        public TRequestType innerRequest;

        protected QueryWrapper(TRequestType innerRequest)
        {
            this.innerRequest = innerRequest;
        }

        public sealed override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            SerializeRequest(writer);
            innerRequest.OnSend(writer);
        }
        public sealed override void OnResponse(BinaryReader reader)
        {
            innerRequest.OnResponse(reader);
        }

        protected abstract void SerializeRequest(BinaryWriter writer);
    }
    public class InvokeWithLayerQueryWrapper<TRequestType> : QueryWrapper<TRequestType> where TRequestType : MtProtoRequest
    {
        private readonly int layer;

        public InvokeWithLayerQueryWrapper(int layer, TRequestType innerRequest) : base(innerRequest)
        {
            this.layer = layer;
        }

        protected override uint requestCode => 0xda9b0d0d;

        protected override void SerializeRequest(BinaryWriter writer)
        {
            writer.Write(layer);
        }
    }

    public class InitConnectionQueryWrapper<TRequestType> : QueryWrapper<TRequestType> where TRequestType : MtProtoRequest
    {
        private readonly int apiId;
        private readonly string deviceModel;
        private readonly string systemVersion;
        private readonly string appVersion;
        private readonly string langCode;

        public InitConnectionQueryWrapper(int apiId, string deviceModel, string systemVersion, string appVersion, string langCode, TRequestType innerRequest) : base(innerRequest)
        {
            this.apiId = apiId;
            this.deviceModel = deviceModel;
            this.systemVersion = systemVersion;
            this.appVersion = appVersion;
            this.langCode = langCode;
        }

        protected override uint requestCode => 0x69796de9;

        protected override void SerializeRequest(BinaryWriter writer)
        {
            writer.Write(apiId);
            Serializers.String.Write(writer, deviceModel);
            Serializers.String.Write(writer, systemVersion);
            Serializers.String.Write(writer, appVersion);
            Serializers.String.Write(writer, langCode);
        }
    }

    public class SetLayerAndInitConnectionQuery : MtProtoRequest
    {
        private readonly InvokeWithLayerQueryWrapper<InitConnectionQueryWrapper<GetConfigRequest>> wrappedQuery;

        public ConfigConstructor config => wrappedQuery.innerRequest.innerRequest.config.Cast<ConfigConstructor>();

        public SetLayerAndInitConnectionQuery(int layer, int apiId, string deviceModel, string systemVersion, string appVersion, string langCode)
        {
            wrappedQuery = new InvokeWithLayerQueryWrapper<InitConnectionQueryWrapper<GetConfigRequest>>(
                layer, 
                new InitConnectionQueryWrapper<GetConfigRequest>(apiId, deviceModel, systemVersion, appVersion, langCode,
                new GetConfigRequest()));
        }

        protected override uint requestCode => 0;

        public override void OnSend(BinaryWriter writer)
        {
            wrappedQuery.OnSend(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            wrappedQuery.OnResponse(reader);
        }
    }
}
