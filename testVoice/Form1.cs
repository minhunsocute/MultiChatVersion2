using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testVoice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadDevices();
            WaveFileReader reader = new WaveFileReader(@"D:\sql\MultiChatVersion2\voiceRecord\3.mp3");
            TimeSpan duration = reader.TotalTime;
            textBox1.Text = (duration.Seconds.ToString());
        }
        WaveIn wave;
        WaveFileWriter writer;
        string outputFileName;
        private void LoadDevices()
        {
            for(int deviceId = 0;deviceId < WaveIn.DeviceCount; deviceId++)
            {
                var deviceInfo = WaveIn.GetCapabilities(deviceId);
                comboBox1.Items.Add(deviceInfo.ProductName);
            }   
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                var deviceInfo = WaveOut.GetCapabilities(deviceId);
                comboBox2.Items.Add(deviceInfo.ProductName);
            }
        }
        string outFileVoceRecord = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private void button1_Click(object sender, EventArgs e)
        {
            /*var dialog = new SaveFileDialog();
            dialog.Filter = "Wave files | *.mp3";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;*/
            DirectoryInfo d = new DirectoryInfo(outFileVoceRecord.Substring(0, outFileVoceRecord.Length - 20) + @"\voiceRecord"); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.mp3"); //Getting Text files
            outputFileName = outFileVoceRecord.Substring(0, outFileVoceRecord.Length - 20) + @"\voiceRecord\" + $"{(Files.Length + 1).ToString()}.mp3";
            button1.Enabled = false;
            button2.Enabled = true;
            wave = new WaveIn();
            wave.WaveFormat = new WaveFormat(44100, 1);
            wave.DeviceNumber = 1;
            wave.DataAvailable += Wave_DataAvailable;
            wave.RecordingStopped += Wave_RecordingStopped;
            writer = new WaveFileWriter(outputFileName, wave.WaveFormat);
            wave.StartRecording();
        }

        private void Wave_RecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();
            button1.Enabled = true;
            button2.Enabled = false;    
        }

        private void Wave_DataAvailable(object sender, WaveInEventArgs e)
        {
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            wave.StopRecording();
            if (outputFileName == null)
                return;
            var processStartInfo = new ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(outputFileName),
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SoundPlayer simpleSound = new SoundPlayer(@"D:\sql\MultiChatVersion2\voiceRecord\1.mp3");
            simpleSound.Play();
        }
    }
}
