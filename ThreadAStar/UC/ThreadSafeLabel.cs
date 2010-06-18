using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ThreadAStar.UC
{
    public partial class ThreadSafeLabel : Label
    {
        delegate void SetText_Callback(string text);

        public void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                SetText_Callback call = new SetText_Callback(SetText);
                this.Invoke(call, text);
            }
            else
            {
                this.Text = text;
            }
        }
    }
}
