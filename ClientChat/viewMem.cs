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
    public partial class viewMem : Form
    {
        public viewMem()
        {
            InitializeComponent();
        }
        public List<ClientOnline> allClient = new List<ClientOnline>();

        private void viewMem_Load(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                foreach (ClientOnline item in allClient)
                {
                    flpListClient.Controls.Add(item);
                }
            }));
        }
    }
}
