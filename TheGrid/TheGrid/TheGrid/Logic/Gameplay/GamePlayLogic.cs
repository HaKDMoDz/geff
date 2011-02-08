using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Common;
using Microsoft.Xna.Framework;
using TheGrid.Model.UI.Menu;
using TheGrid.Model.Instrument;
using System.Xml.Serialization;
using System.Xml;
using TheGrid.Model.UI;

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

            InitializePlayers();

            Context.Map = FileSystem.LoadLevel("Test2");
            EvaluateMuscianGrid();
            Stop();

            GameEngine.UI.ListUIComponent.Add(new Ribbon(GameEngine.UI));
        }

        private void InitializeMap()
        {
            Context.Map = new Map(15, 15);
            Context.Map.CreateGrid();

            InitializeChannel();
            ////---
            //Cell cell1 = Context.Map.Cells[15];
            //cell1.InitClip();
            //cell1.Clip.Directions[3] = true;
            //cell1.Clip.Directions[2] = true;
            //cell1.Clip.Instrument = new InstrumentStart();
            //cell1.Channel = Context.Map.Channels[2];
            //Context.Map.Channels[2].CellStart = cell1;

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
            //cell1.Channel = Context.Map.Channels[2];
            //Context.Map.Channels[2].CellStart = cell1;

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
            //cell1.Channel = Context.Map.Channels[2];
            //Context.Map.Channels[2].CellStart = cell1;

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
            //cell1.Channel = Context.Map.Channels[2];
            //Context.Map.Channels[2].CellStart = cell1;

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
            cell1.Channel = Context.Map.Channels[2];
            Context.Map.Channels[2].CellStart = cell1;

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
            Context.Map.Channels = new List<Channel>();

            Context.Map.Channels.Add(new Channel("Empty", new Color(0.3f, 0.3f, 0.3f)));
            Context.Map.Channels.Add(new Channel("Drums", Color.Red));
            Context.Map.Channels.Add(new Channel("Keys", Color.Blue));
            Context.Map.Channels.Add(new Channel("Guitar", Color.Yellow));
            Context.Map.Channels.Add(new Channel("Bass", Color.Purple));
        }

        private void InitializePlayers()
        {
            Context.Players = new List<Player>();
            Context.Players.Add(new Player() { PlayerName = "Geff" });
            Context.CurrentPlayer = Context.Players[0];
        }

        public void Update(GameTime gameTime)
        {
            if (Context.IsPlaying || Context.IsNavigatingThroughTime)
                UpdateMusicians(gameTime);

            GameEngine.Window.Title = Context.Time.ToString();// +" - " + Context.Map.Channels[2].CountMusicianPlaying.ToString();
        }

        public void UpdateMusicians(GameTime gameTime)
        {
            //---> Met à jour le temps écoulé
            if (!Context.IsNavigatingThroughTime)
            {
                TimeSpan time = new TimeSpan((long)((float)gameTime.ElapsedGameTime.Ticks * Context.SpeedFactor));
                Context.Time = Context.Time.Add(time);

                if (Context.Time >= Context.PartitionDuration)
                    Context.IsPlaying = false;
            }

            foreach (Channel channel in Context.Map.Channels)
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
            foreach (Channel channel in Context.Map.Channels)
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

            int channelCalculationInProgress = Context.Map.Channels.Count;

            while (channelCalculationInProgress > 0)
            {
                channelCalculationInProgress = 0;

                foreach (Channel channel in Context.Map.Channels)
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
                                                else
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

        public void UpdateMusiciansToTime()
        {
            foreach (Channel channel in Context.Map.Channels)
            {
                foreach (Musician musician in channel.ListMusician)
                {
                    int index = musician.Partition.FindLastIndex(p => p.Time <= Context.Time);

                    if (index == -1)
                    {
                        musician.IsPlaying = false;
                        musician.CurrentIndex = 0;
                        musician.CurrentCell = null;
                    }
                    else
                    {
                        musician.CurrentIndex = index;
                        musician.CurrentCell = musician.Partition[index].Value;

                        musician.IsPlaying = true;

                        if (musician.CurrentCell.Clip != null && musician.CurrentCell.Clip.Instrument != null && musician.CurrentCell.Clip.Instrument is InstrumentStop)
                        {
                            musician.IsPlaying = false;
                        }
                    }
                }
            }
        }

        public void Play()
        {
            Context.IsPlaying = true;
        }

        public void Pause()
        {
            Context.IsPlaying = false;
        }

        public void Stop()
        {
            Context.IsPlaying = false;

            Context.Time = new TimeSpan(0, 0, 0);

            foreach (Channel channel in Context.Map.Channels)
            {
                foreach (Musician musician in channel.ListMusician)
                {
                    musician.CurrentCell = null;
                    musician.NextCell = null;
                    musician.IsPlaying = false;
                }
            }
        }

        public void Backward(GameTime gameTime)
        {
            Context.IsNavigatingThroughTime = true;

            if (Context.Time >= TimeSpan.Zero)
            {
                float ratio = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
                TimeSpan time = new TimeSpan(0, 0, 0, 0, (int)(ratio * 2000 * Context.SpeedFactor));
                Context.Time = Context.Time.Subtract(time);

                GameEngine.GamePlay.UpdateMusiciansToTime();
            }
        }

        public void Forward(GameTime gameTime)
        {
            Context.IsNavigatingThroughTime = true;
            if (Context.Time < Context.PartitionDuration)
            {
                float ratio = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
                TimeSpan time = new TimeSpan(0, 0, 0, 0, (int)(ratio * 2000 * Context.SpeedFactor));
                Context.Time = Context.Time.Add(time);

                GameEngine.GamePlay.UpdateMusiciansToTime();
            }
        }

        public void SpeedDown(GameTime gameTime)
        {
            Context.SpeedFactor -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;

            if (Context.SpeedFactor < 0f)
                Context.SpeedFactor = 0f;
        }

        public void SpeedUp(GameTime gameTime)
        {
            Context.SpeedFactor += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;

            if (Context.SpeedFactor > 4f)
                Context.SpeedFactor = 4f;
        }
    }
}
