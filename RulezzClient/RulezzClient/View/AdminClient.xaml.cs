using System.IO;
using System.Text;
using System.Windows;

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
