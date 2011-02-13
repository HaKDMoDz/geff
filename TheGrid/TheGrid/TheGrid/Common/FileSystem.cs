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
        public static void SaveLevel(Map map, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            XmlWriter writer = new XmlTextWriter(Path.Combine(Path.GetFullPath(Application.ExecutablePath), @"..\Level", fileName + ".xml") , Encoding.UTF8);
            serializer.Serialize(writer, map);
            writer.Close();
        }

        public static Map LoadLevel(GamePlayLogic gamePlay, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            XmlReader reader = new XmlTextReader(Path.Combine(Path.GetFullPath(Application.ExecutablePath), @"..\Level", fileName + ".xml"));

            Map map = (Map)serializer.Deserialize(reader);

            gamePlay.LoadLibrary(map.LibraryName, map);

            //---
            foreach (Cell cell in map.Cells)
            {
                cell.Map = map;

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
            
            map.CalcNeighborough();

            return map;
        }
    }
}
