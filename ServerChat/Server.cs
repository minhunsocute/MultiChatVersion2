using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerChat
{
    public partial class Server : Form
    {
        //Bien toan cuc
        public static List<Client> listCList = new List<Client>();

        public Server()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            textIP.Text = "127.0.0.1";
        }
        //Lay dia chia IP Hien co
        IPEndPoint IP;
        Socket Server1;
        List<Socket> ClientList;

        private void sendString(string s,Socket clien) { 
            foreach(Socket item in ClientList) {
                if (item.RemoteEndPoint.ToString() == clien.RemoteEndPoint.ToString()) {
                    clien.Send(Serialize(s));
                    break;
                }
            }
        }
        private void Connect() {
            ClientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, Int32.Parse(textPort.Text));
            Server1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server1.Bind(IP);
            Thread Listen = new Thread(() =>
            {
                try {
                    while (true) {
                        Server1.Listen(100);
                        Socket clien = Server1.Accept();
                        ClientList.Add(clien);
                        Thread rec = new Thread(Receive);
                        rec.IsBackground = true;
                        rec.Start(clien);
                    }
                }
                catch {
                    IP = new IPEndPoint(IPAddress.Any, Int32.Parse(textPort.Text));
                    Server1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        bool SocketConnected(Socket s) {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        private void Receive(object obj)
        {
            Socket clien = (Socket)obj;
            try
            {
                while (true) {
                    byte[] data = new byte[1024 * 5000];
                    clien.Receive(data);
                    string mess = (string)Deserialize(data);
                    checkString1(mess,clien);
                }
            }
            catch { }
        }
        private void checkString1(string s,Socket clien) {
            sql_manage f = new sql_manage();
            if (s[0] == '1') {
                int i = 1;
                string userName = "";string password = "";
                while (true) {
                    if(s[i]!='@')userName += s[i];
                    if (s[i] == '@') {i++;break;}
                    i++;
                }
                while (i < s.Length) { 
                    password += s[i];i++;
                }

            }
        }
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);
            return stream.ToArray();// stream tra ra 1 day byte
        }
        // gom manh
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }
    }
}
