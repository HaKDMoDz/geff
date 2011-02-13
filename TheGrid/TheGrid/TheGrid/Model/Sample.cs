using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NAudio.Wave;

namespace TheGrid.Model
{
    public class Sample
    {
        public Channel Channel { get; set; }
        public String FileName { get; set; }
        public String Name { get { return Path.GetFileNameWithoutExtension(FileName); } }
        public WaveStream WaveStream;

        public Sample()
        {
        }
        
        public Sample(Channel channel, string fileName)
        {
            this.Channel = channel;
            this.FileName = fileName;
            
            //this.WaveStream = CreateInputStream(FileName);
        }

        private WaveStream CreateInputStream(string fileName)
        {
            WaveChannel32 inputStream;
            if (fileName.EndsWith(".mp3"))
            {
                WaveStream reader = new Mp3FileReader(fileName);
                inputStream = new WaveChannel32(reader);
            }
            else
            {
                WaveStream reader = new NAudio.Wave.WaveFileReader(fileName);
                inputStream = new WaveChannel32(reader);
            }
            
            return inputStream;
        }
    }
}
