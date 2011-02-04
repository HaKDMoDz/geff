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

            Context.SpeedFactor = 1f;
            Context.PartitionDuration = new TimeSpan(0, 1, 0);

            InitializeChannel();
            InitializePlayers();
            InitializeMap();
        }

        private void InitializeMap()
        {
            Context.Map = new Map(15, 15);
            Context.Map.CreateGrid();

            ////---
            //Cell cell1 = Context.Map.Cells[15];
            //cell1.InitClip();
            //cell1.Clip.Directions[3] = true;
            //cell1.Clip.Directions[2] = true;
            //cell1.Clip.Instrument = new InstrumentStart();
            //cell1.Channel = Context.Channels[2];
            //Context.Channels[2].CellStart = cell1;

            //Cell cell0 = cell1.GetDirection(3, 3);
            //cell0.InitClip();
            //cell0.Clip.Directions[2] = true;

            //Cell cell2 = cell0.GetDirection(2, 3);
            //cell2.InitClip();
            //cell2.Clip.Directions[3] = true;

            //Cell cell3 = cell2.GetDirection(3, 4);
            //cell3.InitClip();
            //cell3.Clip.Directions[1] = true;
            //cell3.Clip.Directions[5] = true;

            //Cell cell4 = cell1.GetDirection(2, 6);
            //cell4.InitClip();
            //cell4.Clip.Directions[4] = true;
            ////---

            ////---
            //Cell cell1 = Context.Map.Cells[15];
            //cell1.InitClip();
            //cell1.Clip.Directions[3] = true;
            //cell1.Clip.Instrument = new InstrumentStart();
            //cell1.Channel = Context.Channels[2];
            //Context.Channels[2].CellStart = cell1;

            //Cell cell0 = cell1.GetDirection(3, 3);
            //cell0.InitClip();
            //cell0.Clip.Directions[2] = true;
            //cell0.Clip.Directions[3] = true;

            //Cell cell2 = cell0.GetDirection(2, 3);
            //cell2.InitClip();
            //cell2.Clip.Directions[0] = true;

            //Cell cell3 = cell2.GetDirection(0, 3);
            //cell3.InitClip();
            //cell3.Clip.Directions[4] = true;

            //Cell cell4 = cell0.GetDirection(3, 4);
            //cell4.InitClip();
            //cell4.Clip.Instrument = new InstrumentStop();
            ////---


            ////---
            //Cell cell1 = Context.Map.Cells[15];
            //cell1.InitClip();
            //cell1.Clip.Directions[3] = true;
            //cell1.Clip.Instrument = new InstrumentStart();
            //cell1.Channel = Context.Channels[2];
            //Context.Channels[2].CellStart = cell1;

            //Cell cell0 = cell1.GetDirection(3, 3);
            //cell0.InitClip();
            //cell0.Clip.Directions[2] = true;

            //Cell cell2 = cell0.GetDirection(2, 3);
            //cell2.InitClip();
            //cell2.Clip.Directions[0] = true;
            //cell2.Clip.Directions[1] = true;
            //cell2.Clip.Directions[2] = true;
            //cell2.Clip.Directions[3] = true;
            //cell2.Clip.Directions[4] = true;

            //Cell cell3 = cell2.GetDirection(0, 3);
            //cell3.InitClip();
            //cell3.Clip.Directions[4] = true;

            //Cell cell4 = cell2.GetDirection(1, 2);
            //cell4.InitClip();
            //cell4.Clip.Instrument = new InstrumentStop();

            //Cell cell5 = cell2.GetDirection(2, 2);
            //cell5.InitClip();
            //cell5.Clip.Instrument = new InstrumentStop();

            //Cell cell6 = cell2.GetDirection(3, 2);
            //cell6.InitClip();
            //cell6.Clip.Instrument = new InstrumentStop();

            //Cell cell7 = cell2.GetDirection(4, 2);
            //cell7.InitClip();
            //cell7.Clip.Instrument = new InstrumentStop();
            ////---

            ////---
            //Cell cell1 = Context.Map.Cells[15];
            //cell1.InitClip();
            //cell1.Clip.Directions[3] = true;
            //cell1.Clip.Instrument = new InstrumentStart();
            //cell1.Channel = Context.Channels[2];
            //Context.Channels[2].CellStart = cell1;

            //Cell cell0 = cell1.GetDirection(3, 3);
            //cell0.InitClip();
            //cell0.Clip.Directions[2] = true;

            //Cell cell2 = cell0.GetDirection(2, 3);
            //cell2.InitClip();
            //cell2.Clip.Directions[0] = true;
            //cell2.Clip.Directions[1] = true;
            //cell2.Clip.Directions[2] = true;
            //cell2.Clip.Directions[3] = true;
            //cell2.Clip.Directions[4] = true;

            //Cell cell3 = cell2.GetDirection(0, 3);
            //cell3.InitClip();
            //cell3.Clip.Directions[4] = true;

            //Cell cell4 = cell2.GetDirection(1, 2);
            //cell4.InitClip();
            //cell4.Clip.Instrument = new InstrumentStop();

            //Cell cell5 = cell2.GetDirection(2, 2);
            //cell5.InitClip();
            //cell5.Clip.Instrument = new InstrumentStop();

            //Cell cell6 = cell2.GetDirection(3, 2);
            //cell6.InitClip();
            //cell6.Clip.Instrument = new InstrumentStop();

            //Cell cell7 = cell2.GetDirection(4, 2);
            //cell7.InitClip();
            //cell7.Clip.Instrument = new InstrumentStop();
            ////---



            //---
            Cell cell1 = Context.Map.Cells[15];
            cell1.InitClip();
            cell1.Clip.Directions[2] = true;
            cell1.Clip.Directions[3] = true;
            cell1.Clip.Instrument = new InstrumentStart();
            cell1.Channel = Context.Channels[2];
            Context.Channels[2].CellStart = cell1;

            Cell cell0 = cell1.GetDirection(3, 3);
            cell0.InitClip();
            cell0.Clip.Directions[2] = true;

            Cell cell01 = cell1.GetDirection(2, 3);
            cell01.InitClip();
            cell01.Clip.Directions[3] = true;

            Cell cell2 = cell0.GetDirection(2, 3);
            cell2.InitClip();
            cell2.Clip.Directions[1] = true;

            Cell cell3 = cell2.GetDirection(1, 1);
            cell3.InitClip();
            cell3.Clip.Directions[1] = true;

            Cell cell4 = cell2.GetDirection(1, 3);
            cell4.InitClip();
            cell4.Clip.Directions[3] = true;
            cell4.Clip.Directions[5] = true;

            Cell cell5 = cell4.GetDirection(3, 3);
            cell5.InitClip();
            cell5.Clip.Directions[4] = true;
            cell5.Clip.Directions[5] = true;

            Cell cell6 = cell5.GetDirection(4, 3);
            cell6.InitClip();
            cell6.Clip.Directions[5] = true;

            Cell cell7 = cell6.GetDirection(5, 3);
            cell7.InitClip();
            cell7.Clip.Directions[0] = true;
            cell7.Clip.Directions[1] = true;
            //---

            EvaluateMuscianGrid();
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

            GameEngine.Window.Title = Context.Time.ToString();// +" - " + Context.Channels[2].CountMusicianPlaying.ToString();
        }

        public void UpdateMusicians(GameTime gameTime)
        {
            //---> Met à jour le temps écoulé
            TimeSpan time = new TimeSpan((long)((float)gameTime.ElapsedGameTime.Ticks * Context.SpeedFactor));
            Context.Time = Context.Time.Add(time);

            foreach (Channel channel in Context.Channels)
            {
                foreach (Musician musician in channel.ListMusician)
                {
                    if (musician.Partition.Count > 0)
                    {
                        //---> Première cellule
                        if (musician.CurrentCell == null && musician.Partition[0].Time <= Context.Time)
                        {
                            musician.CurrentIndex = 0;
                            musician.CurrentCell = musician.Partition[musician.CurrentIndex].Value;
                            musician.IsPlaying = true;
                        }

                        if (musician.CurrentCell != null && musician.Partition.Count > musician.CurrentIndex + 1)
                        {
                            musician.NextCell = musician.Partition[musician.CurrentIndex + 1].Value;

                            TimeSpan durationToNextCell = musician.Partition[musician.CurrentIndex + 1].Time.Subtract(musician.Partition[musician.CurrentIndex].Time);

                            double percent =
                                (Context.Time.Subtract(musician.Partition[musician.CurrentIndex].Time)).TotalMilliseconds /
                                (musician.Partition[musician.CurrentIndex + 1].Time.Subtract(musician.Partition[musician.CurrentIndex].Time)).TotalMilliseconds;

                            musician.Position = new Vector3(musician.CurrentCell.Location + (musician.NextCell.Location - musician.CurrentCell.Location) * (float)percent, 0f);

                            //---> Passe à la cellule suivante
                            if (Context.Time >= musician.Partition[musician.CurrentIndex + 1].Time)
                            {
                                musician.CurrentIndex++;
                                musician.CurrentCell = musician.Partition[musician.CurrentIndex].Value;

                                //TODO : SoundLogic
                                if (musician.CurrentCell.Clip != null && (musician.CurrentCell.Channel == null || musician.CurrentCell.Channel == musician.Channel) && musician.CurrentCell.Clip.Instrument is InstrumentStop)
                                {
                                    musician.IsPlaying = false;
                                }
                                else if (musician.CurrentCell.Clip != null)
                                {
                                    musician.IsPlaying = true;
                                }
                            }
                        }
                    }
                }
            }
        }


        public void EvaluateMuscianGrid()
        {
            //--- Initialisation
            foreach (Channel channel in Context.Channels)
            {
                channel.ListMusician = new List<Musician>();
                channel.ElapsedTime = new TimeSpan();
                channel.ListSpeed = new List<TimeValue<float>>();
                channel.ListSpeed.Add(new TimeValue<float>(new TimeSpan(), 1f));

                if (channel.CellStart != null)
                {
                    Musician musician = channel.GetMusicianNotPlaying();
                    musician.IsPlaying = true;
                    musician.NextCell = channel.CellStart;
                }
            }
            //---

            int channelCalculationInProgress = Context.Channels.Count;

            while (channelCalculationInProgress > 0)
            {
                channelCalculationInProgress = 0;

                foreach (Channel channel in Context.Channels)
                {
                    if (channel.ElapsedTime < Context.PartitionDuration)
                    {
                        List<Musician> newMusicians = new List<Musician>();

                        foreach (Musician musician in channel.ListMusician)
                        {
                            if (musician.NextCell != null)
                            {
                                Cell cell = musician.NextCell;

                                if (cell != null && (cell.Channel == null || cell.Channel == channel) && cell.Clip != null)
                                {
                                    bool ignoreCell = false;

                                    //--- Repeater
                                    if (cell.Clip.Repeater.HasValue)
                                    {
                                        TimeValue<Cell> part = musician.Partition.LastOrDefault(p => p.Value.Clip != null && p.Value.Clip.Instrument != null && p.Value.Clip.Instrument is InstrumentStop);
                                        TimeSpan timePart = TimeSpan.Zero;

                                        if (part != null)
                                            timePart = part.Time;

                                        ignoreCell = musician.Partition.Count(p => p.Time > timePart && p.Value == cell) >= (cell.Clip.Repeater.Value + 1);
                                    }
                                    //---

                                    if (!ignoreCell)
                                    {
                                        //--- Speed
                                        if (cell.Clip.Speed.HasValue)
                                        {
                                            float speed = channel.GetSpeedFromTime(channel.ElapsedTime);

                                            speed *= (0.22f * (float)(cell.Clip.Speed.Value)) + 1f;

                                            if (speed < 1f / 16f)
                                                speed = 1f / 16f;
                                            else if (speed > 4f)
                                                speed = 4f;

                                            channel.ListSpeed.Add(new TimeValue<float>(channel.ElapsedTime, speed));
                                        }
                                        //---

                                        //--- Instrument
                                        if (cell.Clip.Instrument is InstrumentStop)
                                        {
                                            musician.IsPlaying = false;
                                            musician.NextCell = null;
                                        }
                                        //---

                                        //--- Direction
                                        bool divided = false;

                                        for (int i = 0; i < 6; i++)
                                        {
                                            if (cell.Clip.Directions[i])
                                            {
                                                if (divided)
                                                {
                                                    Musician newMusician = new Musician(channel);

                                                    newMusician.CurrentDirection = i;
                                                    newMusician.CurrentCell = cell;
                                                    newMusician.NextCell = cell.Neighbourghs[newMusician.CurrentDirection];

                                                    newMusicians.Add(newMusician);
                                                }

                                                if (!divided)
                                                {
                                                    musician.CurrentDirection = i;

                                                    divided = true;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (cell != null)
                                {
                                    //--- Met à jour la partition du musicien
                                    musician.Partition.Add(new TimeValue<Cell>(channel.ElapsedTime, cell));
                                    if (musician.IsPlaying)
                                        musician.NextCell = cell.Neighbourghs[musician.CurrentDirection];
                                    //---
                                }
                            }
                        }

                        //--- Création des nouveaux musiciens
                        foreach (Musician newMusician in newMusicians)
                        {
                            Musician musician = channel.GetMusicianNotPlaying();

                            if (musician != null)
                            {
                                musician.Partition.Add(new TimeValue<Cell>(channel.ElapsedTime, newMusician.CurrentCell));

                                musician.NextCell = newMusician.NextCell;
                                musician.CurrentDirection = newMusician.CurrentDirection;
                            }
                        }
                        //---

                        //--- Suppression des doublons de musiciens
                        foreach (Musician musician in channel.ListMusician)
                        {
                            if (musician.IsPlaying && musician.NextCell != null)
                            {
                                List<Musician> doublonMusician = channel.ListMusician.FindAll(
                                    m => m.IsPlaying &&
                                        m != musician &&
                                        m.Partition.Last() == musician.Partition.Last() &&
                                        m.NextCell != null &&
                                        m.NextCell.IndexPosition == musician.NextCell.IndexPosition
                                        );

                                foreach (Musician doublon in doublonMusician)
                                {
                                    //--- Création d'une cellule fictive pour la suppression du doublon
                                    Cell cell = doublon.Partition.Last().Value;
                                    Cell cellDoublon = new Cell(Context.Map, cell.Coord.X, cell.Coord.Y, cell.Location.X, cell.Location.Y);

                                    cellDoublon.InitClip();
                                    cellDoublon.Clip.Instrument = new InstrumentStop();
                                    //---

                                    doublon.IsPlaying = false;
                                    doublon.Partition.Add(new TimeValue<Cell>(doublon.Partition.Last().Time, cellDoublon));
                                    doublon.NextCell = null;
                                }
                            }
                        }
                        //---

                        //--- Incrémente le temps du channel selon la vitesse en cours
                        channel.ElapsedTime = channel.ElapsedTime.Add(new TimeSpan(0, 0, 0, 0, (int)(musicianSpeed / channel.GetSpeedFromTime(channel.ElapsedTime))));
                        //---

                        channelCalculationInProgress++;
                    }
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

            Context.NextMenu = item.ParentMenu.ParentMenu;
        }
        #endregion

        #region Menu Instrument
        void itemMenuInstrument_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            Menu menuInstrument = new Menu(item.ParentMenu.ParentCell, item.ParentMenu, true);

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

            Context.NextMenu = menuInstrument;
        }

        void itemInstrumentReset_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.Close(gameTime);

            item.ParentMenu.ParentCell.InitClip();
            item.ParentMenu.ParentCell.Clip.Instrument = null;

            Context.NextMenu = item.ParentMenu.ParentMenu;
        }

        void itemSample_Selected(Item item, GameTime gameTime)
        {
        }

        void itemNote_Selected(Item item, GameTime gameTime)
        {
        }

        void itemEffect_Selected(Item item, GameTime gameTime)
        {
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

                Context.NextMenu = item.ParentMenu.ParentMenu;
            }
        }

        void itemMusicianStop_Selected(Item item, GameTime gameTime)
        {
            item.ParentMenu.ParentCell.InitClip();
            item.ParentMenu.ParentCell.Clip.Instrument = new InstrumentStop();
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
