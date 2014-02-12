using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;
using UnityEngine;

public class Map
{
    public int Beat { get; set; }
    public int Size { get; set; }
    public int LayerCount { get; set; }
    public Dictionary<int, AudioClip> AudioSamples { get; set; }

    //public Dictionary<int, List<Cube>> LayerCubes { get; set; }

    public List<Layer> Layers { get; set; }

    private List<AudioSource> _audioSources = new List<AudioSource>();
    private List<Color> channelColors = new List<Color>();
    double _initialTime;
    private Cubeat _cubeat;

    public Map(string audioLibrary, Cubeat cubeat)
    {
        _cubeat = cubeat;
        _initialTime = AudioSettings.dspTime;

        _audioSources.AddRange(GameObject.Find("Audio").GetComponents<AudioSource>());


        channelColors.Add(Color.white);
        channelColors.Add(Color.green);
        channelColors.Add(Color.yellow);
        channelColors.Add(Color.Lerp(Color.yellow, Color.red, 0.3f));
        channelColors.Add(Color.red);
        channelColors.Add(Color.Lerp(Color.red, Color.blue, 0.5f));
        channelColors.Add(new Color(0.2f, 0.6f, 0.8f));
        channelColors.Add(new Color(0.6f, 0.6f, 0.6f));

        this.LayerCount = 8;
        CreateMap();

        ReadSamples(audioLibrary);
    }

    public void ReadSamples(string audioLibrary)
    {
        AudioSamples = Resource.GetAudioSamples(audioLibrary);
        this.LayerCount = AudioSamples.Count + 1;
        this.Size = LayerCount * 2 - 1;

        for (int i = 0; i < this.LayerCount; i++)
        {
            if (AudioSamples.ContainsKey(i + 1))
            {
                _audioSources[i].clip = AudioSamples[i + 1];

                _audioSources[i].rolloffMode = AudioRolloffMode.Linear;
            }
        }

        if (this.Layers != null)
        {
            foreach (Layer layer in this.Layers)
            {
                foreach (Cube cube in layer.Cubes)
                {
                    cube.gameObject.SetActive(cube.Layer < this.LayerCount);
                }
            }
        }
    }

    public void CreateMap()
    {
        this.Layers = new List<Layer>();

        System.Random rnd = new System.Random();

        for (int layerID = 0; layerID < LayerCount; layerID++)
        {
            Layer layer = new Layer();
            this.Layers.Add(layer);

            int numberOnLayer = 0;
            for (int side = 0; side < 4; side++)
            {
                for (int step = 0; step < layerID * 2 || (layerID == 0 && step == 0); step++)
                {
                    if (layerID == 0 && side > 0)
                        continue;

                    int p0 = Size / 2 - layerID;
                    int p1 = Size / 2 + layerID;
                    int x = 0;
                    int y = 0;

                    GameObject cubeObject = (GameObject)GameObject.Instantiate(Resource.Cube);
                    Cube cube = cubeObject.GetComponentInChildren<Cube>();
                    cube.Layer = layerID;
                    cube.NumberOnLayer = numberOnLayer;
                    cube.IsOnMeasure = cube.NumberOnLayer % 4 == 0;

                    if (channelColors.Count > layerID)
                        cube.Color = channelColors[layerID];

                    cube.IsEmpty = true;

                    //if (rnd.NextDouble() > 0.7)
                    //    cube.IsEmpty = false;

                    numberOnLayer++;

                    //List<Cube> cubesOnLayer = null;

                    layer.Cubes.Add(cube);

                    //if (LayerCubes.ContainsKey(layer))
                    //{
                    //    cubesOnLayer = LayerCubes[layer];
                    //}
                    //else
                    //{
                    //    cubesOnLayer = new List<Cube>();
                    //    LayerCubes.Add(layer, cubesOnLayer);
                    //}

                    //cubesOnLayer.Add(cube);

                    if (side == 0)
                    {
                        x = p0 + step;
                        y = p0;
                    }
                    else if (side == 1)
                    {
                        x = p0 + 2 * layerID;
                        y = p0 + step;
                    }
                    else if (side == 2)
                    {
                        x = p1 - step;
                        y = p1;
                    }
                    else if (side == 3)
                    {
                        x = p0;
                        y = p1 - step;
                    }

                    //Debug.Log(String.Format("layer : {0} // side : {1} // step  : {2} // p0 : {3} // x : {4} // y : {5}", layer, side, step, p0, x, y));

                    cubeObject.transform.localPosition = new Vector3(cube.transform.localScale.x * (x - (Size / 2 - 1)), 0, cube.transform.localScale.z * (y - (Size / 2 - 1)));
                }
            }
        }
    }

    double prevTime = 0;
    
    public void Update(double time)
    {
        //Beat++;

        double curTime = time - _initialTime;
        if (prevTime == 0)
            prevTime = curTime;

        for (int i = 0; i < Layers.Count; i++)
        {
            //double curLayerTime = curTime % (double)Layers[i].Cubes.Count;
            double layerDuration = (double)Layers[i].Cubes.Count * 60.0 / _cubeat.BPM;

            foreach (Cube cube in Layers[i].Cubes)
            {
                if (!cube.IsEmpty)
                {
                        
                    cube.IsEmitting = false;
                    cube.IsInPlayedTime = false;

                    double cubeBeatTime = (double)cube.NumberOnLayer * 60.0 / _cubeat.BPM;
                    double playNextTime = _initialTime + cubeBeatTime + layerDuration * (int)(curTime / layerDuration);

                    //Debug.Log((prevTime + _initialTime) + " ___ " + playNextTime + " ___ " + (curTime + _initialTime));

                    if (prevTime+_initialTime <= playNextTime+0.05F && playNextTime+0.05F < curTime+_initialTime)
                    {
                        cube.PlayNextTime = playNextTime;

                        PlayCube(cube, cube.PlayNextTime);
                    }

                }
            }
        }

        prevTime = curTime;
    }

    public void PlayCube(Cube cube, double time)
    {
        _audioSources[cube.Layer - 1].PlayScheduled(time);

        //cube.animation.wrapMode = WrapMode.Once;
        cube.animation.Play();

        //if (AudioSamples.ContainsKey(cube.Layer))
        //{
        //    if (_audioSources[cube.Layer - 1].isPlaying)
        //        _audioSources[cube.Layer - 1].Stop();

        //    _audioSources[cube.Layer - 1].Play();
        //}
    }
}
