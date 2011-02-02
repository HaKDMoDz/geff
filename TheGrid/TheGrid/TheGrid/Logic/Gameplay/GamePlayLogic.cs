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
            InitializeChannel();
            InitializePlayers();
            InitializeMap();
        }

        private void InitializeMap()
        {
            Context.Map = new Map(10, 10);
            //Context.Map = new Map(5, 5);
            Context.Map.CreateGrid();

            //---
            Cell cell1 = Context.Map.Cells[15];
            cell1.InitClip();
            cell1.Clip.Directions[3] = true;
            cell1.Clip.Directions[2] = true;
            cell1.Channel = Context.Channels[2];
            Context.Channels[2].CellStart = cell1;

            Cell cell0 = cell1.GetDirection(3, 3);
            cell0.InitClip();
            cell0.Clip.Directions[2] = true;

            Cell cell2 = cell0.GetDirection(2, 3);
            cell2.InitClip();
            cell2.Clip.Directions[3] = true;

            Cell cell3 = cell2.GetDirection(3, 4);
            cell3.InitClip();
            cell3.Clip.Directions[1] = true;
            cell3.Clip.Directions[5] = true;

            Cell cell4 = cell1.GetDirection(2, 6);
            cell4.InitClip();
            cell4.Clip.Directions[4] = true;
            //---
        }



        private void InitializeChannel()
        {
            Context.Channels = new List<Channel>();

            Context.Channels.Add(new Channel("Empty", Color.White));
            Context.Channels.Add(new Channel("Drums", Color.Red));
            Context.Channels.Add(new Channel("Keys", Color.Blue));
            //Context.Channels.Add(new Channel("Guitar", Color.Yellow));
            //Context.Channels.Add(new Channel("Bass", Color.Purple));
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

            if (Context.IsPlaying)
                UpdateMusicians(gameTime);
        }

        public void UpdateMusicians(GameTime gameTime)
        {
            //---> Met à jour le temps écoulé
            Context.Time = Context.Time.Add(gameTime.ElapsedGameTime);

            foreach (Channel channel in Context.Channels)
            {
                foreach (Musician musician in channel.ListMusician)
                {
                    if (musician.Partition.Count > 0)
                    {
                        //---> Première cellule
                        if (musician.CurrentCell == null && musician.PartitionTime[0] <= Context.Time)
                        {
                            musician.CurrentIndex = 0;
                            musician.CurrentCell = musician.Partition[musician.CurrentIndex];
                        }


                        if (musician.CurrentCell != null && musician.Partition.Count > musician.CurrentIndex + 1)
                        {
                            musician.NextCell = musician.Partition[musician.CurrentIndex + 1];

                            TimeSpan durationToNextCell = musician.PartitionTime[musician.CurrentIndex + 1].Subtract(musician.PartitionTime[musician.CurrentIndex]);

                            double percent =
                                (Context.Time.Subtract(musician.PartitionTime[musician.CurrentIndex])).TotalMilliseconds /
                                (musician.PartitionTime[musician.CurrentIndex + 1].Subtract(musician.PartitionTime[musician.CurrentIndex])).TotalMilliseconds;


                            musician.Position = new Vector3(musician.CurrentCell.Location + (musician.NextCell.Location - musician.CurrentCell.Location) * (float)percent, 0f);


                            //---> Passe à la cellule suivante
                            if (Context.Time >= musician.PartitionTime[musician.CurrentIndex + 1])
                            {
                                musician.CurrentIndex++;
                                musician.CurrentCell = musician.Partition[musician.CurrentIndex];
                                //TODO : SoundLogic
                            }
                        }
                    }
                }
            }
        }

        public void EvaluateMuscianGrid()
        {
            TimeSpan elapsedTime = new TimeSpan(0, 0, 0);

            foreach (Channel channel in Context.Channels)
            {
                channel.ListMusician = new List<Musician>();
                channel.ListSpeed = new List<float>();
                channel.ListSpeedTime = new List<TimeSpan>();

                channel.Speed = 1f;
                channel.ListSpeed.Add(1f);
                channel.ListSpeedTime.Add(new TimeSpan());

                if (channel.CellStart != null)
                {
                    Musician musician = new Musician(channel);
                    channel.ListMusician.Add(musician);

                    musician.Partition.Add(channel.CellStart);
                    musician.PartitionTime.Add(new TimeSpan());

                    EvaluateMusicianPartition(musician, channel.CellStart, channel, elapsedTime);
                }
            }
        }

        public void EvaluateMusicianPartition(Musician musician, Cell cell, Channel channel, TimeSpan elapsedTime)
        {
            musician.NextCell = null;
            List<Musician> listNewMusician = new List<Musician>();
            
            if (elapsedTime.TotalMilliseconds > 1000 * 60 * 1)
                return;

            //if (channel.ListMusician.Count < 4)
            {
                if ((cell.Channel == null || cell.Channel == channel) && cell.Clip != null)
                {
                    if (cell.Clip.Speed.HasValue)
                    {
                        float speed = channel.GetSpeedFromTime(elapsedTime);

                        speed *= (0.22f * (float)(cell.Clip.Speed.Value)) + 1f;
                        //speed = 0.25f * (float)(cell.Clip.Speed.Value) + 1f;

                        if (speed < 1f / 16f)
                            speed = 1f / 16f;
                        else if (speed > 4f)
                            speed = 4f;

                        channel.ListSpeed.Add(speed);
                        channel.ListSpeedTime.Add(elapsedTime);
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        if (cell.Clip.Directions[i])
                        {
                            if (listNewMusician.Count > 0 && channel.ListMusician.Count < 4)
                            {
                                musician = new Musician(channel);
                                musician.CurrentDirection = i;
                                musician.Partition.Add(cell);
                                musician.PartitionTime.Add(elapsedTime);

                                channel.ListMusician.Add(musician);
                                listNewMusician.Add(musician);
                            }

                            if (listNewMusician.Count == 0)
                            {
                                musician.CurrentDirection = i;
                                listNewMusician.Add(musician);
                            }
                        }
                    }
                }
            }

            if (!listNewMusician.Contains(musician))
                listNewMusician.Add(musician);

            foreach (Musician newMusician in listNewMusician)
            {
                newMusician.NextCell = cell.Neighbourghs[newMusician.CurrentDirection];

                TimeSpan newElapsedTime = new TimeSpan(elapsedTime.Ticks);
                
                newElapsedTime = elapsedTime.Add(new TimeSpan(0, 0, 0, 0, (int)(musicianSpeed / channel.GetSpeedFromTime(newElapsedTime))));

                if (newMusician.NextCell != null)
                {
                    newMusician.Partition.Add(newMusician.NextCell);
                    newMusician.PartitionTime.Add(newElapsedTime);

                    EvaluateMusicianPartition(newMusician, newMusician.NextCell, channel, newElapsedTime);
                }
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
            menuChannel.AngleDeltaMouse = 0;

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
            foreach (Channel channel in Context.Channels)
            {
                if (channel.CellStart == item.ParentMenu.ParentCell)
                    channel.CellStart = null;
            }

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
