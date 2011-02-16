using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Model.UI.Menu;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Model.Instrument;
using TheGrid.Model.UI;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Model.UI.Effect;
using TheGrid.Model.Effect;

namespace TheGrid.Logic.UI
{
    public class UILogic
    {
        public GameEngine GameEngine { get; set; }

        public List<UIComponent> ListUIComponent { get; set; }

        public UILogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
            this.ListUIComponent = new List<UIComponent>();
        }

        public void Update(GameTime gameTime)
        {
            ListUIComponent.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));

            for (int i = 0; i < ListUIComponent.Count; i++)
            {
                if (ListUIComponent[i].Alive)
                {
                    ListUIComponent[i].Update(gameTime);
                }

                ListUIComponent[i].UpdateUIDependency(gameTime);
            }

            ListUIComponent.RemoveAll(ui => !ui.Alive);
        }

        public void Draw(GameTime gameTime)
        {
            GameEngine.Render.SpriteBatch.Begin();

            ListUIComponent.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));

            foreach (UIComponent uiComponent in ListUIComponent)
            {
                if (uiComponent.Visible)
                {
                    uiComponent.Draw(gameTime);
                }
            }

            GameEngine.Render.SpriteBatch.End();
        }

        public bool IsUIModalActive()
        {
            bool isUIModalActive = false;

            foreach (UIComponent ui in ListUIComponent)
            {
                if (ui.Alive && ui.Visible)
                {
                    isUIModalActive |= ui.IsUIModalActive();
                }
            }

            return isUIModalActive;
        }

        public bool IsMouseHandled()
        {
            bool isMouseHandled = false;

            if (ListUIComponent != null)
            {
                foreach (UIComponent ui in ListUIComponent)
                {
                    if (ui.Alive && ui.Visible)
                    {
                        isMouseHandled |= ui.IsMouseHandled();
                    }
                }
            }

            return isMouseHandled;
        }

        #region Menu
        private void NextMenu(CircularMenu previousMenu, CircularMenu nextMenu)
        {
            previousMenu.UIDependency = null;
            nextMenu.UIDependency = previousMenu;
            nextMenu.Alive = true;
            nextMenu.Visible = false;
            this.ListUIComponent.Add(nextMenu);
        }

        public CircularMenu CreateMenu(Cell cell, TimeSpan creationTime)
        {
            //---
            CircularMenu menuRoot = new CircularMenu(this, creationTime, cell, null, null, true);

            Item itemReset = new Item(menuRoot, "Reset");
            itemReset.Selected += new Item.SelectedHandler(itemReset_Selected);
            menuRoot.Items.Add(itemReset);

            Item itemMenuDirection = new Item(menuRoot, "Direction");
            itemMenuDirection.Selected += new Item.SelectedHandler(itemMenuDirection_Selected);
            menuRoot.Items.Add(itemMenuDirection);

            Item itemMenuRepeater = new Item(menuRoot, "Repeater");
            itemMenuRepeater.Selected += new Item.SelectedHandler(itemMenuRepeater_Selected);
            menuRoot.Items.Add(itemMenuRepeater);

            Item itemMenuSpeed = new Item(menuRoot, "Speed");
            itemMenuSpeed.Selected += new Item.SelectedHandler(itemMenuSpeed_Selected);
            menuRoot.Items.Add(itemMenuSpeed);

            Item itemMenuInstrument = new Item(menuRoot, "Instrument");
            itemMenuInstrument.Selected += new Item.SelectedHandler(itemMenuInstrument_Selected);
            menuRoot.Items.Add(itemMenuInstrument);

            Item itemMenuChannel = new Item(menuRoot, "Channel");
            itemMenuChannel.Selected += new Item.SelectedHandler(itemMenuChannel_Selected);
            menuRoot.Items.Add(itemMenuChannel);
            //---

            return menuRoot;
        }

        #region Menu Channel
        void itemMenuChannel_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            CircularMenu menuChannel = new CircularMenu(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell, item.ParentMenu, item, false);

            for (int i = 0; i < Context.Map.Channels.Count; i++)
            {
                Item itemChannel = new Item(menuChannel, Context.Map.Channels[i].Name, i);
                itemChannel.Color = Context.Map.Channels[i].Color;

                if (item.ParentMenu.ParentCell.Channel != null && item.ParentMenu.ParentCell.Channel == Context.Map.Channels[i])
                    itemChannel.Checked = true;

                itemChannel.Selected += new Item.SelectedHandler(itemChannel_Selected);
                menuChannel.Items.Add(itemChannel);
            }

            menuChannel.nbVertex = menuChannel.Items.Count * 4;

            NextMenu(item.ParentMenu, menuChannel);
        }

        void itemChannel_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.Channel = Context.Map.Channels[item.Value];

            if (item.ParentMenu.ParentItem.Name == "Sample")
                itemSample_Selected(item, gameTime);
            else if (item.ParentMenu.ParentItem.Name == "MusicianStart")
                itemMusicianStart_Selected(item, gameTime);
            else if (item.ParentMenu.ParentItem.Name == "Effect")
                itemEffect_Selected(item, gameTime);
            else
                NextMenu(item.ParentMenu, item.ParentMenu.ParentMenu);
        }
        #endregion

        #region Menu Instrument
        void itemMenuInstrument_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            CircularMenu menuInstrument = new CircularMenu(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell, item.ParentMenu, item, true);

            Item itemReset = new Item(menuInstrument, "Reset");
            itemReset.Selected += new Item.SelectedHandler(itemInstrumentReset_Selected);
            menuInstrument.Items.Add(itemReset);

            Item itemSample = new Item(menuInstrument, "Sample");
            itemSample.Selected += new Item.SelectedHandler(itemSample_Selected);
            menuInstrument.Items.Add(itemSample);

            Item itemNote = new Item(menuInstrument, "Note");
            itemNote.Selected += new Item.SelectedHandler(itemNote_Selected);
            menuInstrument.Items.Add(itemNote);

            Item itemEffect = new Item(menuInstrument, "Effect");
            itemEffect.Selected += new Item.SelectedHandler(itemEffect_Selected);
            menuInstrument.Items.Add(itemEffect);

            Item itemMusicianStart = new Item(menuInstrument, "MusicianStart");
            itemMusicianStart.Selected += new Item.SelectedHandler(itemMusicianStart_Selected);
            menuInstrument.Items.Add(itemMusicianStart);

            Item itemMusicianStop = new Item(menuInstrument, "MusicianStop");
            itemMusicianStop.Selected += new Item.SelectedHandler(itemMusicianStop_Selected);
            menuInstrument.Items.Add(itemMusicianStop);

            NextMenu(item.ParentMenu, menuInstrument);
        }

        void itemInstrumentReset_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.InitClip();
            item.ParentMenu.ParentCell.Clip.Instrument = null;

            NextMenu(item.ParentMenu, item.ParentMenu.ParentMenu);
        }

        void itemSample_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            if (item.ParentMenu.ParentCell.Channel == null || item.ParentMenu.ParentCell.Channel.Name == "Empty")
            {
                itemMenuChannel_Selected(item, gameTime);
            }
            else
            {
                item.ParentMenu.Alive = false;

                ListSample listSample = new ListSample(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell);
                this.ListUIComponent.Add(listSample);
            }
        }

        void itemNote_Selected(Item item, GameTime gameTime)
        {
        }

        void itemEffect_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            if (item.ParentMenu.ParentCell.Channel == null || item.ParentMenu.ParentCell.Channel.Name == "Empty")
            {
                itemMenuChannel_Selected(item, gameTime);
            }
            else
            {
                CircularMenu menuChannelEffect = new CircularMenu(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell, item.ParentMenu, item, false, true);

                //--- Création des items liés aux effets du channel
                for (int i = 0; i < item.ParentMenu.ParentCell.Channel.ListEffect.Count; i++)
                {
                    Item itemChannelEffect = new Item(menuChannelEffect, item.ParentMenu.ParentCell.Channel.ListEffect[i].Name);
                    itemChannelEffect.Value = i;
                    itemChannelEffect.Selected += new Item.SelectedHandler(itemChannelEffect_Selected);
                    menuChannelEffect.Items.Add(itemChannelEffect);
                }
                //---

                NextMenu(item.ParentMenu, menuChannelEffect);
            }
        }

        void itemChannelEffect_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);
            item.ParentMenu.Alive = false;

            EffectPanel effectPanel = new EffectPanel(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell.Channel.ListEffect[item.Value], item.ParentMenu.ParentCell);
            this.ListUIComponent.Add(effectPanel);
        }

        void itemMusicianStart_Selected(Item item, GameTime gameTime)
        {
            if (item.ParentMenu.ParentCell.Channel == null || item.ParentMenu.ParentCell.Channel.Name == "Empty")
            {
                itemMenuChannel_Selected(item, gameTime);
            }
            else
            {
                if (item.ParentMenu.ParentCell.Channel.CellStart != null)
                {
                    item.ParentMenu.ParentCell.Channel.CellStart.InitClip();
                    item.ParentMenu.ParentCell.Channel.CellStart.Clip.Instrument = null;
                }

                item.ParentMenu.ParentCell.Channel.CellStart = item.ParentMenu.ParentCell;
                item.ParentMenu.ParentCell.Channel.CellStart.InitClip();
                item.ParentMenu.ParentCell.Channel.CellStart.Clip.Instrument = new InstrumentStart();

                item.ParentMenu.Close(gameTime);

                NextMenu(item.ParentMenu, item.ParentMenu.ParentMenu);
            }
        }

        void itemMusicianStop_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.InitClip();
            item.ParentMenu.ParentCell.Clip.Instrument = new InstrumentStop();


            NextMenu(item.ParentMenu, item.ParentMenu.ParentMenu);
        }
        #endregion

        #region Menu Speed
        void itemMenuSpeed_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            CircularMenu menuSpeed = new CircularMenu(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell, item.ParentMenu, item, true);

            for (int i = 0; i < 9; i++)
            {
                Item itemSpeed;

                if (i == 0)
                    itemSpeed = new Item(menuSpeed, "Reset", i);
                else if (i < 5)
                    itemSpeed = new Item(menuSpeed, "SpeedH" + i.ToString(), i);
                else
                    itemSpeed = new Item(menuSpeed, "SpeedL" + (9 - i).ToString(), i - 9);

                itemSpeed.Selected += new Item.SelectedHandler(itemSpeed_Selected);
                menuSpeed.Items.Add(itemSpeed);
            }
            //---

            NextMenu(item.ParentMenu, menuSpeed);
        }

        void itemSpeed_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.InitClip();

            bool isChecked = !item.Checked;

            for (int i = 0; i < 9; i++)
            {
                item.ParentMenu.Items[i].Checked = false;
            }

            item.Checked = isChecked;

            item.ParentMenu.ParentCell.Clip.Speed = item.Value;

            NextMenu(item.ParentMenu, item.ParentMenu.ParentMenu);
        }
        #endregion

        #region Menu Repeater
        void itemMenuRepeater_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            CircularMenu menuRepeater = new CircularMenu(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell, item.ParentMenu, item, true);
            menuRepeater.AngleDelta = -MathHelper.TwoPi / 12;

            for (int i = 0; i < 6; i++)
            {
                Item itemRepeater = new Item(menuRepeater, "Repeater" + (i + 1).ToString(), i);

                if (item.ParentMenu.ParentCell.Clip != null && item.ParentMenu.ParentCell.Clip.Repeater.HasValue && i <= item.ParentMenu.ParentCell.Clip.Repeater.Value)
                    itemRepeater.Checked = true;

                itemRepeater.Selected += new Item.SelectedHandler(itemRepeater_Selected);
                menuRepeater.Items.Add(itemRepeater);
            }
            //---

            NextMenu(item.ParentMenu, menuRepeater);
        }

        void itemRepeater_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.InitClip();
            int newValue = item.Value;

            if (item.ParentMenu.ParentCell.Clip.Repeater == item.Value)
                newValue = 0;

            for (int i = 0; i < 6; i++)
            {
                item.ParentMenu.Items[i].Checked = i <= newValue;
            }

            if (newValue == 0)
                item.ParentMenu.ParentCell.Clip.Repeater = null;
            else
                item.ParentMenu.ParentCell.Clip.Repeater = newValue;

            NextMenu(item.ParentMenu, item.ParentMenu.ParentMenu);
        }
        #endregion

        #region Menu Direction
        void itemMenuDirection_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            CircularMenu menuDirection = new CircularMenu(this, gameTime.TotalGameTime, item.ParentMenu.ParentCell, item.ParentMenu, item, true);

            for (int i = 0; i < 6; i++)
            {
                Item itemDirection = new Item(menuDirection, "Direction" + (i + 1).ToString(), i);

                if (item.ParentMenu.ParentCell.Clip != null && item.ParentMenu.ParentCell.Clip.Directions[i])
                    itemDirection.Checked = true;

                itemDirection.Selected += new Item.SelectedHandler(itemDirection_Selected);
                menuDirection.Items.Add(itemDirection);
            }
            //---

            NextMenu(item.ParentMenu, menuDirection);
        }

        void itemDirection_Selected(Item item, GameTime gameTime)
        {
            item.Checked = !item.Checked;

            item.ParentMenu.ParentCell.InitClip();
            item.ParentMenu.ParentCell.Clip.Directions[item.Value] = item.Checked;
        }
        #endregion

        void itemReset_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            if (item.ParentMenu.ParentCell.Clip != null &&
                item.ParentMenu.ParentCell.Clip.Instrument != null &&
                item.ParentMenu.ParentCell.Channel != null &&
                item.ParentMenu.ParentCell == item.ParentMenu.ParentCell.Channel.CellStart)
            {
                item.ParentMenu.ParentCell.Clip.Instrument = null;
                item.ParentMenu.ParentCell.Channel.CellStart = null;
            }

            item.ParentMenu.ParentCell.Clip = null;
            item.ParentMenu.ParentCell.Channel = null;
        }
        #endregion
    }
}
