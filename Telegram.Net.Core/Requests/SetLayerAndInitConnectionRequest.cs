using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public abstract class QueryWrapper<TRequestType> : MtProtoRequest where TRequestType : MtProtoRequest
    {
        public TRequestType innerRequest;

        protected QueryWrapper(TRequestType innerRequest)
        {
            this.innerRequest = innerRequest;
        }

        public sealed override void OnResponse(BinaryReader reader)
        {
            innerRequest.OnResponse(reader);
        }
    }
    public class InvokeWithLayerQueryWrapper<TRequestType> : QueryWrapper<TRequestType> where TRequestType : MtProtoRequest
    {
        private readonly int layer;

        public InvokeWithLayerQueryWrapper(int layer, TRequestType innerRequest) : base(innerRequest)
        {
            this.layer = layer;
        }

        protected override uint requestCode => 0xda9b0d0d;
        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            writer.Write(layer);
            innerRequest.OnSend(writer);
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
        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            writer.Write(apiId);
            Serializers.String.Write(writer, deviceModel);
            Serializers.String.Write(writer, systemVersion);
            Serializers.String.Write(writer, appVersion);
            Serializers.String.Write(writer, langCode);
            innerRequest.OnSend(writer);
        }
    }

    public class InvokeWithLayerAndInitConnectionQueryWrapper<TRequestType> : QueryWrapper<TRequestType> where TRequestType : MtProtoRequest
    {
        private readonly InvokeWithLayerQueryWrapper<InitConnectionQueryWrapper<TRequestType>> wrappedQuery;
        public InvokeWithLayerAndInitConnectionQueryWrapper(int layer, int apiId, string deviceModel, string systemVersion, string appVersion, string langCode, TRequestType innerRequest) : base(innerRequest)
        {
            wrappedQuery = new InvokeWithLayerQueryWrapper<InitConnectionQueryWrapper<TRequestType>>(
                layer,
                new InitConnectionQueryWrapper<TRequestType>(apiId, deviceModel, systemVersion, appVersion, langCode, innerRequest));
        }

        protected override uint requestCode => 0;
        public override void OnSend(BinaryWriter writer)
        {
            wrappedQuery.OnSend(writer);
        }
    }

    public class InitConnectionAndGetConfigRequest : MtProtoRequest
    {
        private readonly InvokeWithLayerAndInitConnectionQueryWrapper<GetConfigRequest> wrappedGetConfigRequest;

        public ConfigConstructor config => wrappedGetConfigRequest.innerRequest.config.Cast<ConfigConstructor>();

        public InitConnectionAndGetConfigRequest(int layer, int apiId, DeviceInfo deviceInfo)
        {
            wrappedGetConfigRequest = new InvokeWithLayerAndInitConnectionQueryWrapper<GetConfigRequest>(layer, apiId, deviceInfo.deviceModel, deviceInfo.systemVersion, deviceInfo.appVersion, deviceInfo.langCode, new GetConfigRequest());
        }

        protected override uint requestCode => 0;

        public override void OnSend(BinaryWriter writer)
        {
            wrappedGetConfigRequest.OnSend(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            wrappedGetConfigRequest.OnResponse(reader);
        }
    }
}
