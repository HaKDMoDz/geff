using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.Model
{
    [Serializable]
    public class TimeValue<T>
    {
        public TimeSpan Time { get; set; }
        public T Value { get; set; }

        public TimeValue() { }

        public TimeValue(TimeSpan time, T value)
        {
            this.Time = time;
            this.Value = value;
        }

        public static bool operator ==(TimeValue<T> a, TimeValue<T> b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Time == b.Time && a.Value.Equals(b.Value);
        }

        public static bool operator !=(TimeValue<T> a, TimeValue<T> b)
        {
            return !(a == b);
        }
    }
}
