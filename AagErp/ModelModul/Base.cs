namespace ModelModul
{
    #region IBarcodeScaner
    /// <summary>
    /// Обработчик события получения штрих кода
    /// </summary>
    /// <param name="barcode">Штрих код</param>
    public delegate void BarcodeHandler(string barcode);
    #endregion

    #region IBarcodeScaner
    /// <summary>
    /// Интерфейс устройства
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
    /// Набор статусов поиска кода
    /// </summary>
    public enum BarcodeReadStatus { Wait, ReadPrefix, ReadBarcode, ReadSuffix };
    #endregion

    #region BarcodeScaner
    /// <summary>
    /// Базовый класс для сканеров
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

        // включить
        public bool Connect()
        {
            _connect = true;
            return true;
        }

        // выключить
        public bool Disconnect()
        {
            _connect = false;
            return true;
        }

        //// префикс
        public int[] Prefix
        {
            get => _prefix;
            set => _prefix = value;
        }

        // постфикс
        public int[] Suffix
        {
            get => _suffix;
            set => _suffix = value;
        }

        // сбросить настройки
        protected void Reset()
        {
            _prefixIndex = 0;
            _suffixIndex = 0;
            _barcode = "";
            _status = BarcodeReadStatus.Wait;
        }

        /// <summary>
        /// Посимвольный анализ
        /// Поиск сочетаний суффикса и префикса, передача полученного кода
        /// </summary>
        /// <param name="data">Код символа</param>
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
        /// Обработка непосредственно штрих-кода
        /// Сюда проверки писать
        /// </summary>
        /// <param name="data">Байт информации</param>
        /// <returns>True в случае успеха</returns>
        protected bool ReadBarcode(int data)
        {
            //проверка кода символа, только цифры 48-57
            if (data < 48 || data > 57)
                return false;
            // проверка длинны, до 13 символов 
            if (_barcode.Length + 1 > 13)
                return false;
            _barcode += (char)data;
            return true;
        }

        /// <summary>
        /// Отправка штрих-кода
        /// </summary>
        protected void SendBarcode()
        {
            GetBarcode?.Invoke(_barcode);
        }
    }
}

    #endregion