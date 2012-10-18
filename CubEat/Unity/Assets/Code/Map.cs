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

    public Dictionary<int, List<Cube>> LayerCubes { get; set; }
    private List<AudioSource> _audioSources = new List<AudioSource>();
    private List<Color> channelColors = new List<Color>();

    public Map(int layerCount)
    {
        this.LayerCount = layerCount;
        this.Size = LayerCount * 2 - 1;
        this.LayerCubes = new Dictionary<int, List<Cube>>();

        AudioSamples = Resource.GetAudioSamples("Bass");

        for (int i = 0; i < 6; i++)
        {
            _audioSources.Add(GameObject.Find("Audio").GetComponents<AudioSource>()[i]);

            if (AudioSamples.ContainsKey(i + 1))
            {
                _audioSources[i].clip = AudioSamples[i + 1];

                _audioSources[i].rolloffMode = AudioRolloffMode.Linear;

            }
        }

        channelColors.Add(Color.blue);
        channelColors.Add(Color.green);
        channelColors.Add(Color.yellow);
        channelColors.Add(Color.Lerp(Color.yellow, Color.red, 0.3f));
        channelColors.Add(Color.red);
        channelColors.Add(Color.Lerp(Color.red, Color.blue, 0.5f));

        CreateMap();
    }

    public void CreateMap()
    {
        System.Random rnd = new System.Random();

        for (int layer = 0; layer < LayerCount; layer++)
        {
            int numberOnLayer = 0;
            for (int side = 0; side < 4; side++)
            {
                for (int step = 0; step < layer * 2 || (layer == 0 && step == 0); step++)
                {
                    if (layer == 0 && side > 0)
                        continue;

                    int p0 = Size / 2 - layer;
                    int p1 = Size / 2 + layer;
                    int x = 0;
                    int y = 0;

                    GameObject cubeObject = (GameObject)GameObject.Instantiate(Resource.Cube);
                    Cube cube = cubeObject.GetComponentInChildren<Cube>();
                    cube.Layer = layer;
                    cube.NumberOnLayer = numberOnLayer;
                    cube.IsOnMeasure = cube.NumberOnLayer % 4 == 0;
                    cube.Color = channelColors[layer];

                    cube.IsEmpty = true;

                    //if (rnd.NextDouble() > 0.7)
                    //    cube.IsEmpty = false;

                    numberOnLayer++;

                    List<Cube> cubesOnLayer = null;

                    if (LayerCubes.ContainsKey(layer))
                    {
                        cubesOnLayer = LayerCubes[layer];
                    }
                    else
                    {
                        cubesOnLayer = new List<Cube>();
                        LayerCubes.Add(layer, cubesOnLayer);
                    }

                    cubesOnLayer.Add(cube);

                    if (side == 0)
                    {
                        x = p0 + step;
                        y = p0;
                    }
                    else if (side == 1)
                    {
                        x = p0 + 2 * layer;
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

    public void Update()
    {
        Beat++;

        for (int i = 0; i < LayerCount; i++)
        {
            foreach (Cube cube in LayerCubes[i])
            {
                if (!cube.IsEmpty)
                {
                    cube.IsEmitting = false;
                    cube.IsInPlayedTime = false;

                    //=== Détermination des cubes joués dans le temps courant

                    //--- Nombre de cases sur la couche
                    int numberOfCubeOnLayer = i * 8;

                    if (numberOfCubeOnLayer == 0)
                        numberOfCubeOnLayer = 1;

                    //--- Beat  de la couche
                    int layerBeat = Beat % numberOfCubeOnLayer;

                    if (cube.NumberOnLayer == layerBeat)
                        PlayCube(cube);
                }
            }
        }
    }

    public void PlayCube(Cube cube)
    {
        //int index = this.LayerCubes[cube.Layer].IndexOf(cube);

        cube.animation.wrapMode = WrapMode.Once;
        cube.animation.Play();
        
        if (AudioSamples.ContainsKey(cube.Layer+1))
        {
            if (_audioSources[cube.Layer].isPlaying)
                _audioSources[cube.Layer].Stop();

            _audioSources[cube.Layer].Play();
        }
    }
}
