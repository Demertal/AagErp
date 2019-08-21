using System;
using System.Windows;
using System.Windows.Interop;

namespace AagClient.Views
{
    /// <summary>
    /// Логика взаимодействия для Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            this.SourceInitialized += Shell_SourceInitialized;
        }

        private void Shell_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        private const int WmSyscommand = 0x0112;
        private const int ScMove = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WmSyscommand:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == ScMove)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
