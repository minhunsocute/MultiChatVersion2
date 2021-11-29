using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class FilMessSend : UserControl
    {
        public FilMessSend()
        {
            InitializeComponent();
        }
        public FilMessSend(string fileName) {
            this.fileName = fileName;
        }
        private  string fileName;

        public string FileName { get => fileName; set => fileName = value; }

        private void FilMessSend_Load(object sender, EventArgs e)
        {
            guna2TextBox1.Text = this.fileName;
        }
    }
}
