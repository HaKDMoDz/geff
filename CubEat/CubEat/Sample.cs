using System;
using System.Collections.Generic;
using System.Text;

namespace CubEat
{
    public class Sample
    {
        public SampleModel SampleModel { get; set; }

        public Sample(SampleModel sampleModel)
        {
            this.SampleModel = sampleModel;
        }

        //public void PlaySample()
        //{
        //    this.SampleModel.PlaySample();
        //}
    }
}
