using System;
using System.IO.Ports;
using System.Windows;

namespace ModelModul
{
    /// <summary>
    /// ����� ����������� ������� ��������� �� ���������� RS232
    /// </summary>
    public class COMBarcodeScaner : BarcodeScaner, IBarcodeScaner
    {
        protected SerialPort Port;
        /// <summary>
        /// ����������
        /// ��������� COM1
        /// </summary>
        public COMBarcodeScaner()
        {
            Port = new SerialPort("COM4");
            Initialize();
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="portName">��� �����</param>
        public COMBarcodeScaner(string portName)
        {
            Port = new SerialPort(portName);
            Initialize();
        }

        /// <summary>
        /// ���������� ����������
        /// </summary>
        protected void Initialize()
        {
            Suffix = new[] { 13, 10 };
        }

        /// <summary>
        /// ����������� � ��������� ������
        /// </summary>
        /// <returns>True � ������ ������</returns>
        public new bool Connect()
        {
            // ����������
            try
            {
                Port.Open();
                Port.DataReceived += Port_DataReceived;
                _connect = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "������ ����������� �������", MessageBoxButton.OK, MessageBoxImage.Error);
                _connect = false;
            }
            return _connect;
        }


        /// <summary>
        /// ���������� �� ��������� ������
        /// </summary>
        /// <returns>True � ������ ������</returns>
        public new bool Disconnect()
        {
            // ������������
            try
            {
                Port.Close();
                Port.DataReceived -= Port_DataReceived;
                _connect = false;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "������ ����������� �������", MessageBoxButton.OK, MessageBoxImage.Error);
                _connect = Port.IsOpen;
                return false;
            }
        }

        /// <summary>
        /// ��������� ����������� ������
        /// </summary>
        /// <param name="sender">�����������</param>
        /// <param name="e">��������� �������</param>
        protected void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_connect) return;
            string code = Port.ReadExisting();
            foreach (char data in code)
                Read(data);
        }
    }
}
