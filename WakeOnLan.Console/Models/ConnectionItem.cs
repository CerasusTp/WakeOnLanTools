using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using WakeOnLan.Library.Extensions;

namespace WakeOnLan.Console.Models
{
    public class ConnectionItem
    {
        public string Display {  get; set; }
        public IPAddress? IPAddress { get; set; }

        public ConnectionItem(string _display, IPAddress? _address)
        {
            Display = _display;
            IPAddress = _address;
        }

        public static List<ConnectionItem> GetConnections()
        {
            // 手動設定を追加
            List<ConnectionItem> _list = [new ConnectionItem("手動", null)];
            _list.AddRange(IPAddressExtension.GetLocalIPv4()
                .Select(x => new ConnectionItem(
                    $"{x.Address}/{x.PrefixLength}",IPAddressExtension.GetBroadCastAddress(x.Address, x.IPv4Mask))));
            return _list;
        }
    }
}
