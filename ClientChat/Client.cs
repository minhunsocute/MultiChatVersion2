using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class Client : Form
    {
        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        Socket client;
        IPEndPoint ipe;
        Thread threadConnectServer;
        string name;
        WaveIn wave;
        WaveFileWriter writer;
        /// <summary>
        /// BIến này là biến để kiểm tra xem là thằng server nó còn online hay k 
        /// Kiểu đkm tk server nó mà tắt rối ấy t thấy cái ràng buộc client.send nó bắt buộc gửi 
        /// nên bị lỗi ngầm mặc dù k hiện rõ ra nhưng nó vẫn là lỗi
        /// nên tao thêm biến này 1 -> server còn connect 0 -> không còn
        /// </summary>
        int checkServerOn = 0;
        string fileNamePath = "";
        int choseAvt = 0;
        int checkRecord = 0;
        int deviceName =0;
        List<ClientOnline> listClientOnline;
        System.Timers.Timer t;
        int s = 30;
        public Client()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            metroTabControl1.SelectedTab = metroTabControl1.TabPages["login"];
            LoadListView();
            recordVoice.Hide();
            deviceName = LoadDevices();
            //tbSearch.Text = deviceName;
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += OntimeEvent;
        }

        private int LoadDevices()
        {
            int s =0;
            //for (int deviceId = 0; deviceId < WaveIn.DeviceCount; deviceId++){
            //    var deviceInfo = WaveIn.GetCapabilities(deviceId);
            //}
            if(WaveIn.DeviceCount > 0) { 
                if(WaveIn.DeviceCount > 1) {
                    for (int deviceId = 0; deviceId < WaveIn.DeviceCount; deviceId++){
                        var deviceInfo = WaveIn.GetCapabilities(deviceId);
                        if (deviceInfo.ProductName.Contains("DroidCam"))
                            continue;
                        else
                        {
                            s = deviceId;
                            break;
                        }
                    }
                }
                else {
                    s = 0;
                }
            }
            return s;
        }
        private void OntimeEvent(object sender, ElapsedEventArgs e){
            this.Invoke(new Action(() =>
            {
                s -= 1;
                if (s == 0) {
                    t.Stop();
                    stopRecording();
                }
                timeOut.Text = string.Format($"00:{s.ToString()}");
            }));
        }
        
        // CHỉnh size cho Send mess
        private void buitSizeSend(string s,Send uc) { 
            if(s.Length < 30){
                uc.guna2CustomGradientPanel1.Width = s.Length * 10 + 40;
                uc.label1.Width = s.Length*10+20;
                uc.label1.Text = s;
            }
            else {
                uc.label1.Text = string.Empty;
                for(int i = 0; i < s.Length; i++) {
                    if (i % 30 == 0&&i!=0) {
                        uc.Height += 17;
                        uc.guna2CustomGradientPanel1.Height += 17;
                        uc.label1.Height += 17;
                        uc.label1.Text += $"{Environment.NewLine}{s[i]}";
                    }
                    else
                        uc.label1.Text += s[i];
                }
            }
        }
        //Chỉnh size cho Rec mess
        private void buitSizeRec(string s,Recieve uc) { 
            if(s.Length < 30){
                uc.guna2CustomGradientPanel1.Width = s.Length * 10 + 40;
                uc.label1.Width = s.Length*10+20;
                uc.label1.Text = s;
            }
            else {
                uc.label1.Text = string.Empty;
                for(int i = 0; i < s.Length; i++) {
                    if (i % 30 == 0&&i!=0) {
                        uc.Height += 17;
                        uc.guna2CustomGradientPanel1.Height += 17;
                        uc.label1.Height += 17;
                        uc.label1.Text += $"{Environment.NewLine}{s[i]}";
                    }
                    else
                        uc.label1.Text += s[i];
                }
            }
        }

        private void sendData(byte header,string s) {
            byte[] stringBye = Serialize(s);
            byte[] send = new byte[stringBye.Length + 1];
            send[0] = header;
            for (int i = 1; i <= stringBye.Length; i++)
                send[i] = stringBye[i - 1];
            client.Send(send);
        }
        private void btnConnectServer_Click(object sender, EventArgs e)
        {
            threadConnectServer = new Thread(new ThreadStart(ConnectServer));
            threadConnectServer.IsBackground = true;
            threadConnectServer.Start();
        }
        //Kết nối đến server 
        private void ConnectServer()
        {
            try{
                ipe = new IPEndPoint(IPAddress.Parse(tbIP.Text), int.Parse(tbPort.Text));
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipe);
                checkServerOn = 1;
                btnSignIn.Enabled = true;
                btnRegister.Enabled = true;
                btnDisConnect.Enabled = true;
                btnConnectServer.Enabled = false;

                Thread listerServer = new Thread(ReceiveMessage);
                listerServer.IsBackground = true;
                listerServer.Start();
            }
            catch{
                MessageBox.Show("Can't connect to server", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Hiện ra lỗi mỗi khi Server ngắt kết nối và trở về trang login
        private void showErrorWhenServerDis() {
            MessageBox.Show("Server is disconnected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            checkServerOn = 0;
            this.Invoke(new Action(() =>{
                allMessage.Controls.Clear();
                flpListClient.Controls.Clear();
                ((Control)mess).Enabled = false;
                ((Control)login).Enabled = true;
                ((Control)creat).Enabled = true;
                btnConnectServer.Enabled = true;
                btnDisConnect.Enabled = false;
                metroTabControl1.SelectedTab = metroTabControl1.TabPages["login"];
                Username.Text = Password.Text = string.Empty;
                name = string.Empty;
            }));
        }
        //Nhận File
        public static string receivedPath = "C:/Users/ASUS/OneDrive/caro/OneDrive/Desktop/";
        public static string receivedPath1 = "D:/sql/MultiChatVersion2/imageTrash/";
        //Nhận file 
        public void ReceiveFile(int receivedBytesLen,byte[] clientData) {
            try {         
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.UTF8.GetString(clientData, 4, fileNameLen);

                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath1 + "/" + fileName, FileMode.Append));
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                bWrite.Close();
                if (fileName.Substring(fileName.Length - 3) == "jpg" || fileName.Substring(fileName.Length - 3) == "png") {
                    this.Invoke(new Action(() =>{
                        var pic = new imageMessRec();
                        Image image = Image.FromFile(receivedPath1+"/"+fileName);
                        var ms = new MemoryStream();
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var bytes = ms.ToArray();
                        pic.guna2PictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
                        pic.guna2CirclePictureBox1.Image = opAvt.Image;
                        pic.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        //pic.guna2Button4.Click += functionSave;
                        pic.clientData = clientData;
                        pic.receivedBytesLen = receivedBytesLen;
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                    }));
                }
                else {
                    this.Invoke(new Action(() =>
                    {
                        var pic = new filMessRec();
                        pic.guna2TextBox1.Text = fileName;
                        pic.receivedBytesLen = receivedBytesLen;
                        pic.clientData = clientData;
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                    }));
                }
            }
            catch {
                MessageBox.Show("File receive error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);    
            }
        }
        //Tải ảnh xuống

        private void ReceiveMessage()
        {
            try{
                while (true){
                    byte[] buffer = new byte[1024 * 5000];
                    int rec = client.Receive(buffer);
                    if (buffer[0] != 11) { 
                        string mess = (String)Deserialize(buffer);
                        CheckMessage(mess);
                    }
                    else {
                        byte[] data = new byte[1024 * 5000];
                        for(int i = 1; i < 1024 * 5000; i++) {
                            data[i - 1] = buffer[i];
                        }
                        ReceiveFile(rec - 1, data);
                    }
                }
            }
            catch
            {
                showErrorWhenServerDis();
            }
        }

        //hàm check message từ server
        private void CheckMessage(string message){
            //login thành công
            if (message[0] == '1'){
                name = Username.Text;
                this.Invoke(new Action(() =>{
                    metroTabControl1.SelectedTab = metroTabControl1.TabPages["mess"];
                    allEmoji.Hide();
                    panel1.Hide();
                    ((Control)mess).Enabled = true;
                    ((Control)login).Enabled = false;
                    ((Control)creat).Enabled = false;
                    nameCLient.Text = Username.Text;
                    string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"Avt\"+message.Substring(8));
                    var ms = new MemoryStream();
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var bytes = ms.ToArray();
                    avtClient.Image = Image.FromStream(new MemoryStream(bytes));
                }));
                //load người dùng
                sendData(4, $"4{Username.Text}");
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
            }
            //Kiểm tra server bị nhấn vào nút disconnect
            else if (message[0] == '5')
            {
                showErrorWhenServerDis();
            }
            else if(message[0] == '6')
            {
                this.Invoke(new Action(() => {
                    string mess = message.Substring(1);
                    string[] listTmp = mess.Split(':');
                    string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    listClientOnline = new List<ClientOnline>();
                    flpListClient.Controls.Clear();
                    for (int j = 0; j < listTmp.Length; j+=2) {
                        if (listTmp[j] != "" && listTmp[j] != name) {
                            ClientOnline clientOnline = new ClientOnline();
                            clientOnline.lbName.Text = listTmp[j];
                            clientOnline.CheckClick = 0;
                            clientOnline.NoRecDontSee = 0;
                            listClientOnline.Add(clientOnline);
                            Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"Avt\" + listTmp[j+1]);
                            var ms = new MemoryStream();
                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            var bytes = ms.ToArray();
                            clientOnline.avtClient.Image = Image.FromStream(new MemoryStream(bytes));
                            clientOnline.Tag = clientOnline;
                            clientOnline.Click += ClientOnline_Click;
                        }
                    }
                    int i = 0;
                    foreach (ClientOnline item in listClientOnline)
                    {
                        if (i == 0) { 
                            OpText.Text = item.lbName.Text;
                            opAvt.Image = item.avtClient.Image;
                        }
                        flpListClient.Controls.Add(item);
                        i++;
                    }
                }));
            }
            else if (message[0] == '7') { //Nhận và load danh sách các tin nhắn
                this.Invoke(new Action(() => {
                    string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    allMessage.Controls.Clear();
                    List<mess> lism = messInstance.Instance.LoadMess(nameCLient.Text, OpText.Text, message);
                    foreach (mess item in lism)
                    {
                        if (item.Content[0] == '0') { 
                            if (item.Type == 1)
                            {
                                Send f = new Send();
                                buitSizeSend(item.Content.Substring(1), f);
                                allMessage.Controls.Add(f);
                            }
                            else if (item.Type == -1)
                            {
                                Recieve f = new Recieve();
                                f.guna2CirclePictureBox1.Image = opAvt.Image;
                                f.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                buitSizeRec(item.Content.Substring(1), f);
                                allMessage.Controls.Add(f);
                            }
                        }
                        else if(item.Content[0] == '1') { 
                            if(item.Type == 1) {
                                Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\" + item.Content.Substring(1));
                                var ms = new MemoryStream();
                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                var bytes = ms.ToArray();
                                var pic = new ImageMessSend();
                                pic.guna2PictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
                                pic.Height -= 100;
                                pic.guna2PictureBox1.Height -= 100;
                                pic.guna2PictureBox1.Width -= 140;
                                allMessage.Controls.Add(pic);

                            }
                            else if(item.Type == -1) {
                                Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\" + item.Content.Substring(1));
                                var ms = new MemoryStream();
                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                var bytes = ms.ToArray();
                                var pic = new imageMessRec();
                                pic.guna2PictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
                                pic.guna2CirclePictureBox1.Image = opAvt.Image;
                                pic.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                pic.Height -= 100;
                                pic.guna2PictureBox1.Height -= 100;
                                pic.guna2PictureBox1.Width -= 140;
                                allMessage.Controls.Add(pic);
                            }
                        }
                        else if (item.Content[0] == '2') { 
                            if(item.Type == -1) {
                                //Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\" + message.Substring(Index + 1));
                                var pic = new voiceMessRec();
                                pic.outFileVoceRecord = path.Substring(0, path.Length - 20) + @"\voiceRecord\" + item.Content.Substring(1);
                                pic.Load();
                                pic.guna2CirclePictureBox1.Image = opAvt.Image;
                                pic.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                allMessage.Controls.Add(pic);
                                allMessage.ScrollControlIntoView(pic);
                            }
                            else {
                                var pic = new voieMessSend();
                                pic.outFileVoceRecord = path.Substring(0, path.Length - 20) + @"\voiceRecord\" + item.Content.Substring(1);
                                pic.Load();
                                allMessage.Controls.Add(pic);
                                allMessage.ScrollControlIntoView(pic);
                            }
                        }
                    }
                    //...
                    if (lism.Count != 0)
                    {
                        if (lism[lism.Count - 1].Type == 1)
                        {
                            var pic = new Send();
                            allMessage.Controls.Add(pic);
                            allMessage.ScrollControlIntoView(pic);
                            allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                            pic.Left - allMessage.AutoScrollPosition.Y);
                            allMessage.Controls.Remove(pic);
                        }
                        else
                        {
                            var pic = new Recieve();
                            allMessage.Controls.Add(pic);
                            allMessage.ScrollControlIntoView(pic);
                            allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                            pic.Left - allMessage.AutoScrollPosition.Y);
                            allMessage.Controls.Remove(pic);
                        }
                    }
                }));                          
            }
            else if (message[0] == '8') {//mesage 
                this.Invoke(new Action(() =>
                {
                    int Index = message.IndexOf('@');
                    if (OpText.Text == message.Substring(1, Index - 1))
                    {
                        var pic = new Recieve();
                        buitSizeRec(message.Substring(Index + 1), pic);
                        pic.guna2CirclePictureBox1.Image = opAvt.Image;
                        pic.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; 
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                    }
                    else { 
                        foreach(ClientOnline item in listClientOnline) { 
                            if(item.lbName.Text == message.Substring(1, Index - 1)) {
                                item.NoRecDontSee++;item.lbCount.Text = item.NoRecDontSee.ToString();item.lbCount.Show();
                                break;
                            }
                        }
                    }
                }));
            }
            else if (message[0] == '9') {//Nhận Emoji              
                this.Invoke(new Action(() =>
                {
                    int Index = message.IndexOf('@');
                    if (OpText.Text == message.Substring(1, Index - 1))
                    {
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\"+message.Substring(Index+1));
                        var ms = new MemoryStream();
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var bytes = ms.ToArray();
                        var pic = new imageMessRec();
                        pic.guna2PictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
                        pic.guna2CirclePictureBox1.Image = opAvt.Image;
                        pic.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Height -= 100;
                        pic.guna2PictureBox1.Height -= 100;
                        pic.guna2PictureBox1.Width -= 140;
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                    }
                    else { 
                        foreach(ClientOnline item in listClientOnline) { 
                            if(item.lbName.Text == message.Substring(1, Index - 1)) {
                                item.NoRecDontSee++;item.lbCount.Text = item.NoRecDontSee.ToString();item.lbCount.Show();
                                break;
                            }
                        }
                    }
                }));
            }
            else if (message[0] == '0' ) { //Nhận voice chat
                this.Invoke(new Action(() =>{
                    int Index = message.IndexOf('@');
                    if (OpText.Text == message.Substring(1, Index - 1))
                    {
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        //Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\" + message.Substring(Index + 1));
                        var pic = new voiceMessRec();
                        pic.outFileVoceRecord = path.Substring(0, path.Length - 20) + @"\voiceRecord\" + message.Substring(Index + 1);
                        pic.Load();
                        pic.guna2CirclePictureBox1.Image = opAvt.Image;
                        pic.guna2CirclePictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);

                    }
                    else
                    {
                        foreach (ClientOnline item in listClientOnline){
                            if (item.lbName.Text == message.Substring(1, Index - 1))
                            {
                                item.NoRecDontSee++; item.lbCount.Text = item.NoRecDontSee.ToString(); item.lbCount.Show();
                                break;
                            }
                        }
                    }
                }));
            }
        }
        //Load ListView 
        private void LoadListView(){
            allEmoji.Controls.Clear();
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            for(int i = 1; i <= 25; i++) {
                Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\"+$"{i.ToString()}.png");
                var ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var bytes = ms.ToArray();
                Guna.UI2.WinForms.Guna2Button f = new Guna.UI2.WinForms.Guna2Button() { Width = 60,Height=60};
                f.Image = Image.FromStream(new MemoryStream(bytes));
                f.BackColor = Color.Transparent;
                f.Tag = i.ToString();
                f.Click += EmojiClick;
                f.ImageSize = new Size(60, 60);
                f.FillColor = Color.Transparent;
                allEmoji.Controls.Add(f);
            }
        }
        //Emoji click 
        private void EmojiClick(object sender, EventArgs e){
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string name = ((sender as Guna.UI2.WinForms.Guna2Button).Tag as string);
            Image image = Image.FromFile(path.Substring(0, path.Length - 9) + @"emoji\"+$"{name}.png");
            var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var bytes = ms.ToArray();
            var pic = new ImageMessSend();
            pic.guna2PictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
            pic.Height -= 100;
            pic.guna2PictureBox1.Height -= 100;
            pic.guna2PictureBox1.Width -= 140;
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    allMessage.Controls.Add(pic);
                    allMessage.ScrollControlIntoView(pic);
                    allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                    pic.Left - allMessage.AutoScrollPosition.Y);
                });
            }
            else
            {
                allMessage.Controls.Add(pic);
                allMessage.ScrollControlIntoView(pic);
                allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                pic.Left - allMessage.AutoScrollPosition.Y);
            }
            allEmoji.Hide();
            sendData(7, $"{OpText.Text}@{name}.png");
        }

        private void ClientOnline_Click(object sender, EventArgs e)
        {
            ClientOnline ite = (sender as ClientOnline).Tag as ClientOnline;
            //string s = (sender as ClientOnline).Tag as string;
            OpText.Text = ite.lbName.Text;
            opAvt.Image = ite.avtClient.Image;
            foreach(ClientOnline item in listClientOnline) { 
                if(item.lbName.Text == ite.lbName.Text) {
                    item.BackColor = Color.FromArgb(232,243,254);
                    item.CheckClick = 1;
                    item.lbName.BackColor = Color.FromArgb(232, 243, 254);
                }
                else
                {
                    item.CheckClick = 0;
                    item.BackColor = DefaultBackColor;
                    item.lbName.BackColor = DefaultBackColor;
                }
                if (item.lbCount.Text != "0") {
                    item.NoRecDontSee = 0;item.lbCount.Hide();
                }
            }
            //client.Send(Serialize($"6{nameCLient.Text}@{s}"));
            sendData(6, $"6{nameCLient.Text}@{ite.lbName.Text}");
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
            if(!string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(Password.Text)){
                sendData(1, $"1{Username.Text}@{Password.Text}");
                //client.Send(Serialize($"1{Username.Text}@{Password.Text}"));
            }
            else{
                MessageBox.Show("Username or password can't be empty");
            }
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameRegister.Text) && !string.IsNullOrEmpty(PasswordRegister.Text) && PasswordRegister.Text == RePassRegister.Text){
                if(Avt.Image != null) { 
                    //sendData(2, $"2{UsernameRegister.Text}@{PasswordRegister.Text}");
                    sendFile(fileNamePath, $"{UsernameRegister.Text}@{PasswordRegister.Text}", 2);
                    //client.Send(Serialize($"2{UsernameRegister.Text}@{PasswordRegister.Text}"));
                }
                else {
                    MessageBox.Show("Please choose Avt for Account", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else{
                MessageBox.Show("Username or password can't be empty");
            }
        }
        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (checkServerOn == 1){
                if (name != ""){
                    sendData(3, $"3{name}");
                    //client.Send(Serialize($"3{name}"));
                    client.Close();
                    Application.Exit();
                }
                else{
                    sendData(3, $"3{name}");
                    //client.Send(Serialize($"3{name}"));
                    client.Close();
                    Application.Exit();
                }
            }
        }
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            if (checkServerOn == 1) { 
                if(name != "") {
                    sendData(3, $"3{name}");
                    //client.Send(Serialize($"3{name}"));
                    client.Close();
                    t.Stop();
                    Application.Exit();
                }
                else
                {
                    sendData(3, $"3{name}");
                    //client.Send(Serialize($"3{name}"));
                    client.Close();
                    t.Stop();
                    Application.Exit();
                }
            }
        }
        private void messageText_Enter(object sender, EventArgs e){
            
        }
        //Gửi message
        private void messageText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) { 
                if (!string.IsNullOrEmpty(messageText.Text)) {
                    this.Invoke(new Action(() =>
                    {
                        sendData(5, $"5{OpText.Text}@{messageText.Text}");
                        //client.Send(Serialize($"5{OpText.Text}@{messageText.Text}"));
                        var pic = new Send();
                        buitSizeSend(messageText.Text, pic);
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                        messageText.Text = "";
                    }));
                }
            }
        }
        //Gửi file
        public void sendFile(string fileName,string s,byte header) {
            try
            {
                string filePath = "";
                fileName = fileName.Replace("\\", "/");
                while (fileName.IndexOf("/") > -1)
                {
                    filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                    fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                }
                byte[] fileNameByte = Encoding.UTF8.GetBytes(fileName);
                if (fileNameByte.Length > (50 * 1024-100)){
                    MessageBox.Show("File size is more than 5Mb,please try with small file ","Message",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                string fullPath = filePath + fileName;
                byte[] fileData = File.ReadAllBytes(fullPath);
                byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                fileNameLen.CopyTo(clientData, 0);
                fileNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                //Thêm Header vào cho việc gửi size
                byte[] send = new byte[clientData.Length + s.Length+2];
                send[0] = header;
                send[1] = (byte)s.Length;
                for(int i = 0; i < s.Length; i++) { 
                    send[i+2]=Convert.ToByte(s[i]);
                }
                int k = s.Length+2;
                for (int i = 0; i < clientData.Length; i++)
                { send[k] = clientData[i];k++; }
                client.Send(send, 0, send.Length, 0);
            }
            catch(Exception ex) {
                if (ex.Message == "No connection could be made because the target machine actively refused it")
                    MessageBox.Show("File Sending fail. Because server not running.","Message",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                else
                    MessageBox.Show("File Sending fail." + ex.Message,"Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        //Gửi file
        private void guna2Button3_Click(object sender, EventArgs e){
            Thread t = new Thread((ThreadStart)(() => {
                FileDialog fd = new OpenFileDialog();
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = fd.FileName; 
                    fileName = fileName.Replace("\\", "/");
                    while (fileName.IndexOf("/") > -1)
                    {
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                    }
                    FilMessSend pic = new FilMessSend();
                    pic.guna2TextBox1.Text = fileName;
                    if (this.InvokeRequired){
                        this.BeginInvoke((MethodInvoker)delegate (){
                            allMessage.Controls.Add(pic);
                            allMessage.ScrollControlIntoView(pic);
                            allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                            pic.Left - allMessage.AutoScrollPosition.Y);
                        });
                    }
                    else{
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                    }
                    sendFile(fd.FileName,OpText.Text,11);
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        //GỬi  hình ảnh
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread((ThreadStart)(() => {
                OpenFileDialog OD = new OpenFileDialog();
                OD.FileName = "";
                OD.Filter = "Supported Images |*.jpg;*.jpeg;*.png";
                if (OD.ShowDialog() == DialogResult.OK)
                {
                    var pic = new ImageMessSend();
                    pic.guna2PictureBox1.Load(OD.FileName);
                    pic.guna2PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate ()
                        {
                            allMessage.Controls.Add(pic);
                            allMessage.ScrollControlIntoView(pic);
                            allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                            pic.Left - allMessage.AutoScrollPosition.Y);
                        });
                    }
                    else
                    {
                        allMessage.Controls.Add(pic);
                        allMessage.ScrollControlIntoView(pic);
                        allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                        pic.Left - allMessage.AutoScrollPosition.Y);
                    }
                    sendFile(OD.FileName,OpText.Text,11);
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        //Gửi like
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //Nhận đường link đến file Debug
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Image image = Image.FromFile(path.Substring(0,path.Length-9)+@"emoji\thumb-up.png");
            var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var bytes = ms.ToArray();
            var pic = new ImageMessSend();
            pic.guna2PictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
            pic.Height -= 100;
            pic.guna2PictureBox1.Height -= 100;
            pic.guna2PictureBox1.Width -= 140;
            pic.guna2PictureBox1.BorderRadius = 0;
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    allMessage.Controls.Add(pic);
                    allMessage.ScrollControlIntoView(pic);
                    allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                    pic.Left - allMessage.AutoScrollPosition.Y);
                });
            }
            else{
                allMessage.Controls.Add(pic);
                allMessage.ScrollControlIntoView(pic);
                allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                pic.Left - allMessage.AutoScrollPosition.Y);
            }
            sendData(7,$"{OpText.Text}@thumb-up.png");
        }

        private void guna2Button4_Click(object sender, EventArgs e){
            allEmoji.Show();
        }
        //Thêm Avt khi đăng kí
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Thread t = new Thread((ThreadStart)(() => {
                OpenFileDialog OD = new OpenFileDialog();
                OD.FileName = "";
                OD.Filter = "Supported Images |*.jpg;*.jpeg;*.png";
                if (OD.ShowDialog() == DialogResult.OK){
                    choseAvt = 1;
                    Avt.Load(OD.FileName);
                    Avt.SizeMode = PictureBoxSizeMode.StretchImage;
                    fileNamePath = OD.FileName;
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        string outFileVoceRecord = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string outputFileName;
        string nameVoice;
        //voiceRecord
        private void guna2Button5_Click(object sender, EventArgs e){
            this.Invoke(new Action(() =>{
                recordVoice.Show();
                s = 30;
                t.Start();
                getVoice();
            }));      
        }
        //D:\sql\MultiChatVersion2\\voiceRecord\1.mp3
        private void getVoice() {
            DirectoryInfo d = new DirectoryInfo(outFileVoceRecord.Substring(0, outFileVoceRecord.Length - 20) + @"\voiceRecord"); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.mp3"); //Getting Text files
            outputFileName = outFileVoceRecord.Substring(0, outFileVoceRecord.Length - 20) + @"\voiceRecord\" + $"{(Files.Length + 1).ToString()}.mp3";
            nameVoice = (Files.Length + 1).ToString()+".mp3";
            wave = new WaveIn();
            wave.WaveFormat = new WaveFormat(44100, 1);
            wave.DeviceNumber = 1;
            wave.DataAvailable += Wave_DataAvailable;
            wave.RecordingStopped += Wave_RecordingStopped;
            writer = new WaveFileWriter(outputFileName, wave.WaveFormat);
            wave.StartRecording();
        }

        private void Wave_RecordingStopped(object sender, StoppedEventArgs e){
            writer.Dispose();
        }

        private void Wave_DataAvailable(object sender, WaveInEventArgs e){
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void guna2Button6_Click(object sender, EventArgs e){
            stopRecording();
        }
        private void stopRecording() { 
            this.Invoke(new Action(() =>{
                wave.StopRecording();
                if (outputFileName == null)
                    return;
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = Path.GetDirectoryName(outputFileName),
                    UseShellExecute = true
                };
                Process.Start(processStartInfo);
                sendVoice();
                recordVoice.Hide();
                var pic = new voieMessSend();
                pic.outFileVoceRecord = outputFileName;
                pic.Load();
                allMessage.Controls.Add(pic);
                allMessage.ScrollControlIntoView(pic);
                allMessage.AutoScrollPosition = new Point(pic.Right - allMessage.AutoScrollPosition.X,
                                                                pic.Left - allMessage.AutoScrollPosition.Y);
            }));
        }
        private void sendVoice() {
            sendData(8, $"{OpText.Text}:{nameVoice}");
        }
        private void Client_FormClosing(object sender, FormClosingEventArgs e){

        }

        private void guna2Button8_Click(object sender, EventArgs e){
            this.Invoke(new Action(() => {
                flpListClient.Controls.Clear();
                foreach(ClientOnline item in listClientOnline) { 
                    flpListClient.Controls.Add(item);
                }
                guna2Button8.Enabled = false;
                guna2Button7.Enabled = true;
            }));
        }
        private void guna2Button7_Click(object sender, EventArgs e){
            this.Invoke(new Action(() => {
                flpListClient.Controls.Clear();
                guna2Button7.Enabled = false;
                guna2Button8.Enabled = true;
            }));
        }

        private void metroTabControl1_Click(object sender, EventArgs e){

        }
        
        private void addMemGroup_Click(object sender, EventArgs e){

        }
    }
}
