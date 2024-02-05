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

        // ����I�����[�h
        private bool _IsManual;
        public bool IsManual { get => _IsManual; set => SetProperty(ref _IsManual, value); }

        public MainView()
        {
            InitializeComponent();
            // �����ݒ�
            Init();
            // �o�C���h����
            SetBinding();
            // �}�j���A�����[�h����
            CheckIsManual();
        }

        // �������ݒ�
        public void Init()
        {
            cmbConnection.DisplayMember = nameof(ConnectionItem.Display);
            cmbConnection.ValueMember = nameof(ConnectionItem.IPAddress);
        }

        // FormControl�ƃv���p�e�B���o�C���h
        private void SetBinding()
        {
            cmbConnection.Bind(nameof(cmbConnection.DataSource), this, nameof(Connections));

            // �\���؂�ւ�
            btnLocal.Bind(nameof(btnLocal.Clicked), this, nameof(IsLocal)).SetConveter(x => !(bool)x);
            btnRemote.Bind(nameof(btnRemote.Clicked), this, nameof(IsLocal));
            txtIPAddress.Bind(nameof(txtIPAddress.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtSubnetMask.Bind(nameof(txtSubnetMask.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            txtHostName.Bind(nameof(txtHostName.ReadOnly), this, nameof(IsManual)).SetConveter(x => !(bool)x);
            btnHostName.Bind(nameof(btnHostName.Enabled), this, nameof(IsManual));
        }

        // �G���[�`�F�b�N
        private bool CheckHasError()
        {
            // �G���[1�ł������true
            bool _hasError = false;

            if (!PhysicalAddress.TryParse(txtMacAddress.Text, out _))
                AddError(txtMacAddress, "MAC�A�h���X���s���ł�", ref _hasError);
            if (IsManual)
            {
                if (!IPAddress.TryParse(txtIPAddress.Text, out _))
                    AddError(txtIPAddress, "IPv4���s���ł�", ref _hasError);
                if (int.TryParse(txtSubnetMask.Text, out int _mask))
                {
                    if (_mask < 1 || 32 < _mask)
                        AddError(txtSubnetMask, "�T�u�l�b�g�}�X�N���͈͊O�ł��i1-32�j", ref _hasError);
                }
                else
                {
                    AddError(txtSubnetMask, "�T�u�l�b�g�}�X�N���s���ł�", ref _hasError);
                }
            }
            return _hasError;
        }

        // �G���[�ǉ�
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

        // �G���[���b�Z�[�W�{�b�N�X�\��
        private void ShowErrorMessage(string _message) =>
            MessageBox.Show(this, _message, "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // �}�j���A�����[�h����
        private void CheckIsManual()
        {
            // �G���[���N���A
            MainViewError.Clear();
            IsManual = cmbConnection.SelectedValue == null;
        }

        // �{�^������
        // ���[�h�؂�ւ��{�^���̃N���b�N����
        private void SwitchModeButton(object sender, EventArgs e) => IsLocal = !IsLocal;

        // �z�X�g������IP�擾����
        private void GetIPAddressButton(object sender, EventArgs e)
        {
            // �G���[���N���A
            MainViewError.Clear();
            // �z�X�g�������͂���Ă��邩�`�F�b�N
            if (string.IsNullOrEmpty(txtHostName.Text))
            {
                AddError(txtHostName, "�z�X�g������͂��Ă�������");
                return;
            }
            // �擾�����A�h���X���i�[
            IPAddress[] _address;
            try
            {
                _address = Dns.GetHostEntry(txtHostName.Text).AddressList.ToList()
                                .Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
            }
            catch (SocketException)
            {
                ShowErrorMessage("�s���ȃz�X�g���ł�");
                return;
            }
            if (_address.Length == 0)
                AddError(txtIPAddress, "IPv4�A�h���X�����݂��܂���");
            else if (_address.Length != 1)
                AddError(txtIPAddress, "���z�X�g����IPv4�A�h���X���������݂��܂�");
            else 
                txtIPAddress.Text = _address[0].ToString();
        }

        // ���M
        private void ClickBootButton(object sender, EventArgs e)
        {
            // �G���[���N���A
            MainViewError.Clear();
            // �G���[�`�F�b�N
            if (CheckHasError())
            {
                ShowErrorMessage("�ݒ肳�ꂽ�l�ɕs���Ȓl������܂�");
                return;
            }

            // WakeOnLan���M
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

        // �l�b�g���[�N�̃R���{�{�b�N�X���ύX���ꂽ�Ƃ�
        private void ChangeConnectionCombobox(object sender, EventArgs e) =>
            CheckIsManual();
    }
}
