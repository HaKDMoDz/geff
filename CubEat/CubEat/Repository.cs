using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using IrrKlang;

namespace CubEat
{
    public class Repository
    {
        public Map Map1 { get; set; }
        public Map Map2 { get; set; }

        public List<LibraySample> ListLibraySample { get; set; }
        public int Beat { get; set; }
        public static Random rnd;

        public Dictionary<int, int> PlayedSample;

        private ISoundEngine _soundEngine;

        public Repository()
        {
            Init();
        }

        public void CreateNewMap(int size, LibraySample libraySample)
        {
            this.Map1 = new Map(size, libraySample);
            this.Map1.CreateMapWithSamples();
            this.Map2 = new Map(size, libraySample);
            this.Map2.CreateMapWithSamples();
        }

        private void Init()
        {
            _soundEngine = new ISoundEngine();

            rnd = new Random();
            PlayedSample = new Dictionary<int, int>();

            CreateSampleModel();
        }

        private void CreateSampleModel()
        {
            ListLibraySample = new List<LibraySample>();

            string directory = Path.Combine(Application.StartupPath, "Samples");

            foreach (String subDirectory in Directory.GetDirectories(directory))
            {
                LibraySample libraySample = new LibraySample(Path.GetFileName(subDirectory));
                this.ListLibraySample.Add(libraySample);

                foreach (String file in Directory.GetFiles(Path.Combine(directory, subDirectory)))
                {
                    Color color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    SampleModel sampleModel = new SampleModel(libraySample, Path.GetFileNameWithoutExtension(file), color, 1, libraySample.ListSampleModel.Count);
                    sampleModel.SoundSource = _soundEngine.AddSoundSourceFromFile(
                    Application.StartupPath + "\\" + "Samples" + "\\" + libraySample.Name + "\\" + sampleModel.Name + ".wav");

                    libraySample.ListSampleModel.Add(sampleModel);
                }
            }
        }

        public void Update()
        {
            Beat++;

            UpdateMap(Map1);
            //UpdateMap(Map2);
        }

        private void UpdateMap(Map map)
        {

            for (int x = 0; x < map.Size; x++)
            {
                for (int y = 0; y < map.Size; y++)
                {
                    if (map.Cells[x, y] != null)
                    {
                        map.Cells[x, y].IsEmitting = false;
                        map.Cells[x, y].IsInPlayedTime = false;


                        //=== Détermination des Cellules jouées dans le temps courant

                        //--- Nombre de cases sur la couche
                        int numberOfCellOnLayer = (map.Size / 2 - map.Cells[x, y].Layer+1) * 8;
                        //---

                        //--- Beat  de la couche
                        int layerBeat = Beat % numberOfCellOnLayer;
                        //---

                        //--- 
                        int minPlayedCell = (layerBeat / 4) * 4;
                        int maxPlayedCell = ((layerBeat / 4) + 1) * 4;

                        if (map.Cells[x, y].NumberOnLayer >= minPlayedCell &&
                            map.Cells[x, y].NumberOnLayer <= maxPlayedCell)
                        {
                            map.Cells[x, y].IsInPlayedTime = true;

                        }
                        //===
                    }
                }
            }

            for (int layer = 0; layer < map.Size / 2; layer++)
            {
                //--- Nombre de cases sur la couche
                int numberOfCellOnLayer = (map.Size / 2 - layer) * 8;
                //---

                //--- Beat  de la couche
                int layerBeat = Beat % numberOfCellOnLayer;
                //---

                //--- Couche de l'instrument principal
                if (layer == (map.Size / 2) - 2 && layerBeat == 0)
                {
                    map.DrainPlayedSample();
                }
                //---

                //--- Point de départ (coin haut/gauche de la couche)
                Point pos = new Point();
                pos.Offset(layer, layer);
                //---

                //--- Calcul de la longueur d'un côté 
                int sideLength = map.Size - layer * 2;
                //---

                //--- Détermination du côté courant
                int currentSide = (layerBeat - 1) / (sideLength - 1);
                //---

                if (currentSide == 0)
                {
                    pos.X += layerBeat;
                }
                else if (currentSide == 1)
                {
                    pos.X += sideLength - 1;
                    pos.Y += layerBeat - sideLength + 1;
                }
                else if (currentSide == 2)
                {
                    pos.X += (sideLength - 1) * 3 - layerBeat;
                    pos.Y += sideLength - 1;
                }
                else if (currentSide == 3)
                {
                    pos.Y += (sideLength - 1) * 4 - layerBeat;
                }

                if (map.Cells[pos.X, pos.Y] != null)
                {
                    map.Cells[pos.X, pos.Y].IsEmitting = true;

                    if (
                        //layer ==0 && 
                        !map.Cells[pos.X, pos.Y].IsEmpty)
                    {
                        PlaySample(map.Cells[pos.X, pos.Y].Sample);

                        int sampleId = map.Cells[pos.X, pos.Y].Sample.SampleModel.SampleModelId;

                        map.PlayedSample[sampleId]++;

                        //if (!PlayedSample.ContainsKey(sampleId))
                        //    PlayedSample.Add(sampleId, 0);

                        //PlayedSample[sampleId] = PlayedSample[sampleId] + 1;
                    }
                }


            }
        }

        public void PlaySample(Sample sample)
        {
            PlaySample(sample.SampleModel);
        }

        public void PlaySample(SampleModel sample)
        {
            _soundEngine.Play2D(Application.StartupPath + "\\" + "Samples" + "\\" + sample.LibraySample.Name + "\\" + sample.Name + ".wav", false);
        }

        internal void StopSound()
        {
            _soundEngine.SetAllSoundsPaused(true);
            _soundEngine.StopAllSounds();
        }
    }
}
