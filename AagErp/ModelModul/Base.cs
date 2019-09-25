namespace ModelModul
{
    #region IBarcodeScaner
    /// <summary>
    /// ���������� ������� ��������� ����� ����
    /// </summary>
    /// <param name="barcode">����� ���</param>
    public delegate void BarcodeHandler(string barcode);
    #endregion

    #region IBarcodeScaner
    /// <summary>
    /// ��������� ����������
    /// </summary>
    public interface IBarcodeScaner
    {
        bool Connect();
        bool Disconnect();
        event BarcodeHandler GetBarcode;
    }
    #endregion

    #region BarcodeReadStatus
    /// <summary>
    /// ����� �������� ������ ����
    /// </summary>
    public enum BarcodeReadStatus { Wait, ReadPrefix, ReadBarcode, ReadSuffix };
    #endregion

    #region BarcodeScaner
    /// <summary>
    /// ������� ����� ��� ��������
    /// </summary>
    public abstract class BarcodeScaner
    {
        private int[] _prefix;
        private int _prefixIndex;
        private int[] _suffix;
        private int _suffixIndex;
        private string _barcode;
        protected bool _connect;
        private BarcodeReadStatus _status;
        public event BarcodeHandler GetBarcode;

        protected BarcodeScaner()
        {
            _prefix = null;
            _suffix = null;
            _connect = false;
            Reset();
        }

        // ��������
        public bool Connect()
        {
            _connect = true;
            return true;
        }

        // ���������
        public bool Disconnect()
        {
            _connect = false;
            return true;
        }

        //// �������
        public int[] Prefix
        {
            get => _prefix;
            set => _prefix = value;
        }

        // ��������
        public int[] Suffix
        {
            get => _suffix;
            set => _suffix = value;
        }

        // �������� ���������
        protected void Reset()
        {
            _prefixIndex = 0;
            _suffixIndex = 0;
            _barcode = "";
            _status = BarcodeReadStatus.Wait;
        }

        /// <summary>
        /// ������������ ������
        /// ����� ��������� �������� � ��������, �������� ����������� ����
        /// </summary>
        /// <param name="data">��� �������</param>
        protected void Read(int data)
        {
            if (!_connect)
            {
                Reset();
                return;
            }
            switch (_status)
            {
                case BarcodeReadStatus.Wait:
                    _status = _prefix == null ? BarcodeReadStatus.ReadBarcode : BarcodeReadStatus.ReadPrefix;
                    Read(data);
                    break;
                case BarcodeReadStatus.ReadPrefix:
                    if (_prefix == null)
                    {
                        _status = BarcodeReadStatus.ReadBarcode;
                        Read(data);
                    }
                    else if (_prefix[_prefixIndex] == data)
                    {
                        _prefixIndex++;
                        if (_prefixIndex == _prefix.Length)
                            _status = BarcodeReadStatus.ReadBarcode;
                    }
                    else Reset();
                    break;
                case BarcodeReadStatus.ReadBarcode:
                    if (_suffix[_suffixIndex] == data)
                    {
                        _status = BarcodeReadStatus.ReadSuffix;
                        Read(data);
                    }
                    else if (!ReadBarcode(data)) Reset();
                    break;
                case BarcodeReadStatus.ReadSuffix:
                    if (_suffix[_suffixIndex] == data)
                    {
                        if (_suffixIndex == _suffix.Length - 1)
                        {
                            SendBarcode();
                            Reset();
                        }
                        else
                            _suffixIndex++;
                    }
                    else
                        Reset();
                    break;
            }
        }

        /// <summary>
        /// ��������� ��������������� �����-����
        /// ���� �������� ������
        /// </summary>
        /// <param name="data">���� ����������</param>
        /// <returns>True � ������ ������</returns>
        protected bool ReadBarcode(int data)
        {
            //�������� ���� �������, ������ ����� 48-57
            if (data < 48 || data > 57)
                return false;
            // �������� ������, �� 13 �������� 
            if (_barcode.Length + 1 > 13)
                return false;
            _barcode += (char)data;
            return true;
        }

        /// <summary>
        /// �������� �����-����
        /// </summary>
        protected void SendBarcode()
        {
            GetBarcode?.Invoke(_barcode);
        }
    }
}

    #endregion