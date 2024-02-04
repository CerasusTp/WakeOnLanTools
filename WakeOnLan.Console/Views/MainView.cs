using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using WakeOnLan.Console.Models;
using WakeOnLan.Console.Extensions;
using WakeOnLan.Library.Extensions;

namespace WakeOnLan.Console.Views
{
    public partial class MainView : Form, INotifyPropertyChanged
    {
        // 変更通知イベント
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? _propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName));

        // プロパティセッター
        protected void SetProperty<T>(ref T _target, T _value, [CallerMemberName] string? _propertyName = null)
        {
            if (Equals(_target, _value)) return;
            _target = _value;
            OnPropertyChanged(_propertyName);
        }

        // プロパティ
        // モード
        private bool _IsLocal;
        public bool IsLocal { get => _IsLocal; set => SetProperty(ref _IsLocal, value); }

        // 接続先一覧
        private List<ConnectionItem> _Connections = ConnectionItem.GetConnections();
        public List<ConnectionItem> Connections { get => _Connections; set => SetProperty(ref _Connections, value); }

        // 選択された接続先
        private ConnectionItem? _SelectedConnection;
        public ConnectionItem? SelectedConnection 
        { 
            get => _SelectedConnection; 
            set
            {
                SetProperty(ref _SelectedConnection, value);
                OnPropertyChanged(nameof(IsManual));
            }
        }

        // MACアドレス
        private string? _StrMACAddress;
        public string? StrMACAddress { get => _StrMACAddress; set => SetProperty(ref _StrMACAddress, value); }

        // 宛先IPアドレス
        private string? _StrIPAddress;
        public string? StrIPAddress { get => _StrIPAddress; set => SetProperty(ref _StrIPAddress, value); }

        // 宛先サブネットマスク
        private string? _StrSubnetMask;
        public string? StrSubnetMask { get => _StrSubnetMask; set => SetProperty(ref _StrSubnetMask, value); }

        // 手動判定
        public bool IsManual { get => SelectedConnection is null || SelectedConnection.IPAddress is null; }
        
        // 送信アドレス
        public PhysicalAddress? MACAddress
        {
            get
            {
                PhysicalAddress.TryParse(StrMACAddress, out PhysicalAddress? _macAddress);
                return _macAddress;
            }
        }

        public IPAddress? Address
        {
            get
            {
                IPAddress.TryParse(StrIPAddress, out IPAddress? _Address);
                return _Address;
            }
        }

        public int? SubnetMask
        {
            get
            {
                if (int.TryParse(StrSubnetMask, out int _mask) && 1 <= _mask || _mask <= 32)
                {
                    return _mask;
                };
                return null;
            }
        }


        public MainView()
        {
            InitializeComponent();
            // 初期設定
            Init();
            // バインドする
            SetBinding();
        }

        // 初期情報設定
        public void Init()
        {
            cmbConnection.DisplayMember = nameof(ConnectionItem.Display);
        }

        // FormControlとプロパティをバインド
        private void SetBinding()
        {
            txtMacAddress.Bind(nameof(txtMacAddress.Text), this, nameof(StrMACAddress));
            txtIPAddress.Bind(nameof(txtIPAddress.Text), this, nameof(StrIPAddress));
            txtSubnetMask.Bind(nameof(txtSubnetMask.Text), this, nameof(StrSubnetMask));
            cmbConnection.Bind(nameof(cmbConnection.DataSource), this, nameof(Connections));
            cmbConnection.Bind(nameof(cmbConnection.SelectedItem), this, nameof(SelectedConnection));

            // 表示切り替え
            btnLocal.Bind(nameof(btnLocal.Clicked), this, nameof(IsLocal)).SetConveter(x => !(bool)x);
            btnRemote.Bind(nameof(btnRemote.Clicked), this, nameof(IsLocal));
            txtIPAddress.Bind(nameof(txtIPAddress.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtSubnetMask.Bind(nameof(txtSubnetMask.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtHostName.Bind(nameof(txtHostName.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
        }

        // 送信
        private void ClickBootButton(object sender, EventArgs e)
        {
            // MACアドレスチェック
            if (MACAddress is null)
            {
                ShowErrorMsg("MACアドレスが不正です");
                return;
            }

            if (IsManual)
            {
                if (Address is null || SubnetMask is null)
                {
                    ShowErrorMsg("IPv4アドレスかサブネットマスクの形式が不正です");
                    return;
                }
                Library.WakeOnLan.Boot(MACAddress, IPAddressExtension.GetBroadCastAddress(Address, (int)SubnetMask));
            }
            else
            {
                Library.WakeOnLan.Boot(MACAddress, IPAddressExtension.GetBroadCastAddress(SelectedConnection.IPAddress, (int)SubnetMask));
            }
        }

        // エラーメッセージ表示
        private static void ShowErrorMsg(string _message) =>
            MessageBox.Show(_message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // モード切り替えボタンのクリック処理
        private void SwitchModeButton(object sender, EventArgs e) => IsLocal = !IsLocal;
    }
}
