using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class Scene
    {
        public List<ComponentBase> listComponent = new List<ComponentBase>();

        [XmlIgnore]
        public Cutting CuttingFront { get; set; }

        [XmlIgnore]
        public Cutting CuttingTop { get; set; }

        public Scene()
        {
            listComponent = new List<ComponentBase>();
        }
    }
}
