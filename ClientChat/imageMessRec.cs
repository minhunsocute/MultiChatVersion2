using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class imageMessRec : UserControl
    {
        public imageMessRec()
        {
            InitializeComponent();
        }
        public static string receivedPath = "C:/Users/ASUS/OneDrive/caro/OneDrive/Desktop/";
        public byte[] clientData = new byte[1024 * 5000];
        public int receivedBytesLen;
        public void RecvImage()
        {
            try
            {
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.UTF8.GetString(clientData, 4, fileNameLen);

                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + "/" + fileName, FileMode.Append));
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                bWrite.Close();
            }
            catch
            {
                MessageBox.Show("Cannot dowload this Image", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e){
            RecvImage();
        }
    }
}
