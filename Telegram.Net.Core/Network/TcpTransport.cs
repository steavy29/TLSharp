using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Net.Core.Network
{
    public class TcpTransport : IDisposable
    {
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;
        private int sendCounter;

        private readonly SemaphoreSlim sendLock = new SemaphoreSlim(1);

        public TcpTransport(string address, int port)
        {
            tcpClient = new TcpClient
            {
                LingerState = new LingerOption(true, 1)
            };
            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            var ipAddress = IPAddress.Parse(address);
            tcpClient.Connect(ipAddress, port);
            stream = tcpClient.GetStream();
        }

        public async Task Send(byte[] packet)
        {
            if (!tcpClient.Connected)
                throw new InvalidOperationException("Client not connected to server.");

            var tcpMessage = new TcpMessage(sendCounter, packet);

            try
            {
                await sendLock.WaitAsync();

                await tcpClient.GetStream().WriteAsync(tcpMessage.Encode(), 0, tcpMessage.Encode().Length);
                sendCounter++;
            }
            finally
            {
                sendLock.Release();
            }
        }

        public async Task<TcpMessage> Receieve()
        {
            // packet length
            var packetLengthBytes = new byte[4];
            if (!await ReadBuffer(stream, packetLengthBytes))
                return null;

            int packetLength = BitConverter.ToInt32(packetLengthBytes, 0);

            // seq
            var seqBytes = new byte[4];
            if (!await ReadBuffer(stream, seqBytes))
                return null;

            int seq = BitConverter.ToInt32(seqBytes, 0);

            // body
            var bodyBytes = new byte[packetLength - 12];
            if (!await ReadBuffer(stream, bodyBytes))
                return null;

            // crc
            var crcBytes = new byte[4];
            if (!await ReadBuffer(stream, crcBytes))
                return null;

            int checksum = BitConverter.ToInt32(crcBytes, 0);

            byte[] rv = new byte[packetLengthBytes.Length + seqBytes.Length + bodyBytes.Length];

            Buffer.BlockCopy(packetLengthBytes, 0, rv, 0, packetLengthBytes.Length);
            Buffer.BlockCopy(seqBytes, 0, rv, packetLengthBytes.Length, seqBytes.Length);
            Buffer.BlockCopy(bodyBytes, 0, rv, packetLengthBytes.Length + seqBytes.Length, bodyBytes.Length);
            var crc32 = new Ionic.Crc.CRC32();
            crc32.SlurpBlock(rv, 0, rv.Length);
            var validChecksum = crc32.Crc32Result;

            if (checksum != validChecksum)
            {
                throw new InvalidOperationException("invalid checksum! skip");
            }

            return new TcpMessage(seq, bodyBytes);
        }

        private static async Task<bool> ReadBuffer(NetworkStream stream, byte[] buffer)
        {
            var bytesRead = 0;

            do
            {
                var availableBytes = await stream.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead);
                if (availableBytes == 0)
                {
                    Debug.WriteLine("TcpTransport: read the connection termination 0 packet");
                    return false;
                }

                bytesRead += availableBytes;
            }
            while (bytesRead != buffer.Length);

            return true;
        }

        public void Dispose()
        {
            try
            {
                if (tcpClient.Connected)
                {
                    tcpClient.Client.Disconnect(false);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception on socket dispose: {e}");
            }

            tcpClient.Close();
        }
    }
}
