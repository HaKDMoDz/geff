using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;

namespace TheGrid.Model.UI
{
    public class Ribbon : UIComponent
    {
        private Partition partition;
        public const int MARGE = 5;

        public Ribbon(UILogic uiLogic)
            : base(uiLogic)
        {
            Visible = true;
            Alive = true;
            this.ListUIChildren = new List<UIComponent>();

            Rec = new Rectangle(0, 0, (int)Render.ScreenWidth, (int)(0.2f * Render.ScreenHeight)); 

            partition = new Partition(this, uiLogic);
            this.ListUIChildren.Add(partition);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle recManualSpeed = new Rectangle(partition.Rec.X + MARGE, partition.Rec.X, (int)(0.1f * Render.ScreenWidth), partition.Rec.Height);

            Render.SpriteBatch.Draw(Render.texEmpty, Rec, Color.Black);

            Render.SpriteBatch.Draw(Render.texEmpty, recManualSpeed, Color.White);




            base.Draw(gameTime);
        }
    }
}
