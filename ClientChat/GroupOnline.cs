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
    public partial class GroupOnline : UserControl
    {
        public GroupOnline()
        {
            InitializeComponent();
            lbCount.Hide();
        }

        public GroupOnline(string name) {
            this.Name1 = name;
        }
        
        
        
        
        private string name;
        public string Name1 { get => name;   set => name = value; }
        public int CheckClick { get => checkClick; set => checkClick = value; }
        public int NoRecDontSee { get => noRecDontSee; set => noRecDontSee = value; }
        private int checkClick;
        private int noRecDontSee;
        public List<ClientOnline> memGroup;
        public int idGroup;
        
        
        
        private void GroupOnline_MouseEnter(object sender, EventArgs e){
            if (CheckClick == 0)
            {
                this.BackColor = Color.Silver;
                lbName.BackColor = Color.Silver;
            }
        }

        private void GroupOnline_MouseLeave(object sender, EventArgs e)
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
