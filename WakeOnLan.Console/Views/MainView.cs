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
        // �ύX�ʒm�C�x���g
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? _propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName));

        // �v���p�e�B�Z�b�^�[
        protected void SetProperty<T>(ref T _target, T _value, [CallerMemberName] string? _propertyName = null)
        {
            if (Equals(_target, _value)) return;
            _target = _value;
            OnPropertyChanged(_propertyName);
        }

        // �v���p�e�B
        // ���[�h
        private bool _IsLocal;
        public bool IsLocal { get => _IsLocal; set => SetProperty(ref _IsLocal, value); }

        // �ڑ���ꗗ
        private List<ConnectionItem> _Connections = ConnectionItem.GetConnections();
        public List<ConnectionItem> Connections { get => _Connections; set => SetProperty(ref _Connections, value); }

        // �I�����ꂽ�ڑ���
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

        // MAC�A�h���X
        private string? _StrMACAddress;
        public string? StrMACAddress { get => _StrMACAddress; set => SetProperty(ref _StrMACAddress, value); }

        // ����IP�A�h���X
        private string? _StrIPAddress;
        public string? StrIPAddress { get => _StrIPAddress; set => SetProperty(ref _StrIPAddress, value); }

        // ����T�u�l�b�g�}�X�N
        private string? _StrSubnetMask;
        public string? StrSubnetMask { get => _StrSubnetMask; set => SetProperty(ref _StrSubnetMask, value); }

        // �蓮����
        public bool IsManual { get => SelectedConnection is null || SelectedConnection.IPAddress is null; }
        
        // ���M�A�h���X
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
            // �����ݒ�
            Init();
            // �o�C���h����
            SetBinding();
        }

        // �������ݒ�
        public void Init()
        {
            cmbConnection.DisplayMember = nameof(ConnectionItem.Display);
        }

        // FormControl�ƃv���p�e�B���o�C���h
        private void SetBinding()
        {
            txtMacAddress.Bind(nameof(txtMacAddress.Text), this, nameof(StrMACAddress));
            txtIPAddress.Bind(nameof(txtIPAddress.Text), this, nameof(StrIPAddress));
            txtSubnetMask.Bind(nameof(txtSubnetMask.Text), this, nameof(StrSubnetMask));
            cmbConnection.Bind(nameof(cmbConnection.DataSource), this, nameof(Connections));
            cmbConnection.Bind(nameof(cmbConnection.SelectedItem), this, nameof(SelectedConnection));

            // �\���؂�ւ�
            btnLocal.Bind(nameof(btnLocal.Clicked), this, nameof(IsLocal)).SetConveter(x => !(bool)x);
            btnRemote.Bind(nameof(btnRemote.Clicked), this, nameof(IsLocal));
            txtIPAddress.Bind(nameof(txtIPAddress.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtSubnetMask.Bind(nameof(txtSubnetMask.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtHostName.Bind(nameof(txtHostName.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
        }

        // ���M
        private void ClickBootButton(object sender, EventArgs e)
        {
            // MAC�A�h���X�`�F�b�N
            if (MACAddress is null)
            {
                ShowErrorMsg("MAC�A�h���X���s���ł�");
                return;
            }

            if (IsManual)
            {
                if (Address is null || SubnetMask is null)
                {
                    ShowErrorMsg("IPv4�A�h���X���T�u�l�b�g�}�X�N�̌`�����s���ł�");
                    return;
                }
                Library.WakeOnLan.Boot(MACAddress, IPAddressExtension.GetBroadCastAddress(Address, (int)SubnetMask));
            }
            else
            {
                Library.WakeOnLan.Boot(MACAddress, IPAddressExtension.GetBroadCastAddress(SelectedConnection.IPAddress, (int)SubnetMask));
            }
        }

        // �G���[���b�Z�[�W�\��
        private static void ShowErrorMsg(string _message) =>
            MessageBox.Show(_message, "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // ���[�h�؂�ւ��{�^���̃N���b�N����
        private void SwitchModeButton(object sender, EventArgs e) => IsLocal = !IsLocal;
    }
}
