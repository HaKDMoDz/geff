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

        public Partition Partition;
        private ClickableImage imgPlay;
        private ClickableImage imgPause;
        private ClickableImage imgStop;

        public Ribbon(UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            CreationTime = TimeSpan.MaxValue;

            Visible = true;
            Alive = true;
            this.ListUIChildren = new List<UIComponent>();

            Rec = new Rectangle(0, 0, (int)Render.ScreenWidth, (int)(0.2f * Render.ScreenHeight));

            Partition = new Partition(this, uiLogic, creationTime);
            this.ListUIChildren.Add(Partition);

            imgPlay = new ClickableImage(this.UI, this.CreationTime, "Play", Render.texPlay, Render.texPlay, new Vector2(Partition.Rec.Right + (int)(0.03f * Render.ScreenWidth) + MARGE * 2, Partition.Rec.Y));
            imgPause = new ClickableImage(this.UI, this.CreationTime, "Pause", Render.texPause, Render.texPause, new Vector2(Partition.Rec.Right + (int)(0.03f * Render.ScreenWidth) + MARGE * 2, Partition.Rec.Y));
            imgPause.Visible = false;
            imgStop = new ClickableImage(this.UI, this.CreationTime, "Stop", Render.texStop, Render.texStop, new Vector2(Partition.Rec.Right + (int)(0.03f * Render.ScreenWidth) + MARGE * 3 + Render.texPlay.Width, Partition.Rec.Y));

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
            Rectangle recManualSpeed = new Rectangle(Partition.Rec.Right + MARGE, Partition.Rec.Y, (int)(0.03f * Render.ScreenWidth), (int)((float)Partition.Rec.Height * Context.SpeedFactor / 4f));
            Rectangle rec = new Rectangle(Rec.X, Rec.Y, Rec.Width, (int)((float)Rec.Height * 1.3f));

            Render.SpriteBatch.Draw(Render.texEmptyGradient, rec, new Color(0.1f, 0.1f, 0.1f, 0.95f));//, 0f, Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);

            Render.SpriteBatch.Draw(Render.texEmptyGradient, recManualSpeed, Color.White);

            base.Draw(gameTime);
        }
    }
}
