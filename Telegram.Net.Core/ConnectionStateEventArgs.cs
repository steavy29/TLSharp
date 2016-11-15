using System;

namespace Telegram.Net.Core
{
    public class ConnectionStateEventArgs : EventArgs
    {
        public readonly bool isConnected;
        public readonly int reconnectAttemptInSeconds;

        public ConnectionStateEventArgs(bool isConnected, int reconnectAttemptInSeconds)
        {
            this.isConnected = isConnected;
            this.reconnectAttemptInSeconds = reconnectAttemptInSeconds;
        }

        public static ConnectionStateEventArgs Connected()
        {
            return new ConnectionStateEventArgs(true, -1);
        }

        public static ConnectionStateEventArgs Disconnected(int reconnectAttemptInSeconds)
        {
            return new ConnectionStateEventArgs(false, reconnectAttemptInSeconds);
        }
    }
}