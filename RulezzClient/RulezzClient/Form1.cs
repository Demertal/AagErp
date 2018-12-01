using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RulezzClient
{
    public partial class Form1 : Form
    {
        private MainWindow _mw;

        public Form1(MainWindow mw)
        {
            _mw = mw;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _mw.NameServer = textBox1.Text;
            Close();
        }
    }
}
