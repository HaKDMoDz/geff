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

namespace TheGrid.Model.UI
{
    public class Ribbon : UIComponent
    {
        public const int MARGE = 5;
        public static int HEIGHT = 0;

        public Partition Partition;
        private ClickableImage imgPlay;
        private ClickableImage imgPause;
        private ClickableImage imgStop;
        //private ClickableText txtNewMap;
        //private ClickableText txtLoadMap;
        //private ClickableText txtSaveMap;
        public Rectangle RecMenuBar;
        private Rectangle recManualSpeedBackGround;
        private CircularMenu menu;

        public Ribbon(UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            CreationTime = creationTime;

            Visible = true;
            Alive = true;
            this.ListUIChildren = new List<UIComponent>();

            Ribbon.HEIGHT = (int)(0.2f * Render.ScreenHeight);
            Rec = new Rectangle(0, 0, (int)Render.ScreenWidth, Ribbon.HEIGHT);
            RecMenuBar = new Rectangle(Ribbon.MARGE + 100, Ribbon.MARGE, Rec.Width - 100 - Ribbon.MARGE * 2, 20);
            recManualSpeedBackGround = new Rectangle(RecMenuBar.Left + 200, RecMenuBar.Top + MARGE, 100, RecMenuBar.Height - MARGE * 2);

            Partition = new Partition(this, uiLogic, GetNewTimeSpan());
            this.ListUIChildren.Add(Partition);

            imgPlay = new ClickableImage(this.UI, GetNewTimeSpan(), "Play", Render.texPlay, Render.texPlay, new Vector2(Partition.Rec.Right + 30 + MARGE * 2, Partition.Rec.Y));
            imgPause = new ClickableImage(this.UI, GetNewTimeSpan(), "Pause", Render.texPause, Render.texPause, new Vector2(Partition.Rec.Right + 30 + MARGE * 2, Partition.Rec.Y));
            imgPause.Visible = false;
            imgStop = new ClickableImage(this.UI, GetNewTimeSpan(), "Stop", Render.texStop, Render.texStop, new Vector2(Partition.Rec.Right + 30 + MARGE * 3 + Render.texPlay.Width, Partition.Rec.Y));

            imgPlay.ClickImage += new ClickableImage.ClickImageHandler(imgPlay_ClickImage);
            imgPause.ClickImage += new ClickableImage.ClickImageHandler(imgPause_ClickImage);
            imgStop.ClickImage += new ClickableImage.ClickImageHandler(imgStop_ClickImage);

            //txtNewMap = new ClickableText(this.UI, GetNewTimeSpan(), "FontMenu", "New", new Vector2(imgStop.Rec.Right + MARGE, Partition.Rec.Y), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.Transparent, VisualStyle.BackForeColorMouseOver, false);
            //txtLoadMap = new ClickableText(this.UI, GetNewTimeSpan(), "FontMenu", "Load", new Vector2(imgStop.Rec.Right + MARGE, txtNewMap.Rec.Bottom + MARGE), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.Transparent, VisualStyle.BackForeColorMouseOver, false);
            //txtSaveMap = new ClickableText(this.UI, GetNewTimeSpan(), "FontMenu", "Save", new Vector2(imgStop.Rec.Right + MARGE, txtLoadMap.Rec.Bottom + MARGE), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.Transparent, VisualStyle.BackForeColorMouseOver, false);

            //txtNewMap.ClickText += new ClickableText.ClickTextHandler(txtNewMap_ClickText);
            //txtLoadMap.ClickText += new ClickableText.ClickTextHandler(txtLoadMap_ClickText);
            //txtSaveMap.ClickText += new ClickableText.ClickTextHandler(txtSaveMap_ClickText);

            //this.ListUIChildren.Add(txtNewMap);
            //this.ListUIChildren.Add(txtLoadMap);
            //this.ListUIChildren.Add(txtSaveMap);

            this.ListUIChildren.Add(imgPlay);
            this.ListUIChildren.Add(imgPause);
            this.ListUIChildren.Add(imgStop);

            //CreateCircularMenu();
        }

        void txtNewMap_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            ListLibrary listLibrary = new ListLibrary(this.UI, gameTime.TotalGameTime);
            this.ListUIChildren.Add(listLibrary);
        }

        void txtLoadMap_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            ListFile listFile = new ListFile(this.UI, gameTime.TotalGameTime, Path.Combine(Directory.GetParent(Application.ExecutablePath).FullName, @"Level\"));
            this.ListUIChildren.Add(listFile);
        }

        void txtSaveMap_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = Path.Combine(Directory.GetParent(Application.ExecutablePath).FullName, @"Level\");
            dlg.Filter = "Niveau The Grid (*.xml)|*.xml";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FileSystem.SaveLevel(Context.Map, Path.GetFileNameWithoutExtension(dlg.FileName));
            }
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

        #region Menu
        private void CreateCircularMenu()
        {
            menu = new CircularMenu(UI, GetNewTimeSpan(), null, null, null, false, true);
            menu.Visible = true;

            Item itemNew = new Item(menu, "New");
            itemNew.Selected += new Item.SelectedHandler(itemNew_Selected);
            menu.Items.Add(itemNew);

            Item itemLoad = new Item(menu, "Load");
            itemLoad.Selected += new Item.SelectedHandler(itemLoad_Selected);
            menu.Items.Add(itemLoad);

            Item itemSave = new Item(menu, "Save");
            itemSave.Selected += new Item.SelectedHandler(itemSave_Selected);
            menu.Items.Add(itemSave);

            Item itemMenu = new Item(menu, "Menu");
            itemMenu.Selected += new Item.SelectedHandler(itemMenu_Selected);
            menu.Items.Add(itemMenu);

            menu.Location = new Vector2(0f, 0f);
            menu.nbVertex = menu.Items.Count * 4;
            menu.AngleDelta = MathHelper.Pi/12;
            menu.PercentVisibility = 0f;
            menu.State = MenuState.Closed;
            menu.EffectVertex = Render.effectUI;
            menu.EffectSprite= Render.effectUISprite;
            menu.IsUI = true;
            menu.IsTurnMode = true;
            menu.MaxAngle = 2 * MathHelper.Pi / 3;

            menu.MinAngleDelta = MathHelper.Pi / 12;
            menu.MaxAngleDelta = 7 * MathHelper.Pi / 12;
            menu.CreateVertex();

            ListUIChildren.Add(menu);
        }

        void itemMenu_Selected(Item item, GameTime gameTime)
        {
            if(item.ParentMenu.State == MenuState.Closed)
                item.ParentMenu.Open(gameTime);
        }

        void itemNew_Selected(Item item, GameTime gameTime)
        {
        }

        void itemLoad_Selected(Item item, GameTime gameTime)
        {
        }
        
        void itemSave_Selected(Item item, GameTime gameTime)
        {
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            imgPlay.Visible = !Context.IsPlaying;
            imgPause.Visible = Context.IsPlaying;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle recManualSpeed = new Rectangle(recManualSpeedBackGround.Left, recManualSpeedBackGround.Top, (int)((float)recManualSpeedBackGround.Width * Context.Map.SpeedFactor / 2f), recManualSpeedBackGround.Height);

            Rectangle rec = new Rectangle(Rec.X, Rec.Y, Rec.Width, (int)((float)Rec.Height * 1.3f));

            Vector2 vecTime = new Vector2(RecMenuBar.Left + MARGE * 4, RecMenuBar.Top + RecMenuBar.Height / 2 - Render.FontTextSmall.MeasureString("0").Y / 2);

            Render.SpriteBatch.Draw(Render.texEmptyGradient, rec, VisualStyle.BackColorLight);
            Render.SpriteBatch.Draw(Render.texEmpty, RecMenuBar, VisualStyle.BackColorDark);

            Render.SpriteBatch.Draw(Render.texEmpty, recManualSpeedBackGround, VisualStyle.BackColorLight);
            Render.SpriteBatch.Draw(Render.texEmpty, recManualSpeed, VisualStyle.BackForeColorChecked);

            Render.SpriteBatch.DrawString(Render.FontTextSmall, Context.Map.BPM.ToString() + " BPM", new Vector2(vecTime.X + 100, vecTime.Y), Color.DarkGray);
            Render.SpriteBatch.DrawString(Render.FontTextSmall, string.Format("{0:00}:{1:00}:{2:00}", Context.Time.Hours, Context.Time.Minutes, Context.Time.Seconds), vecTime, Color.DarkGray);

            base.Draw(gameTime);
        }
    }
}
