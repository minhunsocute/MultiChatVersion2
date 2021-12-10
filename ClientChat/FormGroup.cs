using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class FormGroup : Form
    {
        public FormGroup()
        {
            InitializeComponent();
        }
        public string listCLientinGroup = "";
        List<ClientOnline> allClient = new List<ClientOnline>();
        private void FormGroup_Load(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                string mess = Client.allclie;
                string[] listTmp = mess.Split(':');
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                allClient = new List<ClientOnline>();
                    flpListClient.Controls.Clear();
                for (int j = 0; j < listTmp.Length; j += 2)
                {
                    if (listTmp[j] != "" && listTmp[j] != Client.name)
                    {
                        ClientOnline clientOnline = new ClientOnline();
                        clientOnline.lbName.Text = listTmp[j];
                        clientOnline.CheckClick = 0;
                        clientOnline.NoRecDontSee = 0;
                        allClient.Add(clientOnline);
                        Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"Avt\" + listTmp[j + 1]);
                        var ms = new MemoryStream();
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var bytes = ms.ToArray();
                        clientOnline.avtClient.Image = Image.FromStream(new MemoryStream(bytes));
                        clientOnline.Tag = clientOnline;
                        clientOnline.Click += ClientOnline_Click;
                    }
                }
                int i = 0;
                foreach (ClientOnline item in allClient)
                {
                    flpListClient.Controls.Add(item);
                    i++;
                }
            }));
        }

        private void ClientOnline_Click(object sender, EventArgs e){
            ClientOnline ite = (sender as ClientOnline).Tag as ClientOnline;
            if (ite.CheckClick == -1) { 
                ite.CheckClick = 0;
                ite.BackColor = DefaultBackColor;
                ite.lbName.BackColor = DefaultBackColor;
                
            }
            else {
                ite.CheckClick = -1;
                ite.BackColor = Color.FromArgb(42, 220, 190);
                ite.lbName.BackColor = Color.FromArgb(42, 220, 190);
            }
        }

        private void btnConnectServer_Click(object sender, EventArgs e){
            if (!string.IsNullOrEmpty(nameGroup.Text)) {
                listCLientinGroup = "";
                listCLientinGroup += $"2{nameGroup.Text}@{Client.name}:";
                int count = 0;
                foreach(ClientOnline item in allClient) { 
                    if(item.CheckClick == -1) {
                        count++;
                        listCLientinGroup +=item.lbName.Text+":";
                    }
                }
                if(count > 1) {
                    this.Close();
                }
                else {  
                    MessageBox.Show("the number of people in the group must be greater than 1","Message",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    listCLientinGroup = "";
                }
            }
            else 
                MessageBox.Show("Name group is null","Message",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
    }
}
