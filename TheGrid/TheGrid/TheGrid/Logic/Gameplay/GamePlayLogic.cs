using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Common;
using Microsoft.Xna.Framework;
using TheGrid.Model.Menu;
using TheGrid.Model.Instrument;

namespace TheGrid.Logic.GamePlay
{
    public class GamePlayLogic
    {
        private Random rnd = new Random();
        private float musicianSpeed = 500f;
        public GameEngine GameEngine { get; set; }

        public GamePlayLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
            InitializeMap();
            InitializeChannel();
            InitializePlayers();
        }

        private void InitializeMap()
        {
            Context.Map = new Map(7, 30);
            Context.Map.CreateGrid();
        }

        private void InitializeChannel()
        {
            Context.Channels = new List<Channel>();

            Context.Channels.Add(new Channel("Empty", Color.White));
            Context.Channels.Add(new Channel("Drums", Color.Red));
            Context.Channels.Add(new Channel("Keys", Color.Blue));
            Context.Channels.Add(new Channel("Guitar", Color.Yellow));
            Context.Channels.Add(new Channel("Bass", Color.Purple));
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

        public void EvaluateMuscianGrid()
        {
            TimeSpan elapsedTime = new TimeSpan(0, 0, 0);

            foreach (Channel channel in Context.Channels)
            {
                if (channel.CellStart != null)
                {
                    channel.ListMusician = new List<Musician>();
                    Musician musician = new Musician();
                    musician.Instruments = new List<InstrumentBase>();
                    channel.ListMusician.Add(musician);

                    EvaluateMusicianPartition(musician, channel.CellStart, channel, elapsedTime);
                    /*
                    //foreach (Musician musician in channel.ListMusician)
                    {

                        Cell cell = channel.CellStart;

                        if (cell.Clip != null)
                        {
                            if(cell.Clip.Instrument != null)
                            {
                            cell.Clip.Instrument.StartTime = elapsedTime;
                            musician.Instruments.Add(cell.Clip.Instrument);
                            }

                            for (int i = 0; i < 6; i++)
                            {
                                bool divided = false;
                                if (cell.Clip.Directions[i])
                                {
                                    if (divided)
                                    {
                                        musician = new Musician();
                                        musician.Instruments = new List<InstrumentBase>();
                                        channel.ListMusician.Add(musician);
                                    }

                                    musician.NextCell = cell.Neighbourghs[i];
                                    divided = true;
                                }
                            }
                        }
                    }
                     * */
                }
            }
        }

        public void EvaluateMusicianPartition(Musician musician, Cell cell, Channel channel, TimeSpan elapsedTime)
        {
            musician.NextCell = null;
            List<Musician> listNewMusician = new List<Musician>();

            if (cell.Clip != null)
            {
                if (cell.Clip.Instrument != null)
                {
                    elapsedTime = elapsedTime.Add(new TimeSpan(0,0,0,0,(int)(musicianSpeed*channel.Speed)));
                    cell.Clip.Instrument.StartTime = elapsedTime;
                    musician.Instruments.Add(cell.Clip.Instrument);
                }

                for (int i = 0; i < 6; i++)
                {
                    bool divided = false;
                    if (cell.Clip.Directions[i])
                    {
                        if (divided)
                        {
                            musician = new Musician();
                            musician.Instruments = new List<InstrumentBase>();

                            channel.ListMusician.Add(musician);
                            listNewMusician.Add(musician);
                        }

                        //---
                        InstrumentSample isample = new InstrumentSample();
                        isample.StartTime = elapsedTime;
                        musician.Instruments.Add(isample);
                        musician.CurrentDirection = i;
                        //---
                        musician.NextCell = cell.Neighbourghs[i];
                        divided = true;
                    }
                }
            }

            musician.NextCell = cell.Neighbourghs[musician.CurrentDirection];

            if (musician.NextCell != null)
                EvaluateMusicianPartition(musician, musician.NextCell, channel, elapsedTime);

            foreach (Musician newMusician in listNewMusician)
            {
                newMusician.NextCell = cell.Neighbourghs[newMusician.CurrentDirection];

                if (newMusician.NextCell != null)
                    EvaluateMusicianPartition(newMusician, newMusician.NextCell, channel, elapsedTime);
            }
        }

        #region Menu
        public Menu CreateMenu(Cell cell)
        {
            //---
            Menu menuRoot = new Menu(cell, null, true);

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

            Menu menuChannel = new Menu(item.ParentMenu.ParentCell, item.ParentMenu, false);
            menuChannel.AngleDeltaRender = MathHelper.TwoPi / Context.Channels.Count / 4;
            menuChannel.AngleDeltaMouse = 0;// MathHelper.TwoPi / Context.Channels.Count / 4;

            for (int i = 0; i < Context.Channels.Count; i++)
            {
                Item itemChannel = new Item(menuChannel, Context.Channels[i].Name, i);
                itemChannel.Color = Context.Channels[i].Color;

                if (item.ParentMenu.ParentCell.Channel != null && item.ParentMenu.ParentCell.Channel == Context.Channels[i])
                    itemChannel.Checked = true;

                itemChannel.Selected += new Item.SelectedHandler(itemChannel_Selected);
                menuChannel.Items.Add(itemChannel);
            }

            Context.NextMenu = menuChannel;
        }

        void itemChannel_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.Channel = Context.Channels[item.Value];
            
            //TODO : à supprimer par la suite
            item.ParentMenu.ParentCell.Channel.CellStart = item.ParentMenu.ParentCell;

            Context.NextMenu = item.ParentMenu.ParentMenu;
        }
        #endregion

        #region Menu Instrument
        void itemMenuInstrument_Selected(Item item, GameTime gameTime)
        {
        }
        #endregion

        #region Menu Speed
        void itemMenuSpeed_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            //---
            Menu menuSpeed = new Menu(item.ParentMenu.ParentCell, item.ParentMenu, true);
            menuSpeed.AngleDeltaRender = -MathHelper.TwoPi / 36 * 3;

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
            Menu menuRepeater = new Menu(item.ParentMenu.ParentCell, item.ParentMenu, true);
            menuRepeater.AngleDeltaRender = MathHelper.TwoPi / 12;
            menuRepeater.AngleDeltaRenderIcon = MathHelper.TwoPi / 12;
            menuRepeater.AngleDeltaMouse = -MathHelper.TwoPi / 12;

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
            Menu menuDirection = new Menu(item.ParentMenu.ParentCell, item.ParentMenu, true);

            for (int i = 0; i < 6; i++)
            {
                Item itemDirection = new Item(menuDirection, "Direction" + (i + 1).ToString(), i);

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
