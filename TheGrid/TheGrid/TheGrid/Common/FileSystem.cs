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

        public static Map LoadLevel(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            XmlReader reader = new XmlTextReader(Path.Combine(Path.GetFullPath(Application.ExecutablePath), @"..\Level", fileName + ".xml"));

            Map map = (Map)serializer.Deserialize(reader);

            //---
            foreach (Cell cell in map.Cells)
            {
                cell.Map = map;

                if (cell.Channel != null)
                {
                    cell.Channel = map.Channels.Find(c => c.Name == cell.Channel.Name);
                }

                //TODO : gérer le chargement des samples

                if (cell.Channel != null && cell.Clip != null && cell.Clip.Instrument is InstrumentStart)
                    cell.Channel.CellStart = cell;
            }
            //---
            
            map.CalcNeighborough();

            return map;
        }
    }
}
