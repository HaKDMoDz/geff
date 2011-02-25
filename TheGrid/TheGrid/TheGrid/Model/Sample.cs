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
        public TimeSpan Duration { get; set; }
        public float Frequency { get; set; }

        public Sample()
        {
        }
        
        public Sample(Channel channel, string fileName)
        {
            this.Channel = channel;
            this.FileName = fileName;
        }
    }
}
