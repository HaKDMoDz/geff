using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using TheGrid.Common;

namespace TheGrid.Model.UI
{
    public class Ribbon : UIComponent
    {
        public const int MARGE = 5;

        private Partition partition;
        private ClickableImage imgPlay;
        private ClickableImage imgPause;
        private ClickableImage imgStop;

        public Ribbon(UILogic uiLogic)
            : base(uiLogic)
        {
            Visible = true;
            Alive = true;
            this.ListUIChildren = new List<UIComponent>();

            Rec = new Rectangle(0, 0, (int)Render.ScreenWidth, (int)(0.2f * Render.ScreenHeight));

            partition = new Partition(this, uiLogic);
            this.ListUIChildren.Add(partition);

            imgPlay = new ClickableImage(this.UI, Render.texPlay, Render.texPlay, new Vector2(partition.Rec.Right + (int)(0.03f * Render.ScreenWidth) + MARGE * 2, partition.Rec.Y));
            imgPause = new ClickableImage(this.UI, Render.texPause, Render.texPause, new Vector2(partition.Rec.Right + (int)(0.03f * Render.ScreenWidth) + MARGE * 2, partition.Rec.Y));
            imgPause.Visible = false;
            imgStop = new ClickableImage(this.UI, Render.texStop, Render.texStop, new Vector2(partition.Rec.Right + (int)(0.03f * Render.ScreenWidth) + MARGE * 3 + Render.texPlay.Width, partition.Rec.Y));

            imgPlay.ClickImage += new ClickableImage.ClickImageHandler(imgPlay_ClickImage);
            imgPause.ClickImage += new ClickableImage.ClickImageHandler(imgPause_ClickImage);
            imgStop.ClickImage += new ClickableImage.ClickImageHandler(imgStop_ClickImage);

            this.ListUIChildren.Add(imgPlay);
            this.ListUIChildren.Add(imgPause);
            this.ListUIChildren.Add(imgStop);
        }

        void imgPlay_ClickImage(ClickableImage image, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            GamePlay.Play();
        }

        void imgPause_ClickImage(ClickableImage image, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            GamePlay.Pause();
        }

        void imgStop_ClickImage(ClickableImage image, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            GamePlay.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            imgPlay.Visible = !Context.IsPlaying;
            imgPause.Visible = Context.IsPlaying;
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle recManualSpeed = new Rectangle(partition.Rec.Right + MARGE, partition.Rec.Y, (int)(0.03f * Render.ScreenWidth), (int)((float)partition.Rec.Height * Context.SpeedFactor/4f));

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, new Color(0.1f, 0.1f, 0.1f));

            Render.SpriteBatch.Draw(Render.texEmptyGradient, recManualSpeed, Color.White);

            base.Draw(gameTime);
        }
    }
}
