using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class Client : Form
    {
        Socket client;
        IPEndPoint ipe;
        Thread threadConnectServer;
        string name;
        List<ClientOnline> listClientOnline;
        public Client()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void btnConnectServer_Click(object sender, EventArgs e)
        {
            threadConnectServer = new Thread(new ThreadStart(ConnectServer));
            threadConnectServer.IsBackground = true;
            threadConnectServer.Start();
        }

        private void ConnectServer()
        {
            try
            {
                ipe = new IPEndPoint(IPAddress.Parse(tbIP.Text), int.Parse(tbPort.Text));
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipe);

                btnSignIn.Enabled = true;
                btnRegister.Enabled = true;
                btnDisConnect.Enabled = true;
                btnConnectServer.Enabled = false;

                Thread listerServer = new Thread(ReceiveMessage);
                listerServer.IsBackground = true;
                listerServer.Start();
            }
            catch
            {
                MessageBox.Show("Can't connect to server", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 5000];
                    int rec = client.Receive(buffer);
                    string mess = (String)Deserialize(buffer);
                    MessageFromServer.Text += $"Server:{mess}{Environment.NewLine}";
                    CheckMessage(mess);
                }
            }
            catch
            {
                MessageBox.Show("Server is disconnected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //hàm check message từ server
        private void CheckMessage(string message)
        {
            //login thành công
            if (message[0] == '1')
            {
                name = Username.Text;

                this.Invoke(new Action(() =>
                {
                    metroTabControl1.SelectedTab = metroTabControl1.TabPages["mess"];

                    ((Control)mess).Enabled = true;
                    ((Control)login).Enabled = false;
                    ((Control)creat).Enabled = false;

                }));
                //load người dùng
                client.Send(Serialize($"4{Username.Text}"));
            }
            //login không thành công
            else if (message[0] == '2')
            {
                MessageBox.Show("Invalid password or username","Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            //register thành công
            else if (message[0] == '3')
            {
                MessageBox.Show("Register successfully");
                this.Invoke(new Action(() =>{
                    UsernameRegister.Clear();
                    PasswordRegister.Clear();
                    RePassRegister.Clear();
                }));
            }
            //register không thành công
            else if (message[0] == '4')
            {
                MessageBox.Show("Your username has been already exist");
            //add client online into flow layout panel
            }else if(message[0] == '6')
            {
                string mess = message.Substring(1);

                string[] listTmp = mess.Split('@');

                flpListClient.Controls.Clear();

                foreach(string s in listTmp)
                {
                    if (s != "" && s != name) {
                        ClientOnline clientOnline = new ClientOnline();
                        clientOnline.Name1 = s;
                        clientOnline.Click += ClientOnline_Click;
                        listClientOnline.Add(clientOnline);
                    }
                }

                foreach (ClientOnline item in listClientOnline)
                {
                    OpText.Text = item.Name1;
                    flpListClient.Controls.Add(item);
                }
            }
        }

        private void ClientOnline_Click(object sender, EventArgs e)
        {
            string s = (sender as ClientOnline).Tag as string;
            OpText.Text = s;
        }

        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(Password.Text))
            {
                client.Send(Serialize($"1{Username.Text}@{Password.Text}"));
            }
            else
            {
                MessageBox.Show("Username or password can't be empty");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameRegister.Text) && !string.IsNullOrEmpty(PasswordRegister.Text) && PasswordRegister.Text == RePassRegister.Text)
            {
                client.Send(Serialize($"2{UsernameRegister.Text}@{PasswordRegister.Text}"));
            }
            else
            {
                MessageBox.Show("Username or password can't be empty");
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if(name != "") {
                client.Send(Serialize($"3{name}"));
                client.Close();
                Application.Exit();
            }
            else
            {
                client.Send(Serialize($"3{name}"));
                client.Close();
                Application.Exit();
            }
        }
    }
}
