using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RulezzClient
{
    /// <summary>
    /// Логика взаимодействия для AdminClient.xaml
    /// </summary>
    public partial class AdminClient : Window
    {
        public AdminClient()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Program Files\RulezzClient\settings", true, Encoding.Default))
            {
                sw.WriteLine(IPTextBox.Text);
                sw.WriteLine(PortTextBox.Text);
            }
        }
    }
}
