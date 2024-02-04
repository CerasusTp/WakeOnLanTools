using System.Net;
using System.Net.NetworkInformation;

namespace WakeOnLan.Library.Extensions
{
    public static class IPAddressExtension
    {
        // ブロードキャストアドレス生成
        public static IPAddress GetBroadCastAddress(IPAddress _ip, IPAddress _subnet) =>
            GetBroadCastAddress(_ip.GetAddressBytes(), _subnet.GetAddressBytes());

        public static IPAddress GetBroadCastAddress(IPAddress _ip, int _mask) =>
            GetBroadCastAddress(_ip.GetAddressBytes(), GetSubnetMaskAddress(_mask).GetAddressBytes());

        public static IPAddress GetBroadCastAddress(byte[] _ip, byte[] _subnet)
        {
            // ブロードキャストアドレスのByte配列を生成
            byte[] _broadCast =
                Enumerable.Range(0, 4).ToList().Select(x => (byte)(_ip[x] | ~_subnet[x])).ToArray();

            return new IPAddress(_broadCast);
        }

        // サブネットマスクアドレス生成
        public static IPAddress GetSubnetMaskAddress(int _mask) =>
            new(BitConverter.GetBytes(Convert.ToUInt32(string.Concat(new string('0', 32 -_mask), new string('1', _mask)), 2)));

        // ローカルのIPv4接続一覧取得
        public static List<UnicastIPAddressInformation> GetLocalIPv4()
        {
            // 結果
            List<UnicastIPAddressInformation> _list = [];
            NetworkInterface.GetAllNetworkInterfaces()
                .Where(x =>
                    x.OperationalStatus.Equals(OperationalStatus.Up) &&
                    x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    x.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                .ToList()
                .ForEach(x => _list.AddRange(x.GetIPProperties().UnicastAddresses));
            return _list;
        }

    }
}
