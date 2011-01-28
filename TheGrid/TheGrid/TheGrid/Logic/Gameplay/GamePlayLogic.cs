using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Common;
using Microsoft.Xna.Framework;
using TheGrid.Model.Menu;

namespace TheGrid.Logic.GamePlay
{
    public class GamePlayLogic
    {
        private Random rnd = new Random();

        public Map Map { get; set; }
        public GameEngine GameEngine { get; set; }

        public GamePlayLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
            InitializeMap();
            InitializePlayers();
        }

        private void InitializeMap()
        {
            Map = new Map(7, 30);
            Map.CreateGrid();
        }

        private void InitializePlayers()
        {
            Context.Players = new List<Player>();
            Context.Players.Add(new Player() { PlayerName = "Geff" });
            Context.CurrentPlayer = Context.Players[0];
        }

        public void Update(GameTime gameTime)
        {
            //--- Menu
            if (Context.CurrentMenu != null)
            {
                Context.CurrentMenu.Update(GameEngine, gameTime);

                if (Context.CurrentMenu.State == MenuState.Closed)
                {
                    if (Context.NextMenu != null && Context.CurrentMenu != Context.NextMenu)
                    {
                        Context.CurrentMenu = Context.NextMenu;
                        //Context.NextMenu = null;
                        Context.CurrentMenu.Open(gameTime);
                    }
                    else
                    {
                        Context.CurrentMenu = null;
                    }
                }
            }
            //---
        }

        #region Menu
        public Menu CreateMenu(Cell cell)
        {
            //---
            Menu menuRoot = new Menu(cell, null);

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

            Item itemInstrument = new Item(menuRoot, "Instrument");
            itemInstrument.Selected += new Item.SelectedHandler(itemInstrument_Selected);
            menuRoot.Items.Add(itemInstrument);

            Item itemChannel = new Item(menuRoot, "Channel");
            itemChannel.Selected += new Item.SelectedHandler(itemChannel_Selected);
            menuRoot.Items.Add(itemChannel);
            //---

            return menuRoot;
        }

        void itemChannel_Selected(Item item, GameTime gameTime)
        {
        }

        void itemInstrument_Selected(Item item, GameTime gameTime)
        {
        }

        #region Menu Speed
        void itemMenuSpeed_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            Menu menuSpeed = new Menu(item.ParentMenu.ParentCell, item.ParentMenu);

            for (int i = 0; i < 9; i++)
            {
                Item itemSpeed;
                
                if(i==0)
                    itemSpeed = new Item(menuSpeed, "Reset", i);
                else if (i < 5)
                    itemSpeed = new Item(menuSpeed, "SpeedH" + i.ToString(), i+4);
                else
                    itemSpeed = new Item(menuSpeed, "SpeedL" + (9-i).ToString(), i-9);

                itemSpeed.Selected += new Item.SelectedHandler(itemSpeed_Selected);
                menuSpeed.Items.Add(itemSpeed);
            }
            //---

            Context.NextMenu = menuSpeed;
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

            Context.NextMenu = item.ParentMenu.ParentMenu;
        }
        #endregion

        #region Menu Repeater
        void itemMenuRepeater_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            Menu menuRepeater = new Menu(item.ParentMenu.ParentCell, item.ParentMenu);

            for (int i = 0; i < 6; i++)
            {
                Item itemRepeater = new Item(menuRepeater, "Repeater" + (i + 1).ToString(), i);

                if (item.ParentMenu.ParentCell.Clip != null && item.ParentMenu.ParentCell.Clip.Repeater.HasValue && i <= item.ParentMenu.ParentCell.Clip.Repeater.Value)
                    itemRepeater.Checked = true;

                itemRepeater.Selected += new Item.SelectedHandler(itemRepeater_Selected);
                menuRepeater.Items.Add(itemRepeater);
            }
            //---

            Context.NextMenu = menuRepeater;
        }

        void itemRepeater_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.InitClip();

            for (int i = 0; i < 6; i++)
            {
                item.ParentMenu.Items[i].Checked = i <= item.Value;
            }

            item.ParentMenu.ParentCell.Clip.Repeater = item.Value;

            Context.NextMenu = item.ParentMenu.ParentMenu;
        }
        #endregion

        #region Menu Direction
        void itemMenuDirection_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            Menu menuDirection = new Menu(item.ParentMenu.ParentCell, item.ParentMenu);

            for (int i = 0; i < 6; i++)
            {
                Item itemDirection = new Item(menuDirection, "Direction" + (i+1).ToString(), i);

                if (item.ParentMenu.ParentCell.Clip != null && item.ParentMenu.ParentCell.Clip.Directions[i])
                    itemDirection.Checked = true;

                itemDirection.Selected += new Item.SelectedHandler(itemDirection_Selected);
                menuDirection.Items.Add(itemDirection);
            }
            //---

            Context.NextMenu = menuDirection;
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

            item.ParentMenu.ParentCell.Clip = null;
            item.ParentMenu.ParentCell.Channel = null;
        }
        #endregion
    }
}
