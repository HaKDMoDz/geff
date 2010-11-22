using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NewFlowar.Common;
using SkinnedModel;

namespace NewFlowar.Model.Minion
{
    public abstract class MinionBase
    {
        public abstract String ModelName { get; }
        public abstract float Speed { get; set; }

        public Vector3 Location { get; set; }
        public Vector2 Direction { get; set; }
        public Cell CurrentCell { get; set; }
        public List<int> Path { get; set; }
        public float PathLength { get; set; }
        public float TraveledLength { get; set; }
        public float Angle { get; set; }
        public TimeSpan BornTime { get; set; }
        public AnimationPlayer AnimationPlayer { get; set; }

        public MinionBase(Cell cellStartLocation)
        {
            Path = new List<int>();
            this.CurrentCell = cellStartLocation;
            this.Location = Tools.GetVector3(cellStartLocation.Location);
            this.BornTime = DateTime.Now.TimeOfDay;
        }

        public void InitAnimationPlayer(Microsoft.Xna.Framework.Graphics.Model model)
        {
            // Look up our custom skinning information.
            SkinningData skinningData = model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            AnimationPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips.Values.ElementAt<AnimationClip>(0);//["Walking"];

            AnimationPlayer.StartClip(clip);
        }
    }
}
