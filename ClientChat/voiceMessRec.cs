using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class voiceMessRec : UserControl
    {
        public string outFileVoceRecord ;
        System.Timers.Timer t;
        int s;
        public voiceMessRec()
        {
            InitializeComponent();
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += OntimeEvent;
        }

        private void OntimeEvent(object sender, ElapsedEventArgs e){
            this.Invoke(new Action(() =>{
                s -= 1;
                if (s <= 0) {
                    t.Stop();
                    s = 0;
                }
                if (s >= 10) { 
                    timeOut.Text = string.Format($"00:{s.ToString()}");
                }
                else 
                    timeOut.Text = string.Format($"00:0{s.ToString()}");
            }));
        }
        private void guna2Button4_Click(object sender, EventArgs e){
            t.Start();
            SoundPlayer simpleSound = new SoundPlayer(outFileVoceRecord);
            simpleSound.Play();
        }
        public void Load() {
            WaveFileReader reader = new WaveFileReader(outFileVoceRecord);
            TimeSpan duration = reader.TotalTime;
            s = duration.Seconds;
            if (s >= 10)
                timeOut.Text = $"00:{s.ToString()}";
            else
                timeOut.Text = $"00:{s.ToString()}";
        }
    }
}
