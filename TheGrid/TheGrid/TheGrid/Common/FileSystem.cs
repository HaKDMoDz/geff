using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using TheGrid.Model;
using System.Windows.Forms;
using System.IO;
using TheGrid.Model.Instrument;
using TheGrid.Logic.GamePlay;

namespace TheGrid.Common
{
    public static class FileSystem
    {
        public static void SaveLibraryConfig(Map map)
        {
            string libraryConfig = Path.Combine(Directory.GetParent(Application.ExecutablePath).FullName, @"Files\Sound\Library", map.LibraryName, map.LibraryName + "_Config.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            XmlWriter writer = new XmlTextWriter(libraryConfig, Encoding.UTF8);
            serializer.Serialize(writer, map);
            writer.Close();
        }

        public static void SaveLevel(Map map, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            XmlWriter writer = new XmlTextWriter(Path.Combine(Path.GetFullPath(Application.ExecutablePath), @"..\Files\Level", fileName + ".xml"), Encoding.UTF8);
            serializer.Serialize(writer, map);
            writer.Close();
        }

        public static Map LoadLevelConfig(GamePlayLogic gamePlay, string libraryName)
        {
            string fileName = Path.Combine(Directory.GetParent(Application.ExecutablePath).FullName, @"Files\Sound\Library", libraryName, libraryName + "_Config.xml");

            return LoadLevelFile(gamePlay, fileName, true);
        }

        public static Map LoadLevel(GamePlayLogic gamePlay, string levelName)
        {
            string fileName = Path.Combine(Path.GetFullPath(Application.ExecutablePath), @"..\Files\Level", levelName + ".xml");

            return LoadLevelFile(gamePlay, fileName, false);
        }

        private static Map LoadLevelFile(GamePlayLogic gamePlay, string fileName, bool config)
        {
            if (!File.Exists(fileName))
                return null;

            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            XmlReader reader = new XmlTextReader(fileName);

            Map map = (Map)serializer.Deserialize(reader);

            reader.Close();

            if (!config)
                gamePlay.LoadLibrary(map.LibraryName, map);
            else
                map.Cells.Clear();

            //---
            foreach (Cell cell in map.Cells)
            {
                cell.Map = map;
                cell.InitialLocation = cell.Location;

                if (cell.Channel != null)
                {
                    cell.Channel = map.Channels.Find(c => c.Name == cell.Channel.Name);
                }

                if (cell.Clip != null && cell.Clip.Instrument != null && cell.Clip.Instrument is InstrumentSample)
                {
                    InstrumentSample instrument = cell.Clip.Instrument as InstrumentSample;

                    instrument.Sample = cell.Channel.ListSample.Find(s => s.Name == instrument.Sample.Name);
                }

                if (cell.Channel != null && cell.Clip != null && cell.Clip.Instrument is InstrumentStart)
                    cell.Channel.CellStart = cell;
            }
            //---

            if (map.SpeedFactor == 0f)
                map.SpeedFactor = 1f;

            map.SpeedFactor = map.SpeedFactor;

            if (map.PartitionDuration == TimeSpan.Zero)
                map.PartitionDuration = new TimeSpan(0, 1, 0);

            if (map.TimeDuration == 0f)
                map.TimeDuration = 500f;

            foreach (Channel channel in map.Channels)
            {
                if (channel.Name != "Empty")
                {
                    channel.InitChannelEffect();
                }
            }

            map.CalcNeighborough();

            return map;
        }
    }
}
