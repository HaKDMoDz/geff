using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    [Serializable()]
    public class Scene
    {
        public List<ComponentBase> listComponent = new List<ComponentBase>();

        public Scene()
        {
            listComponent = new List<ComponentBase>();
        }
    }
}
