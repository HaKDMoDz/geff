using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework;
using TheGrid.Logic.Controller;
using TheGrid.Model.Effect;
using Microsoft.Xna.Framework.Input;
using TheGrid.Common;
using TheGrid.Model.UI.Menu;
using TheGrid.Model.Instrument;

namespace TheGrid.Model.UI.Effect
{
    public class EffectPanel : UIComponent
    {
        private ChannelEffect _channelEffect { get; set; }
        private Cell _cell { get; set; }
        private float _widthChannelChooser = 200f;

        public EffectPanel(UILogic uiLogic, TimeSpan creationTime, ChannelEffect channelEffect, Cell cell)
            : base(uiLogic, creationTime)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this._channelEffect = new ChannelEffect(channelEffect.Channel, channelEffect.Name);
            this._cell = cell;

            this.ListUIChildren = new List<UIComponent>();

            //--- TODO : clone des valeurs du channelEffect en entrée dans le channelEffect courant
            for (int i = 0; i < channelEffect.ListEffectProperty.Count; i++)
            {
                _channelEffect.ListEffectProperty[i].Value = channelEffect.ListEffectProperty[i].Value;
            }
            //---

            Vector2 sizeWindow = new Vector2(_widthChannelChooser + Math.Min(channelEffect.ListEffectProperty.Count, 3) * (EffectPropertyChanger.WIDTH + Ribbon.MARGE), (int)Math.Round((float)channelEffect.ListEffectProperty.Count / 3f, MidpointRounding.AwayFromZero) * EffectPropertyChanger.HEIGHT);

            Rec = new Rectangle((int)(Render.ScreenWidth / 2 - sizeWindow.X / 2), (int)(Render.ScreenHeight / 2 - sizeWindow.Y / 2), (int)sizeWindow.X, (int)(sizeWindow.Y));

            Vector2 vec = new Vector2(Rec.X + _widthChannelChooser + Ribbon.MARGE, Rec.Y);
            int nb = 0;

            foreach (EffectProperty effectProperty in _channelEffect.ListEffectProperty)
            {
                EffectPropertyChanger effectPropertyChanger = new EffectPropertyChanger(UI, GetNewTimeSpan(), effectProperty);
                effectPropertyChanger.Rec = new Rectangle((int)vec.X, (int)vec.Y, EffectPropertyChanger.WIDTH, EffectPropertyChanger.HEIGHT);
                effectPropertyChanger.Init();

                nb++;

                if (nb % 3 == 0)
                {
                    vec.X = Rec.X + _widthChannelChooser + Ribbon.MARGE;
                    vec.Y += EffectPropertyChanger.HEIGHT + Ribbon.MARGE;
                }
                else
                    vec.X += EffectPropertyChanger.WIDTH + Ribbon.MARGE;

                this.ListUIChildren.Add(effectPropertyChanger);
            }

            CreateChannelMenu();

            //--- Bouton Valider
            ClickableText txtOk = new ClickableText(UI, GetNewTimeSpan(), "FontText", "Ok", new Vector2(Rec.X + _widthChannelChooser * 0.3f, Rec.Y + Rec.Height * 0.8f), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
            txtOk.ClickText += new ClickableText.ClickTextHandler(txtOk_ClickText);
            ListUIChildren.Add(txtOk);

            ClickableText txtCancel = new ClickableText(UI, GetNewTimeSpan(), "FontText", "Cancel", new Vector2(Rec.X + _widthChannelChooser * 0.5f, Rec.Y + Rec.Height * 0.8f), VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
            txtCancel.ClickText += new ClickableText.ClickTextHandler(txtCancel_ClickText);
            ListUIChildren.Add(txtCancel);
            //---

            //--- Bouton Annuler
            //---

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        void txtCancel_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            this.Alive = false;
        }

        void txtOk_ClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            _cell.InitClip();
            _cell.Clip.Instrument = new InstrumentEffect(_channelEffect);

            UI.GameEngine.GamePlay.EvaluateMuscianGrid();

            this.Alive = false;
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        private void CreateChannelMenu()
        {
            ListUIChildren.RemoveAll(ui => ui is CircularMenu);

            CircularMenu menuChannel = new CircularMenu(UI, GetNewTimeSpan(), null, null, null, false);
            menuChannel.Location = new Vector2(Rec.X + _widthChannelChooser / 2, Rec.Y + Rec.Height / 2);

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
            menuChannel.State = MenuState.Opened;
            menuChannel.Effect = Render.effectUI;
            menuChannel.IsUI = true;

            ListUIChildren.Add(menuChannel);
        }

        void itemChannel_Selected(Item item, GameTime gameTime)
        {
            bool prevItemChecked = item.Checked;
            item.ParentMenu.Items.ForEach(i => i.Checked = false);
            item.Checked = !prevItemChecked;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Render.GraphicsDevice.Viewport.Bounds, VisualStyle.BackColorModalScreen);

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, VisualStyle.BackColorLight);

            base.Draw(gameTime);
        }
    }
}
