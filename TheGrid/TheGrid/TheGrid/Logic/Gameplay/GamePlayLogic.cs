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
using System.IO;
using System.Windows.Forms;
using TheGrid.Model.Effect;

namespace TheGrid.Logic.GamePlay
{
    public class GamePlayLogic
    {
        private Random rnd = new Random();
        //private float musicianSpeed = 500f;
        private TimeSpan lastEffectApplied = TimeSpan.Zero;

        public GameEngine GameEngine { get; set; }

        public GamePlayLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;

            InitializePlayers();



            //LoadMap("Test4");
            LoadMap("TestColor");
            //NewMap("Bass");
        }

        public void LoadMap(string levelFileName)
        {
            Context.Map = FileSystem.LoadLevel(this, Path.GetFileNameWithoutExtension(levelFileName));

            EvaluateMuscianGrid();
            //ribbon.Partition.Init();
            Stop();

            GameEngine.Sound.Init();
        }

        public void NewMap(string libraryName)
        {
            Stop();

            Context.Map = new Map(20, 20);
            Context.Map.SpeedFactor = 1f;
            Context.Map.PartitionDuration = new TimeSpan(0, 1, 0);

            InitializeChannel(Context.Map);
            GameEngine.UI.Ribbon.Partition.Init();

            LoadLibrary(libraryName, Context.Map);

            GameEngine.Sound.Init();
        }

        public void LoadLibrary(string libraryName, Map map)
        {
            map.LibraryName = libraryName;
            string libraryDirectory = Path.Combine(Directory.GetParent(Application.ExecutablePath).FullName, @"Sound\Library", libraryName);

            InitializeChannel(map);

            foreach (string channelDirectory in Directory.GetDirectories(libraryDirectory))
            {
                Channel channel = map.Channels.Find(c => c.Name.ToUpper() == Path.GetFileName(channelDirectory).ToUpper());

                if (channel != null)
                {
                    foreach (string sampleFileName in Directory.GetFiles(channelDirectory))
                    {
                        Sample sample = new Sample(channel, sampleFileName);
                        channel.ListSample.Add(sample);
                    }
                }
            }

            map.Channels.RemoveAll(c => c.ListSample.Count == 0 && c.Name != "Empty");
        }

        private void InitializeChannel(Map map)
        {
            map.Channels = new List<Channel>();

            map.Channels.Add(new Channel("Empty", new Color(0.3f, 0.3f, 0.3f)));
            map.Channels.Add(new Channel("Drum", new Color(1f, 0.2f, 0.2f)));
            map.Channels.Add(new Channel("Key", new Color(0f, 0.3f, 0.8f)));
            map.Channels.Add(new Channel("Guitar", new Color(0f, 0.6f, 0.3f)));
            map.Channels.Add(new Channel("Bass", new Color(0.7f, 0f, 0.5f)));
            map.Channels.Add(new Channel("String", new Color(237, 158, 0)));

            if (GameEngine.Mini)
            {
                foreach (Channel channel in map.Channels)
                {
                    channel.Color = Color.Lerp(Color.Black, channel.Color, 0.3f);
                }
            }

            foreach (Cell cell in map.Cells)
            {
                cell.Life = new float[map.Channels.Count];
            }
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
            {
                UpdateMusicians(gameTime);

                float lifeDec = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 10000f * Context.Map.SpeedFactor);

                foreach (Cell cell in Context.Map.Cells)
                {
                    for (int i = 0; i < cell.Life.Length; i++)
                    {
                        cell.Life[i] -= lifeDec;

                        if (cell.Life[i] < 0.01f)
                            cell.Life[i] = 0f;
                    }
                }
            }

            //GameEngine.Window.Title = Context.Time.ToString();
        }

        public void UpdateMusicians(GameTime gameTime)
        {
            //---> Met à jour le temps écoulé
            if (!Context.IsNavigatingThroughTime)
            {
                TimeSpan time = new TimeSpan((long)((float)gameTime.ElapsedGameTime.Ticks * Context.Map.SpeedFactor));
                Context.Time = Context.Time.Add(time);

                if (Context.Time >= Context.Map.PartitionDuration)
                    Context.IsPlaying = false;
            }

            //--- Met à jour les effets associés au channel
            bool applyEffect = false;

            if (lastEffectApplied == TimeSpan.Zero)
                lastEffectApplied = gameTime.TotalGameTime;

            if (gameTime.TotalGameTime.Subtract(lastEffectApplied).TotalMilliseconds > 300)
            {
                applyEffect = true;
                lastEffectApplied = gameTime.TotalGameTime;
            }
            //---

            foreach (Channel channel in Context.Map.Channels)
            {
                //--- Met à jour les effets associés au channel
                if (applyEffect)
                {
                    channel.Update(GameEngine.Sound, gameTime);
                }
                //---

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

                                //---> Si la prochaine cellule est une InstrumentStop, le musicien disparait
                                if (musician.CurrentCell.Clip != null && (musician.CurrentCell.Channel == null || musician.CurrentCell.Channel == musician.Channel) && musician.CurrentCell.Clip.Instrument is InstrumentStop)
                                {
                                    musician.IsPlaying = false;
                                }
                                else if (musician.CurrentCell.Clip != null)
                                {
                                    musician.IsPlaying = true;

                                    //---> Si la prochaine cellule est une InstrumentSample, le sample est joué
                                    if (musician.CurrentCell.Channel != null && musician.CurrentCell.Channel == musician.Channel && musician.CurrentCell.Clip.Instrument is InstrumentSample)
                                    {
                                        NewBornCell(musician.CurrentCell);

                                        GameEngine.Sound.PlaySample(((InstrumentSample)musician.CurrentCell.Clip.Instrument).Sample);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void NewBornCell(Cell cell)
        {
            int indexChannel = Context.Map.Channels.IndexOf(cell.Channel);

            cell.Life[indexChannel] = 1f;

            float rayon = 4f;
            foreach (Cell otherCell in Context.Map.Cells)
            {
                if (otherCell != cell && otherCell.Clip == null)
                {
                    float distance = Tools.Distance(cell.Location, otherCell.Location);

                    if (distance <= rayon)
                    {
                        float newLife = 1f - (distance / rayon);
                        if (otherCell.Life[indexChannel] < newLife)
                            otherCell.Life[indexChannel] = newLife;
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
                channel.ListSpeed = new List<TimeValue<float>>();
                channel.ListSpeed.Add(new TimeValue<float>(new TimeSpan(), 1f));
                channel.InitChannelEffect();

                if (channel.CellStart != null)
                {
                    Musician musician = channel.GetMusicianNotPlaying();
                    musician.IsPlaying = true;
                    musician.NextCell = channel.CellStart;
                }

                foreach (Musician musician in channel.ListMusician)
                {
                    musician.ElapsedTime = TimeSpan.Zero;
                }
            }
            //---

            int musicianCalculationInProgress = 1;

            while (musicianCalculationInProgress > 0)
            {
                musicianCalculationInProgress = 0;

                foreach (Channel channel in Context.Map.Channels)
                {
                    List<Musician> newMusicians = new List<Musician>();

                    foreach (Musician musician in channel.ListMusician)
                    {
                        if (musician.ElapsedTime < Context.Map.PartitionDuration && musician.NextCell != null)
                        {
                            Cell cell = musician.NextCell;
                            float speedFactor = 1f;

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
                                        float speed = channel.GetSpeedFromTime(musician.ElapsedTime);

                                        speed *= (0.22f * (float)(cell.Clip.Speed.Value)) + 1f;

                                        if (speed < 1f / 16f)
                                            speed = 1f / 16f;
                                        else if (speed > 4f)
                                            speed = 4f;

                                        channel.ListSpeed.Add(new TimeValue<float>(musician.ElapsedTime, speed));
                                    }
                                    //---

                                    //--- Note duration
                                    speedFactor = cell.Clip.Duration;
                                    //---

                                    //--- Instrument
                                    if (cell.Clip.Instrument is InstrumentStop)
                                    {
                                        musician.IsPlaying = false;
                                        musician.NextCell = null;
                                    }
                                    //---

                                    //--- Effet
                                    if (cell.Clip.Instrument is InstrumentEffect)
                                    {
                                        InstrumentEffect effect = cell.Clip.Instrument as InstrumentEffect;

                                        ChannelEffect channelEffect = cell.Channel.ListEffect.Find(ce => ce.Name == effect.ChannelEffect.Name);

                                        for (int i = 0; i < channelEffect.ListEffectProperty.Count; i++)
                                        {
                                            channelEffect.ListEffectProperty[i].Curve.Keys.Add(new CurveKey((float)musician.ElapsedTime.TotalMilliseconds, effect.ChannelEffect.ListEffectProperty[i].Value));
                                        }
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
                                                newMusician.ElapsedTime = musician.ElapsedTime;

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
                                musician.Partition.Add(new TimeValue<Cell>(musician.ElapsedTime, cell));
                                musician.ElapsedTime = musician.ElapsedTime.Add(new TimeSpan(0, 0, 0, 0, (int)(Context.Map.TimeDuration * speedFactor / channel.GetSpeedFromTime(musician.ElapsedTime))));

                                if (musician.IsPlaying)
                                    musician.NextCell = cell.Neighbourghs[musician.CurrentDirection];
                                //---

                                musicianCalculationInProgress++;
                            }
                        }
                    }

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

                    //--- Création des nouveaux musiciens
                    foreach (Musician newMusician in newMusicians)
                    {
                        Musician musician = channel.GetMusicianNotPlaying();

                        if (musician != null)
                        {
                            musician.Partition.Add(new TimeValue<Cell>(newMusician.ElapsedTime, newMusician.CurrentCell));

                            musician.NextCell = newMusician.NextCell;
                            musician.CurrentDirection = newMusician.CurrentDirection;
                        }
                    }
                    //---
                }
            }

            //--- Met à jour le visuel de la partition
            GameEngine.UI.Ribbon.Partition.Init();
            //---
        }

        public void UpdateMusiciansToTime()
        {
            Context.IsNavigatingThroughTime = true;

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
            if (Context.Time >= Context.Map.PartitionDuration)
                Stop();

            Context.IsPlaying = true;
        }

        public void Pause()
        {
            Context.IsPlaying = false;
        }

        public void Stop()
        {
            lastEffectApplied = TimeSpan.Zero;
            Context.IsPlaying = false;

            Context.Time = new TimeSpan(0, 0, 0);

            if (Context.Map != null)
            {
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

            GameEngine.Sound.Stop();
        }

        public void Backward(GameTime gameTime)
        {
            if (Context.Time >= TimeSpan.Zero)
            {
                float ratio = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
                TimeSpan time = new TimeSpan(0, 0, 0, 0, (int)(ratio * 2000 * Context.Map.SpeedFactor));
                Context.Time = Context.Time.Subtract(time);

                GameEngine.GamePlay.UpdateMusiciansToTime();
            }
        }

        public void Forward(GameTime gameTime)
        {
            if (Context.Time < Context.Map.PartitionDuration)
            {
                float ratio = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
                TimeSpan time = new TimeSpan(0, 0, 0, 0, (int)(ratio * 2000 * Context.Map.SpeedFactor));
                Context.Time = Context.Time.Add(time);

                GameEngine.GamePlay.UpdateMusiciansToTime();
            }
        }

        public void SpeedDown(GameTime gameTime)
        {
            float speedFactor = Context.Map.SpeedFactor;
            speedFactor -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3000f;

            if (speedFactor < 0f)
                speedFactor = 1f / 119f;

            Context.Map.SpeedFactor = speedFactor;
            EvaluateMuscianGrid();
        }

        public void SpeedUp(GameTime gameTime)
        {
            float speedFactor = Context.Map.SpeedFactor;
            speedFactor += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3000f;

            if (speedFactor > 2f)
                speedFactor = 2f;

            Context.Map.SpeedFactor = speedFactor;

            EvaluateMuscianGrid();
        }

        public TypePlaying MuteChannel(int idChannel)
        {
            TypePlaying prevTypePlaying = Context.Map.Channels[idChannel].TypePlaying;

            for (int i = 1; i < Context.Map.Channels.Count; i++)
            {
                if (i == idChannel)
                {
                    if (prevTypePlaying == TypePlaying.Muted)
                        Context.Map.Channels[i].TypePlaying = TypePlaying.Playing;
                    else
                        Context.Map.Channels[i].TypePlaying = TypePlaying.Muted;
                }
                else if (prevTypePlaying == TypePlaying.Muted && Context.Map.Channels[i].TypePlaying == TypePlaying.Solo)
                {
                    Context.Map.Channels[i].TypePlaying = TypePlaying.Playing;
                }
            }

            return Context.Map.Channels[idChannel].TypePlaying;
        }

        public TypePlaying SoloChannel(int idChannel)
        {
            TypePlaying prevTypePlaying = Context.Map.Channels[idChannel].TypePlaying;

            for (int i = 1; i < Context.Map.Channels.Count; i++)
            {
                if (i == idChannel)
                {
                    if (prevTypePlaying == TypePlaying.Solo)
                        Context.Map.Channels[i].TypePlaying = TypePlaying.Playing;
                    else
                        Context.Map.Channels[i].TypePlaying = TypePlaying.Solo;
                }
                else if (prevTypePlaying != TypePlaying.Solo)
                {
                    Context.Map.Channels[i].TypePlaying = TypePlaying.Muted;
                }
                else if (prevTypePlaying == TypePlaying.Solo)
                {
                    Context.Map.Channels[i].TypePlaying = TypePlaying.Playing;
                }
            }

            return Context.Map.Channels[idChannel].TypePlaying;
        }

        public TypePlaying MuteMusician(int idChannel, int idMusician)
        {
            for (int i = 0; i < Context.Map.Channels.Count; i++)
            {
                for (int j = 0; j < Context.Map.Channels[i].ListMusician.Count; j++)
                {
                    //if (i == idChannel && j == idMusician)
                    //    Context.Map.Channels[i].IsPlaying = !;
                }
            }

            return Context.Map.Channels[idChannel].ListMusician[idMusician].TypePlaying;
        }

        public TypePlaying SoloMusician(int idChannel, int idMusician)
        {
            for (int i = 0; i < Context.Map.Channels.Count; i++)
            {
                for (int j = 0; j < Context.Map.Channels[i].ListMusician.Count; j++)
                {
                    //Context.Map.Channels[i].IsPlaying = i == idChannel;
                }
            }

            return Context.Map.Channels[idChannel].ListMusician[idMusician].TypePlaying;
        }
    }
}
