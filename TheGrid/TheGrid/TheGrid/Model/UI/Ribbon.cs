using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using TheGrid.Common;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using System.IO;
using TheGrid.Model.UI.Menu;
using TheGrid.Logic.Controller;

namespace TheGrid.Model.UI
{
    public class Ribbon : UIComponent
    {
        public static int HEIGHT = 0;

        public Partition Partition;
        private ClickableImage imgPlay;
        private ClickableImage imgPause;
        private ClickableImage imgStop;
        public Rectangle RecMenuBar;
        private Rectangle recBackground;
        private Vector2 vecTime;
        private CircularMenu menu;

        public Ribbon(UILogic uiLogic, UIComponent parent, TimeSpan creationTime)
            : base(uiLogic, parent, creationTime)
        {
            CreationTime = creationTime;

            Visible = true;
            Alive = true;

            Init();
        }

        public void Init()
        {
            this.ListUIChildren = new List<UIComponent>();

            Ribbon.HEIGHT = (int)(0.2f * Render.ScreenHeight);
            Rec = new Rectangle(0, 0, (int)Render.ScreenWidth, Ribbon.HEIGHT);
            RecMenuBar = new Rectangle(Ribbon.MARGE + 100, 0, Rec.Width - 100 - Ribbon.MARGE * 2, 30);
            recBackground = new Rectangle(Rec.X, Rec.Y, Rec.Width, (int)((float)Rec.Height * 1.2f));
            vecTime = new Vector2(RecMenuBar.Left + MARGE * 4, RecMenuBar.Top + RecMenuBar.Height / 2 - Render.FontTextSmall.MeasureString("0").Y / 2);

            BPMMeter BPMMeter = new BPMMeter(this, this.UI, GetNewTimeSpan());
            BPMMeter.Init();
            this.ListUIChildren.Add(BPMMeter);

            Partition = new Partition(this, this.UI, GetNewTimeSpan());
            Partition.Init();
            this.ListUIChildren.Add(Partition);

            imgPlay = new ClickableImage(UI, this, GetNewTimeSpan(), "Play", Render.texPlay, Render.texPlay, new Vector2(BPMMeter.Rec.Right + MARGE * 2, Partition.Rec.Y + RecMenuBar.Height / 2 - Render.texPlay.Height / 2));
            imgPause = new ClickableImage(UI, this, GetNewTimeSpan(), "Pause", Render.texPause, Render.texPause, new Vector2(BPMMeter.Rec.Right + MARGE * 2, Partition.Rec.Y + RecMenuBar.Height / 2 - Render.texPause.Height / 2));
            imgPause.Visible = false;
            imgStop = new ClickableImage(UI, this, GetNewTimeSpan(), "Stop", Render.texStop, Render.texStop, new Vector2(BPMMeter.Rec.Right + MARGE * 3 + Render.texPlay.Width, Partition.Rec.Y + RecMenuBar.Height / 2 - Render.texStop.Height / 2));

            imgPlay.ClickImage += new ClickableImage.ClickImageHandler(imgPlay_ClickImage);
            imgPause.ClickImage += new ClickableImage.ClickImageHandler(imgPause_ClickImage);
            imgStop.ClickImage += new ClickableImage.ClickImageHandler(imgStop_ClickImage);

            this.ListUIChildren.Add(imgPlay);
            this.ListUIChildren.Add(imgPause);
            this.ListUIChildren.Add(imgStop);
        }

        void imgPlay_ClickImage(ClickableImage image, MouseState mouseState, GameTime gameTime)
        {
            GamePlay.Play();
        }

        void imgPause_ClickImage(ClickableImage image, MouseState mouseState, GameTime gameTime)
        {
            GamePlay.Pause();
        }

        void imgStop_ClickImage(ClickableImage image, MouseState mouseState, GameTime gameTime)
        {
            GamePlay.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            imgPlay.Visible = Context.StatePlaying != StatePlaying.Playing;
            imgPause.Visible = Context.StatePlaying == StatePlaying.Playing;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmptyGradient, recBackground, VisualStyle.BackColorDark);
            Render.SpriteBatch.Draw(Render.texEmpty, RecMenuBar, VisualStyle.BackColorDark);

            Render.SpriteBatch.DrawString(Render.FontTextSmall, Context.Map.BPM.ToString() + " BPM", new Vector2(vecTime.X + 120, vecTime.Y), Color.DarkGray);
            Render.SpriteBatch.DrawString(Render.FontTextSmall, string.Format("{0:00}:{1:00}:{2:00}.{3:000}", Context.Time.Hours, Context.Time.Minutes, Context.Time.Seconds, Context.Time.Milliseconds), vecTime, Color.DarkGray);

            base.Draw(gameTime);
        }
    }
}
