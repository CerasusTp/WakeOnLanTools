using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WakeOnLan.Library
{
    public static class WakeOnLan
    {
        public static int Boot(PhysicalAddress _address, IPEndPoint _endpoint)
        {
            // マジックパケットを送信
            return new UdpClient().Send(GetMagicPacket(_address), _endpoint);
        }

        public static int Boot(PhysicalAddress _address, string _hostname, int _port)
        {
            // マジックパケットを送信
            return new UdpClient().Send(GetMagicPacket(_address), _hostname, _port);
        }

        public static int Boot(PhysicalAddress _address, IPAddress _ip, int _port = 9) => Boot(_address, new IPEndPoint(_ip, _port));

        // マジックパケット生成
        public static ReadOnlySpan<byte> GetMagicPacket(PhysicalAddress _address)
        {
            // マジックパケット配列
            byte[] _packet = new byte[102];
            // マジックパケットの先頭6バイトにFFを追加
            Enumerable.Range(0, 6).ToList().ForEach(x => _packet[x] = 0xFF);

            // MACアドレスのバイト列を取得
            byte[] _bytes_address = _address.GetAddressBytes();

            // MACアドレスをバイトに変換し16回繰り返す
            for (int i = 1; i <= 16; ++i)
            {
                Array.Copy(_bytes_address, 0, _packet, i * _bytes_address.Length, _bytes_address.Length);
            }

            return _packet;
        }
    }
}
