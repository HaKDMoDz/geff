using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using TheGrid.Model.UI.Menu;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI.Note
{
    public class NotePanel : UIComponent
    {
        public ComponentState State { get; set; }
        public float leftPartWidth = 0f;
        public Sample Sample { get; set; }

        public NotePanel(UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            Alive = true;
            Visible = true;

            leftPartWidth = 350f;// 0.3f * UI.GameEngine.Render.ScreenWidth;
            Rec = new Rectangle(0, (int)(0.4f * UI.GameEngine.Render.ScreenHeight), (int)UI.GameEngine.Render.ScreenWidth, (int)(0.6f * UI.GameEngine.Render.ScreenHeight));

            Keyboard keyboard = new Keyboard(this, UI, GetNewTimeSpan());
            ListUIChildren = new List<UIComponent>();
            ListUIChildren.Add(keyboard);

            //--- Boutons
            Vector2 vec = new Vector2(0, Rec.Top + MARGE);
            ClickableText txtStartCell = new ClickableText(UI, GetNewTimeSpan(), Render.FontTextSmall, "Céllule de départ", new Vector2(MARGE, vec.Y), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
            txtStartCell.ClickText += new ClickableText.ClickTextHandler(txtStartCell_ClickText);
            ListUIChildren.Add(txtStartCell);
            vec.Y += MARGE + txtStartCell.Rec.Height;

            ClickableText txtSilence = new ClickableText(UI, GetNewTimeSpan(), Render.FontTextSmall, "Silence", new Vector2(MARGE, vec.Y), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
            txtSilence.ClickText += new ClickableText.ClickTextHandler(txtSilence_ClickText);
            ListUIChildren.Add(txtSilence);
            vec.Y += MARGE + txtSilence.Rec.Height;

            ClickableText txtCapture = new ClickableText(UI, GetNewTimeSpan(), Render.FontTextSmall, "Capture", new Vector2(MARGE, vec.Y), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
            txtCapture.ClickText += new ClickableText.ClickTextHandler(txtCapture_ClickText);
            ListUIChildren.Add(txtCapture);
            vec.Y += MARGE + txtCapture.Rec.Height;
            //---

            //--- Note duration
            CreateNoteDurationMenu();
            //---

            //--- Channel
            CreateChannelMenu();
            //---

            //--- Liste des samples
            ListSample listSample = new ListSample(UI, GetNewTimeSpan(), null, Context.Map.Channels[1], new Rectangle(MARGE+(int)leftPartWidth/2, Rec.Top + Rec.Height/2, (int)leftPartWidth/2, Rec.Height/2), Render.FontTextSmall);
            listSample.Rec = new Rectangle();
            ListUIChildren.Add(listSample);
            //---

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        void txtCapture_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
        }

        void txtSilence_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
        }

        void txtStartCell_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        private void CreateNoteDurationMenu()
        {
            ListUIChildren.RemoveAll(ui => ui is CircularMenu);

            float sizeMenu = 75f;
            CircularMenu menuDuration = new CircularMenu(UI, GetNewTimeSpan(), null, null, null, true);
            menuDuration.LocalSize = (int)sizeMenu;
            menuDuration.Location = new Vector2(Rec.X + MARGE + leftPartWidth *3/4, Rec.Y + Rec.Height / 4);

            //---
            Item itemDurationReset = new Item(menuDuration, "Reset", 1);
            itemDurationReset.Selected += new Item.SelectedHandler(itemDuration_Selected);
            menuDuration.Items.Add(itemDurationReset);

            Item itemDuration2 = new Item(menuDuration, "Duration2", 2);
            itemDuration2.Selected += new Item.SelectedHandler(itemDuration_Selected);
            menuDuration.Items.Add(itemDuration2);


            Item itemDuration3 = new Item(menuDuration, "Duration3", 4);
            itemDuration3.Selected += new Item.SelectedHandler(itemDuration_Selected);
            menuDuration.Items.Add(itemDuration3);

            Item itemDuration4 = new Item(menuDuration, "Duration4", 8);
            itemDuration4.Selected += new Item.SelectedHandler(itemDuration_Selected);
            menuDuration.Items.Add(itemDuration4);
            //---

            menuDuration.nbVertex = menuDuration.Items.Count * 4;
            menuDuration.Visible = true;
            menuDuration.Alive = true;
            menuDuration.PercentVisibility = 1f;
            menuDuration.State = ComponentState.Opened;
            menuDuration.EffectVertex = Render.effectUI;
            menuDuration.EffectSprite = Render.effectUISprite;
            menuDuration.IsUI = true;

            ListUIChildren.Add(menuDuration);
        }

        void itemDuration_Selected(Item item, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        private void CreateChannelMenu()
        {
            ListUIChildren.RemoveAll(ui => ui is CircularMenu);

            float sizeMenu = 75f;
            CircularMenu menuChannel = new CircularMenu(UI, GetNewTimeSpan(), null, null, null, false);
            menuChannel.LocalSize = (int)sizeMenu;
            menuChannel.Location = new Vector2(Rec.X + MARGE + leftPartWidth / 4, Rec.Y + Rec.Height * 3 / 4);

            for (int i = 0; i < Context.Map.Channels.Count; i++)
            {
                Item itemChannel = new Item(menuChannel, Context.Map.Channels[i].Name, i);
                itemChannel.Color = Context.Map.Channels[i].Color;
                itemChannel.Selected += new Item.SelectedHandler(itemChannel_Selected);

                menuChannel.Items.Add(itemChannel);
            }

            menuChannel.nbVertex = menuChannel.Items.Count * 4;
            menuChannel.Visible = true;
            menuChannel.Alive = true;
            menuChannel.PercentVisibility = 1f;
            menuChannel.State = ComponentState.Opened;
            menuChannel.EffectVertex = Render.effectUI;
            menuChannel.EffectSprite = Render.effectUISprite;
            menuChannel.IsUI = true;

            ListUIChildren.Add(menuChannel);
        }

        void itemChannel_Selected(Item item, GameTime gameTime)
        {
        }

        public override void Update(GameTime gametime)
        {
            //if (Enter != null && Leave != null && !UI.GameEngine.Controller.IsMouseOffScreen())
            //{
            //    int countItemSelected = Items.Count(i => i.MouseOver);

            //    if (prevCountItemSelected == 0 && countItemSelected > 0)
            //        Enter(gametime);
            //    if (prevCountItemSelected > 0 && countItemSelected == 0)
            //        Leave(gametime);

            //    prevCountItemSelected = countItemSelected;
            //}

            //double elapsedTimeMenu = gametime.TotalGameTime.Subtract(LastStateChanged).TotalMilliseconds;

            //if (State == ComponentState.Opening && elapsedTimeMenu > 0)
            //    PercentVisibility = elapsedTimeMenu / MENU_ANIMATION_DURATION;

            //if (State == ComponentState.Closing)
            //    PercentVisibility = 1 - elapsedTimeMenu / MENU_ANIMATION_DURATION;

            //if (PercentVisibility > 1)
            //    PercentVisibility = 1;
            //if (PercentVisibility < 0)
            //    PercentVisibility = 0;

            //if (State == ComponentState.Opening || State == ComponentState.Closing || State == ComponentState.Opened)
            //    CreateVertex();

            //if (PercentVisibility == 1 && State == ComponentState.Opening)
            //    State = ComponentState.Opened;
            //if (PercentVisibility == 0 && State == ComponentState.Closing)
            //{
            //    State = ComponentState.Closed;

            //    if (!IsTurnMode)
            //        Visible = false;
            //}

            //if (Alive && Visible)
            //{
            //    MouseOver(gametime);
            //}

            base.Update(gametime);
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Rec, VisualStyle.BackColorDark);

            base.Draw(gameTime);
        }
    }
}
