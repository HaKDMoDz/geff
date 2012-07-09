using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewScore
{
    public class Music
    {
        public List<Channel> ListChanel { get; set; }
        public int MaxMeasure { get; set; }

        public void AddChannel(Channel currentChannel)
        {
            ListChanel.Add(currentChannel);
            MaxMeasure = ListChanel.Max(c => c.ListMeasure.Count);
        }
    }
}
