using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TheGrid.Logic.Render
{
    public class MeshHexa
    {
        public ModelMesh[] Direction { get; set; }
        public ModelMesh[] Repeater { get; set; }
        public ModelMesh Body { get; set; }
        public ModelMesh[] SpeedHigh { get; set; }
        public ModelMesh[] SpeedLow { get; set; }
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public MeshHexa(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Model = model;

            Direction = new ModelMesh[6];
            for (int i = 1; i < 7; i++)
            {
                Direction[i - 1] = model.Meshes["Direction_" + i.ToString()];
            }

            Repeater = new ModelMesh[6];
            for (int i = 1; i < 7; i++)
            {
                Repeater[i - 1] = model.Meshes["Repeater_" + i.ToString()];
            }

            SpeedHigh = new ModelMesh[8];
            for (int i = 1; i < 9; i++)
            {
                SpeedHigh[i - 1] = model.Meshes["Speed_H_" + i.ToString()];
            }

            Body = model.Meshes["Body"];
        }
    }
}
