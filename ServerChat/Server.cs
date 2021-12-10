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
using System.Reflection;

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
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            textName.Text = receivedPath1.Substring(0,receivedPath1.Length - 20) + @"ClientChat\Avt\";
        }
        //D:\sql\MultiChatVersion2\ClientChat\Avt\
        //D:\sql\MultiChatVersion2\ServerChat\bin\Debug
        //D:\sql\MultiChatVersion2\ClientChat\Avt\
        //Lay dia chia IP Hien co
        IPEndPoint IP;
        Socket Server1;
        List<Socket> ClientList;
        string avtNAme = "";
        //Hàm gởi data

        int checkPortInListClient(string ipPort) {
            foreach(Client item in listCList) {
                if (ipPort.Substring(ipPort.IndexOf(':') + 1) == item.IpPort)
                    return -1;
            }
            return 0;
        }
        private void sendString(string s,Socket clien) { 
            foreach(Socket item in ClientList) {
                if (item.RemoteEndPoint.ToString() == clien.RemoteEndPoint.ToString()) {
                    clien.Send(Serialize(s));
                    break;
                }
            }
        }
        //Nhận File
        public static string receivedPath = "C:/Users/ASUS/OneDrive/caro/OneDrive/Desktop/";
        public void ReceiveFile(int receivedBytesLen,byte[] clientData) {
            try {         
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.UTF8.GetString(clientData, 4, fileNameLen);

                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + "/" + fileName, FileMode.Append));
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                bWrite.Close();            }
            catch {
                MessageBox.Show("File receive error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);    
            }
        }
        //Hàm tạo mới IP Socket và connect với người dùng
        private void Connect() {
            ClientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, Int32.Parse(textPort.Text));
            Server1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server1.Bind(IP);
            ClientList.Clear();
            listCList.Clear();
            Thread Listen = new Thread(() =>
            {
                try {
                    while (true) {
                        Server1.Listen(100);
                        Socket clien = Server1.Accept();
                        ClientList.Add(clien);
                        textName.Text = clien.RemoteEndPoint.ToString();
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
        //192.168.56.1
        //Hàm kiểm tra người dùng có còn kết nối hay không
        bool SocketConnected(Socket s) {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
        //Hàm nhận data từ người dùng
        public static string receivedPath1 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //@"D:\sql\MultiChatVersion2\ClientChat\Avt\";
        //D:\sql\MultiChatVersion2\ServerChat\bin\Debug
        private void saveImage(byte[] clientData,int receivedBytesLen) { 
            try{
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.UTF8.GetString(clientData, 4, fileNameLen);
                avtNAme = fileName;
                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath1.Substring(0,receivedPath1.Length-20)+ @"ClientChat\Avt\\" + fileName, FileMode.Append));
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                bWrite.Close();
            }
            catch
            {
                MessageBox.Show("Cannot dowload this Image", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Receive(object obj)
        {
            Socket clien = (Socket)obj;
            try
            {
                while (true) {
                    byte[] data = new byte[1024 * 5000];
                    int recive = clien.Receive(data);
                    if (data[0] != 11 && data[0] != 2 && data[0] !=12 && data[0] != 13) {// xử lý gửi nhận người dùng
                        byte[] data1 = new byte[1024 * 5000];
                        for (int i = 1; i < 1024 * 5000; i++){
                            data1[i - 1] = data[i];
                        }
                        string s = (string)Deserialize(data1);
                        checkString1(s, clien, data);
                    }
                    
                    else if (data[0] == 2) { // xử lý đăng ký 
                        sql_manage f = new sql_manage();
                        int lengthS = (int)data[1];
                        string s = "";
                        for (int j = 2; j < 2 + lengthS; j++)
                            s += Convert.ToChar(data[j]);
                        string username = ""; string password = ""; string name = "";
                        int i = 0;
                        while (true){
                            if (s[i] != '@') username += s[i];
                            if (s[i] == '@') { i++; break; }
                            i++;
                        }
                        while (i < s.Length){
                            password += s[i]; i++;
                        }
                        int check = f.returnNo(username, password, 2);
                        if (check == 0){
                            byte[] data1 = new byte[1024 * 5000];
                            int k = 0;
                            for (i=2 + lengthS;i<1024*5000;i++) {
                                data1[k] = data[i];
                                k++;
                            }
                            saveImage(data1, recive - 2 - lengthS);
                            f.inserAccount(username, password, "", avtNAme);
                            sendString("3success", clien);
                        }
                        else
                            sendString("4unsuccess", clien);
                    }
                    else if (data[0]==11) {//xử lý gửi file hình ảnh
                        int lengthname = (int)data[1];
                        byte[] data1 = new byte[recive-1-lengthname];
                        string opName = "";
                        data1[0] = 11;
                        for (int i = 2; i < 2 + lengthname; i++)
                            opName += Convert.ToChar(data[i]);
                        textName.Text = $"{opName}+{recive}";
                        int k = 1;
                        for(int i = 2+lengthname; i < recive; i++) {
                            data1[k] = data[i];k++;
                        }
                        string ipPortRec = "";
                        //ReceiveFile(recive-2-lengthname, data1);
                        foreach(Client item in listCList) {
                            if (opName == item.Name)
                                ipPortRec = item.IpPort;
                        }
                        foreach(Socket item in ClientList) {
                            if (SocketConnected(item)&&(item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':')+1))==ipPortRec) {
                                item.Send(data1);
                                break;
                            }
                            else continue;
                        }
                    }
                    else if (data[0] == 12)
                    {// xử lý gửi nhận group
                        byte[] data1 = new byte[1024 * 5000];
                        for (int i = 1; i < 1024 * 5000; i++)
                        {
                            data1[i - 1] = data[i];
                        }
                        string s = (string)Deserialize(data1);
                        checkString2(s, clien);
                    }
                    else if(data[0] == 13)
                    {//xử lý gửi file và gửi hình ảnh cho group
                        sql_manage f = new sql_manage();
                        int lengthname = (int)data[1];
                        byte[] data1 = new byte[recive - 1 - lengthname];
                        string opName = "";
                        data1[0] = 13;
                        for (int i = 2; i < 2 + lengthname; i++)
                            opName += Convert.ToChar(data[i]);
                        textName.Text = $"{opName}+{recive}";
                        int k = 1;
                        for (int i = 2 + lengthname; i < recive; i++)
                        {
                            data1[k] = data[i]; k++;
                        }
                        List<string> listName = f.LoadMemGroup2(opName);
                        string listIP = "";
                        foreach (Client item in listCList)
                        {
                            if (checkNameClietinList(listName, item.Name) == -1)
                                listIP += $"{item.IpPort}:";
                        }
                        foreach (Socket item in ClientList)
                        {
                            if (SocketConnected(item) && listIP.Contains((item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':') + 1))))
                            {
                                //item.Send(Serialize(sendString));
                                item.Send(data1);
                            }
                            else continue;
                        }
                    }
                }
            }
            catch { }
        }
        //Xoa người dùng đã out ra khỏi listClient
        public void removeListClient(string username,sql_manage f) {
            listClient.Rows.Clear();
            listClient.Refresh();
            f.updateActi(username, 0);int i = 0;
            foreach(Client item in listCList) {
                if (item.Name == username) break;
                i++;
            }
            listCList.RemoveAt(i);
            f.Loaddata(listClient, "none", username, 1);
        }
        //Load bảng list clien mỗi khi có client khác kết nối
        private void LoadDatGridView(string username,sql_manage f,Socket clien) {
            listClient.Rows.Clear();
            listClient.Refresh();
            f.updateActi(username, 1);
            int index = clien.RemoteEndPoint.ToString().IndexOf(':');
            f.Loaddata(listClient, clien.RemoteEndPoint.ToString().Substring(index + 1), username, 0);
        }
        private void checkString2(string s,Socket clien) {
            sql_manage f = new sql_manage();
            if (s[0] == '1') { //gửi danh sách các client on và off cho client để tạo group
                clien.Send(Serialize(f.getListClient("")));
            }
            else if (s[0] == '2') {//insert group 
                textName.Text = s;
                string nameGroup = s.Substring(1, s.IndexOf('@')-1);
                string allMem = s.Substring(s.IndexOf('@') + 1);
                f.InsertGroup(nameGroup);
                f.insertMemGroup(nameGroup, allMem);
                string sendString = f.LoadMemNewGroup(nameGroup);
                string listIP = "";
                foreach (Client item in listCList)
                {
                    if (allMem.Contains(item.Name))
                        listIP += $"{item.IpPort}:";
                }
                foreach (Socket item in ClientList)
                {
                    if (SocketConnected(item) && listIP.Contains((item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':') + 1))))
                    {
                        item.Send(Serialize(sendString));
                    }
                    else continue;
                }
            }
            else if(s[0] == '3') { // gửi danh sách group và danh sách mem 
                string userName = s.Substring(1);
                string str = f.loadGroup(userName);
                textName.Text = str;
                clien.Send(Serialize(str));
            }
            else if(s[0] == '4') {//Load danh sách các tin nhắn 
                int Index = s.IndexOf('@');
                string userName = s.Substring(1, Index - 1);
                string idGroup = s.Substring(Index + 1);
                clien.Send(Serialize(f.loadMessageGroup(idGroup)));
            }
            else if(s[0] == '5') 
            {//Chat tin nhắn với group
                string[] lism = s.Substring(1).Split('@');
                string nameSend = lism[0];
                string idGroup = lism[1];
                string content = lism[2];
                f.insertMessGroup(nameSend, idGroup,content, 0);
                List<string> listName = f.LoadMemGroup(idGroup);
                string listIP = "";
                foreach(Client item in listCList)
                {
                    if (checkNameClietinList(listName, item.Name) == -1)
                        listIP += $"{item.IpPort}:";
                }
                foreach (Socket item in ClientList)
                {
                    if (SocketConnected(item) && listIP.Contains((item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':') + 1))))
                    {
                        //item.Send(Serialize(sendString));
                        item.Send(Serialize($"e{idGroup}@{nameSend}@{content}"));
                    }
                    else continue;
                }
            }
            else if (s[0] == '6')//chat emoji với group
            {
                int index1 = s.IndexOf('@');
                int index2 = s.IndexOf('@', index1 + 1);
                string nameSend = s.Substring(1, index1 - 1);
                string idGroup = "";
                for (int i = index1 + 1; i < index2; i++)
                    idGroup += s[i];
                List<string> listName = f.LoadMemGroup(idGroup);
                string listIP = "";
                f.insertMessGroup(nameSend, idGroup,s.Substring(index2+1) , 1);
                foreach (Client item in listCList)
                {
                    if (checkNameClietinList(listName, item.Name) == -1)
                        listIP += $"{item.IpPort}:";
                }
                foreach (Socket item in ClientList)
                {
                    if (SocketConnected(item) && listIP.Contains((item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':') + 1))))
                    {
                        //item.Send(Serialize(sendString));
                        item.Send(Serialize($"f{idGroup}@{nameSend}@{s.Substring(index2+1)}"));
                    }
                    else continue;
                }
            }
            else if(s[0]== '7') //Gửi voice chat
            {
                int index1 = s.IndexOf(':');
                int index2 = s.IndexOf(':', index1 + 1);
                string nameSend = s.Substring(1, index1 - 1);
                string idGroup = "";
                for (int i = index1 + 1; i < index2; i++)
                    idGroup += s[i];
                List<string> listName = f.LoadMemGroup(idGroup);
                string listIP = "";
                f.insertMessGroup(nameSend, idGroup, s.Substring(index2 + 1), 2);
                foreach (Client item in listCList)
                {
                    if (checkNameClietinList(listName, item.Name) == -1)
                        listIP += $"{item.IpPort}:";
                }
                foreach (Socket item in ClientList)
                {
                    if (SocketConnected(item) && listIP.Contains((item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':') + 1))))
                    {
                        //item.Send(Serialize(sendString));
                        item.Send(Serialize($"g{idGroup}:{nameSend}:{s.Substring(index2 + 1)}"));
                    }
                    else continue;
                }
            }
        }
        //Kiểm tra name client tồn tại trong list client 
        private int checkNameClietinList(List<string> lism,string nameUser) { 
            foreach(string item in lism)
            {
                if (nameUser == item)
                    return -1;
            }
            return 0;
        }
        //c*7*hungmai*1*1*12*0Alo alo alo*6*bababa*1*1*17*0Alo alo alsdfsdo*7*hungmai*1*1*16*0Alo alo fsdfalo*5*anhem*1*1*15*0Alo alo fsfalo*6*bababa*1*1*16*0Alo alofsfd alo*7*hungmai*1*1*15*0Alo dfsalo alo*5*anhem*1*1*12*0Alo alo alo*6*bababa*1*1*12*0Alo alo alo*7*hungmai*1*1*9*0Alo  alo
        //b2:Djitme:5:hung:hung22:hungmai:bababa:anhem:
        //b1:AnhEm:3:hungmai:bababa:anhem:2:Djitme:5:hung:hung22:hungmai:bababa:anhem:
        private void checkString1(string s,Socket clien,byte[] rec) {
            sql_manage f = new sql_manage();
            if (rec[0]==1) {//Kiểm tra người dùng đăng nhập thành công 
                int i = 1;
                string userName = "";string password = "";
                while (true) {
                    if(s[i]!='@')userName += s[i];
                    else{i++;break;}
                    i++;
                }
                while (i < s.Length) { 
                    password += s[i];i++;
                }
                textName.Text = $"{userName}   {password}";
                int check = f.returnNo(userName, password, 1);
                if (check == -1) {
                    sendString($"1success{f.getAvtName(userName)}", clien);
                }
                else
                    sendString("2unsuccess", clien);
            }    
            else if (rec[0] == 3) { //KIểm tra người dùng bị out 
                string userName = s.Substring(1);
                if (!string.IsNullOrEmpty(userName)) { 
                    removeListClient(userName, f);
                    string listClien = f.getListClientActi(s.Substring(1));
                    foreach(Socket item in ClientList) {
                        if (SocketConnected(item)&&checkPortInListClient(item.RemoteEndPoint.ToString())==-1) {
                            item.Send(Serialize(listClien));
                        }    
                        else
                            continue;
                    }
                }
            }
            else if (rec[0] == 4) {//Load danh sách người đang online cho người dùng
                string username = "";
                for (int i = 1; i < s.Length; i++)
                    username += s[i];
                LoadDatGridView(username, f, clien);
                string listClien = f.getListClientActi(username);
                foreach (Socket item in ClientList)
                {
                    if (SocketConnected(item) && checkPortInListClient(item.RemoteEndPoint.ToString()) == -1)
                        item.Send(Serialize(listClien));
                }
            }
            else if (rec[0] == 5) {//Gửi và nhận tin nhắn
                int Index = s.IndexOf('@');
                string userName = "";
                string content = "";
                string nameSend = "";
                string ipPortRec = "";
                int i = 1;
                while (true)
                {
                    if (s[i] == '@')
                    {
                        i++;break;
                    }
                    else
                    {
                        userName += s[i];
                        i++;
                    }
                }
                while (i < s.Length)
                {
                    content += s[i];i++;
                }
                textName.Text = userName + "/" + content;
                foreach(Client item in listCList) {
                    if ($"{textIP.Text}:{item.IpPort}" == clien.RemoteEndPoint.ToString()) {
                        f.InsertMess(item.Name, userName, content,0);
                        nameSend = item.Name;
                        //break;
                    }
                    if (userName == item.Name)
                        ipPortRec = item.IpPort;
                }
                foreach(Socket item in ClientList) {
                    if (SocketConnected(item)&&(item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':')+1))==ipPortRec) {
                        item.Send(Serialize($"8{nameSend}@{content}"));
                        break;
                    }
                    else
                        continue;
                }
            }
            else if (rec[0] == 6) { //Nhận và in ra danh sách các tin nhắn
                int Index = s.IndexOf('@');
                string sendString = f.LoadMess(s.Substring(1, Index - 1), s.Substring(Index + 1));
                clien.Send(Serialize(sendString));
            }
            else if (rec[0] == 7) { //Nhận file 
                int Index = s.IndexOf('@');
                string userName = s.Substring(0, Index);
                string nameSend = "";
                string ipPortRec = "";
                foreach(Client item in listCList) {
                    if ($"{textIP.Text}:{item.IpPort}" == clien.RemoteEndPoint.ToString()) {
                        f.InsertMess(item.Name, userName, s.Substring(Index + 1),1);
                        nameSend = item.Name;
                        //break;
                    }
                    if (userName == item.Name)
                        ipPortRec = item.IpPort;
                }
                foreach(Socket item in ClientList) {
                    if (SocketConnected(item)&&(item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':')+1))==ipPortRec) {
                        item.Send(Serialize($"9{nameSend}@{s.Substring(Index + 1)}"));
                        break;
                    }
                    else
                        continue;
                }
            }
            else if (rec[0] == 8) {//Nhận voice chat
                int Index = s.IndexOf(':');
                string userName = s.Substring(0, Index);
                string nameSend = "";
                string ipPortRec = "";
                foreach(Client item in listCList) {
                    if ($"{textIP.Text}:{item.IpPort}" == clien.RemoteEndPoint.ToString()) {
                        f.InsertMess(item.Name, userName, s.Substring(Index + 1),2);
                        nameSend = item.Name;
                        //break;
                    }
                    if (userName == item.Name)
                        ipPortRec = item.IpPort;
                }
                foreach(Socket item in ClientList) {
                    if (SocketConnected(item)&&(item.RemoteEndPoint.ToString().Substring(item.RemoteEndPoint.ToString().IndexOf(':')+1))==ipPortRec) {
                        item.Send(Serialize($"0{nameSend}@{s.Substring(Index + 1)}"));
                        break;
                    }
                    else
                        continue;
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

        private void BtnConnect_Click(object sender, EventArgs e){
            if (!string.IsNullOrEmpty(textIP.Text) && !string.IsNullOrEmpty(textPort.Text) && !string.IsNullOrEmpty(textName.Text)) {
                Connect();
                BtnConnect.Enabled = false;
                btnOUT.Enabled = true;
            }
            else
                MessageBox.Show("IP or Port is Empty","Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void btnOUT_Click(object sender, EventArgs e){
            //Server1.Shutdown(SocketShutdown.Both);
            sql_manage f = new sql_manage();
            foreach(Client item in listCList) {
                f.updateActi(item.Name, 0);
            }
            foreach(Socket item in ClientList)
            {
                if (SocketConnected(item) && checkPortInListClient(item.RemoteEndPoint.ToString()) == -1)
                {
                    item.Send(Serialize("5Disconnect"));
                }
                else
                    continue;
            }
            listCList.Clear();
            Server1.Close();
            listClient.Rows.Clear();
            BtnConnect.Enabled = true;
            btnOUT.Enabled = false;
        }
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            listCList.Clear();
            sql_manage f = new sql_manage();
            foreach (Client item in listCList)
            {
                f.updateActi(item.Name, 0);
            }
        }
    }
}
