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

        public Client()
        {
            
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

                Thread listerServer = new Thread(new ThreadStart(ReceiveMessage));
                listerServer.IsBackground = true;
                listerServer.Start();
            }
            catch
            {
                ipe = new IPEndPoint(IPAddress.Parse(tbIP.Text), int.Parse(tbPort.Text));
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 5000];

                int rec = client.Receive(buffer);
                string mess = (String)Deserialize(buffer);

                CheckMessage(mess);
            }
        }

        //hàm check message từ server
        private void CheckMessage(string message)
        {
            //login thành công
            if (message[0] == '1')
            {
                metroTabControl1.SelectedTab = metroTabControl1.TabPages["mess"];
            }
            //login không thành công
            else if (message[0] == '2')
            {
                MessageBox.Show("Invalid password or username");
            }
            //register thành công
            else if (message[0] == '3')
            {
                MessageBox.Show("Register successfully");
                UsernameRegister.Clear();
                PasswordRegister.Clear();
                RePassRegister.Clear();
            }
            //register không thành công
            else if (message[0] == '4')
            {
                MessageBox.Show("Your username has been already exist");
            }
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
    }
}
