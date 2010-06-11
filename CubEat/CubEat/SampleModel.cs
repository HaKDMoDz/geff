using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Media;
using System.IO;
using System.Windows.Forms;
using IrrKlang;

namespace CubEat
{
    public class SampleModel
    {
        public String Name { get; set; }
        public Color Color { get; set; }
        public int Length { get; set; }
        public ISoundSource SoundSource { get; set; }
        public int SampleModelId { get; set; }
        public LibraySample LibraySample { get; set; }

        private SoundPlayer soundPlayer = new SoundPlayer();

        public SampleModel(LibraySample libraySample, string name, Color color, int length, int sampleModelId)
        {
            this.LibraySample = libraySample;
            this.Name = name;
            this.Color = color;
            this.Length = length;
            this.SampleModelId = sampleModelId;
        }

        //public void PlaySample()
        //{
        //    soundPlayer.Play();
        //}
    }
}
