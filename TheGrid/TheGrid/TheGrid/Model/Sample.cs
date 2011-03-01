using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NAudio.Wave;
using System.Xml.Serialization;

namespace TheGrid.Model
{
    public class Sample
    {
        [XmlIgnore]
        public Channel Channel { get; set; }
        public String FileName { get; set; }
        [XmlIgnore]
        public String Name { get { return Path.GetFileNameWithoutExtension(FileName); } }
        [XmlIgnore]
        public TimeSpan Duration { get; set; }
        public float Frequency { get; set; }
        public float NoteKey { get; set; }

        public Sample()
        {
            Frequency = -1f;
        }
        
        public Sample(Channel channel, string fileName)
        {
            Frequency = -1f;
            this.Channel = channel;
            this.FileName = fileName;
        }
    }
}
