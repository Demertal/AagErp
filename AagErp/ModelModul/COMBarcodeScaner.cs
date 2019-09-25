using System;
using System.IO.Ports;
using System.Windows;

namespace ModelModul
{
    /// <summary>
    /// Класс подключения сканера штрихкода по интерфейсу RS232
    /// </summary>
    public class COMBarcodeScaner : BarcodeScaner, IBarcodeScaner
    {
        protected SerialPort Port;
        /// <summary>
        /// Коструктор
        /// Открывает COM1
        /// </summary>
        public COMBarcodeScaner()
        {
            Port = new SerialPort("COM4");
            Initialize();
        }

        /// <summary>
        /// Коструктор
        /// </summary>
        /// <param name="portName">Имя порта</param>
        public COMBarcodeScaner(string portName)
        {
            Port = new SerialPort(portName);
            Initialize();
        }

        /// <summary>
        /// Подготовка параметров
        /// </summary>
        protected void Initialize()
        {
            Suffix = new[] { 13, 10 };
        }

        /// <summary>
        /// Подключение к источнику данных
        /// </summary>
        /// <returns>True в случае успеха</returns>
        public new bool Connect()
        {
            // Соединение
            try
            {
                Port.Open();
                Port.DataReceived += Port_DataReceived;
                _connect = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошбика подключения сканера", MessageBoxButton.OK, MessageBoxImage.Error);
                _connect = false;
            }
            return _connect;
        }


        /// <summary>
        /// Отключение от источника данных
        /// </summary>
        /// <returns>True в случае успеха</returns>
        public new bool Disconnect()
        {
            // Отсоединение
            try
            {
                Port.Close();
                Port.DataReceived -= Port_DataReceived;
                _connect = false;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошбика подключения сканера", MessageBoxButton.OK, MessageBoxImage.Error);
                _connect = Port.IsOpen;
                return false;
            }
        }

        /// <summary>
        /// Обработка поступающих данных
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="e">Параметры события</param>
        protected void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_connect) return;
            string code = Port.ReadExisting();
            foreach (char data in code)
                Read(data);
        }
    }
}
