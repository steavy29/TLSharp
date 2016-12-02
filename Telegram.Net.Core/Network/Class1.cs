using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Requests;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Network
{
    class PersistentProto : IDisposable
    {
        private static int connectionReinitializationTimeoutSeconds = 8;

        private MtProtoSender protoSender;

        private readonly int apiId;
        private readonly DeviceInfo deviceInfo;

        private readonly Session session;

        private int isReconnecting = 0;
        private bool isClosed;

        public PersistentProto(Session session, int apiId, DeviceInfo deviceInfo)
        {
            this.session = session;
            this.apiId = apiId;
            this.deviceInfo = deviceInfo;
        }

        public string dcServerAddress => protoSender.dcServerAddress;

        public event EventHandler<Updates> UpdateMessage;
        public event EventHandler<ConnectionStateEventArgs> ConnectionStateChanged;

        public async Task<bool> Start()
        {
            try
            {
                await ReconnectImpl();
                return true;
            }
            catch (Exception)
            {
                StartReconnecting().IgnoreAwait();
                return false;
            }
        }

        public Task Send(MtProtoRequest request) => protoSender.Send(request);

        public void Dispose()
        {
            isClosed = true;
            DisposeProto();
        }

        private async Task ReconnectImpl()
        {
            DisposeProto();

            protoSender = new MtProtoSender(session, TLObject.apiLayer, apiId, deviceInfo);

            protoSender.UpdateMessage += OnUpdateMessage;
            protoSender.Broken += OnBroken;

            await protoSender.Start();

            //var initializationRequest = new SetLayerAndInitConnectionQuery(apiLayer, apiId, deviceInfo.deviceModel, deviceInfo.systemVersion, deviceInfo.appVersion, deviceInfo.langCode);
            //await SendRpcRequest(initializationRequest);
            //dcOptions = new DcOptionsCollection(initializationRequest.config.Cast<ConfigConstructor>().dcOptions);

            OnConnectionStateChanged(ConnectionStateEventArgs.Connected());
        }

        private async Task StartReconnecting()
        {
            if (Interlocked.Exchange(ref isReconnecting, 1) == 1)
                return;

            while (!isClosed)
            {
                try
                {
                    await ReconnectImpl();
                    break;
                }
                catch (Exception ex)
                {
                    DisposeProto();

                    Debug.WriteLine($"Failed to initialize connection: {ex.Message}");
                    Debug.WriteLine($"Retrying in {connectionReinitializationTimeoutSeconds} seconds..");

                    OnConnectionStateChanged(ConnectionStateEventArgs.Disconnected(connectionReinitializationTimeoutSeconds));
                    await Task.Delay(TimeSpan.FromSeconds(connectionReinitializationTimeoutSeconds));
                }
            }

            isReconnecting = 0;
        }
        
        private void DisposeProto()
        {
            if (protoSender != null)
            {
                Debug.WriteLine("Closing current transport");

                protoSender.UpdateMessage -= OnUpdateMessage;
                protoSender.Broken -= OnBroken;
                protoSender.Dispose();
            }
        }

        private void OnUpdateMessage(object sender, Updates e)
        {
            UpdateMessage?.Invoke(this, e);
        }
        private void OnBroken(object sender, EventArgs e)
        {
            StartReconnecting().IgnoreAwait(); // no await
        }
        private void OnConnectionStateChanged(ConnectionStateEventArgs e)
        {
            Debug.WriteLine($"Connection status: {(e.isConnected ? "connected" : "disconnected")}");
            ConnectionStateChanged?.Invoke(this, e);
        }
    }
}
