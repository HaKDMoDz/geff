using System;
using System.Collections.Generic;
using System.Text;

namespace CubEat
{
    public class LibraySample
    {
        public List<SampleModel> ListSampleModel { get; set; }
        public String Name { get; set; }

        public LibraySample(String name)
        {
            ListSampleModel = new List<SampleModel>();
            this.Name = name;
        }
    }
}
