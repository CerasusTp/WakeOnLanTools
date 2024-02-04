using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

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

        public static List<ConnectionItem> GetLocalIPv4Connection()
        {
            // 手動設定を追加
            List<ConnectionItem> _list = [new ConnectionItem("手動", null)];
            _list.AddRange(Dns.GetHostAddresses(Dns.GetHostName())
                .Where(x => x.AddressFamily.Equals(AddressFamily.InterNetwork))
                .Select(x => new ConnectionItem(x.ToString(), x))
                .ToList());
            return _list;
        }
    }
}
