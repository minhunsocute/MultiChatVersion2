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
    public partial class ClientOnline : UserControl
    {
        public ClientOnline()
        {
            InitializeComponent();
            lbCount.Hide();
        }
        public ClientOnline(string name)
        {
            this.Name1 = name;

        }
        private string name;
        public string Name1 { get => name; set => name = value; }
        public int CheckClick { get => checkClick; set => checkClick = value; }
        public int NoRecDontSee { get => noRecDontSee; set => noRecDontSee = value; }

        private int checkClick;
        private int noRecDontSee;

        private void ClientOnline_MouseEnter(object sender, EventArgs e)
        {
            if (CheckClick == 0)
            {
                this.BackColor = Color.Silver;
                lbName.BackColor = Color.Silver;
            }
        }

        private void ClientOnline_MouseLeave(object sender, EventArgs e)
        {
            if (checkClick == 0)
            {
                this.BackColor = DefaultBackColor;
                lbName.BackColor = DefaultBackColor;
            }
        }

        private void lbName_MouseEnter(object sender, EventArgs e)
        {
            if (CheckClick == 0)
            {
                this.BackColor = Color.Silver;
                lbName.BackColor = Color.Silver;
            }
        }

        private void lbName_MouseLeave(object sender, EventArgs e)
        {
            if (checkClick == 0)
            {
                this.BackColor = DefaultBackColor;
                lbName.BackColor = DefaultBackColor;
            }
        }
    }
}
