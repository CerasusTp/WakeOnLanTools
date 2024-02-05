using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using WakeOnLan.Console.Models;
using WakeOnLan.Console.Extensions;
using WakeOnLan.Library.Extensions;
using System.Net.Sockets;
using Reactive.Bindings;
using System.Windows.Forms;

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

        // 宛先選択モード
        private bool _IsManual;
        public bool IsManual { get => _IsManual; set => SetProperty(ref _IsManual, value); }

        public MainView()
        {
            InitializeComponent();
            // 初期設定
            Init();
            // バインドする
            SetBinding();
            // マニュアルモード判定
            CheckIsManual();
        }

        // 初期情報設定
        public void Init()
        {
            cmbConnection.DisplayMember = nameof(ConnectionItem.Display);
            cmbConnection.ValueMember = nameof(ConnectionItem.IPAddress);
        }

        // FormControlとプロパティをバインド
        private void SetBinding()
        {
            cmbConnection.Bind(nameof(cmbConnection.DataSource), this, nameof(Connections));

            // 表示切り替え
            btnLocal.Bind(nameof(btnLocal.Clicked), this, nameof(IsLocal)).SetConveter(x => !(bool)x);
            btnRemote.Bind(nameof(btnRemote.Clicked), this, nameof(IsLocal));
            txtIPAddress.Bind(nameof(txtIPAddress.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtSubnetMask.Bind(nameof(txtSubnetMask.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtHostName.Bind(nameof(txtHostName.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            btnHostName.Bind(nameof(btnHostName.Enabled), this, nameof(IsManual));
        }

        // エラーチェック
        private bool CheckHasError()
        {
            // エラー1つでもあればtrue
            bool _hasError = false;

            if (!PhysicalAddress.TryParse(txtMacAddress.Text, out _))
                AddError(txtMacAddress, "MACアドレスが不正です", ref _hasError);
            if (IsManual)
            {
                if (!IPAddress.TryParse(txtIPAddress.Text, out _))
                    AddError(txtIPAddress, "IPv4が不正です", ref _hasError);
                if (int.TryParse(txtSubnetMask.Text, out int _mask))
                {
                    if (_mask < 1 || 32 < _mask)
                        AddError(txtSubnetMask, "サブネットマスクが範囲外です（1-32）", ref _hasError);
                }
                else
                {
                    AddError(txtSubnetMask, "サブネットマスクが不正です", ref _hasError);
                }
            }
            return _hasError;
        }

        // エラー追加
        private void AddError(Control _control, string _message)
        {
            MainViewError.SetError(_control, _message);
            ShowErrorMessage(_message);
        }

        private void AddError(Control _control, string _message, ref bool _hasError)
        {
            AddError(_control, _message);
            _hasError = true;
        }

        // エラーメッセージボックス表示
        private void ShowErrorMessage(string _message) =>
            MessageBox.Show(this, _message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // マニュアルモード判定
        private void CheckIsManual()
        {
            // エラー情報クリア
            MainViewError.Clear();
            IsManual = cmbConnection.SelectedValue == null;
        }

        // ボタン処理
        // モード切り替えボタンのクリック処理
        private void SwitchModeButton(object sender, EventArgs e) => IsLocal = !IsLocal;

        // ホスト名からIP取得処理
        private void GetIPAddressButton(object sender, EventArgs e)
        {
            // エラー情報クリア
            MainViewError.Clear();
            // ホスト名が入力されているかチェック
            if (string.IsNullOrEmpty(txtHostName.Text))
            {
                AddError(txtHostName, "ホスト名を入力してください");
                return;
            }
            // 取得したアドレスを格納
            IPAddress[] _address;
            try
            {
                _address = Dns.GetHostEntry(txtHostName.Text).AddressList.ToList()
                                .Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
            }
            catch (SocketException)
            {
                ShowErrorMessage("不明なホスト名です");
                return;
            }
            if (_address.Length == 0)
                AddError(txtIPAddress, "IPv4アドレスが存在しません");
            else if (_address.Length != 1)
                AddError(txtIPAddress, "同ホスト名でIPv4アドレスが複数存在します");
            else 
                txtIPAddress.Text = _address[0].ToString();
        }

        // 送信
        private void ClickBootButton(object sender, EventArgs e)
        {
            // エラー情報クリア
            MainViewError.Clear();
            // エラーチェック
            if (CheckHasError())
            {
                ShowErrorMessage("設定された値に不正な値があります");
                return;
            }

            // WakeOnLan送信
            if (cmbConnection.SelectedValue is null)
            {
                Library.WakeOnLan.Boot(
                    PhysicalAddress.Parse(txtMacAddress.Text),
                    IPAddressExtension.GetBroadCastAddress(IPAddress.Parse(txtIPAddress.Text), int.Parse(txtSubnetMask.Text)));
            }
            else
            {
                Library.WakeOnLan.Boot(
                    PhysicalAddress.Parse(txtMacAddress.Text), (IPAddress)cmbConnection.SelectedValue);
            }
        }

        // ネットワークのコンボボックスが変更されたとき
        private void ChangeConnectionCombobox(object sender, EventArgs e) =>
            CheckIsManual();
    }
}
