using System.Net;
using System.Net.NetworkInformation;
using WakeOnLan.Library.Extensions;

namespace WakeOnLan.Tests
{
    [TestClass]
    public class LibraryTests
    {
        [TestMethod]
        public void TestGetMagicPacket()
        {
            // 取得できるはずのByte列
            List<byte> _exp = new() { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            // MACアドレス
            List<byte> _address = new() { 0x00, 0x11, 0x12, 0xAA, 0xBB, 0xFF };
            // MACアドレスを16回追加
            for (int i = 0; i < 16 ; i++) { _exp.AddRange(_address); }

            // メソッドでByte列取得
            ReadOnlySpan<byte> _act = Library.WakeOnLan.GetMagicPacket(new PhysicalAddress(_address.ToArray()));

            // 要素数が同じことを確認
            Assert.AreEqual(_exp.Count, _act.Length);

            // 中身の確認
            for (int i = 0; i < _act.Length; i++)
            {
                Assert.AreEqual(_exp[i], _act[i]);
            }
        }

        [TestMethod]
        public void TestGetSubnetMaskAddress()
        {
            string _act = IPAddressExtension.GetSubnetMaskAddress(24).ToString();
            Assert.AreEqual("255.255.255.0", _act);
        }

        [TestMethod]
        public void TestGetBroadCastAddress()
        {
            string _act = IPAddressExtension.GetBroadCastAddress(
                IPAddress.Parse("172.16.0.1"), 24).ToString();
            Assert.AreEqual("172.16.0.255", _act);
        }
    }
}