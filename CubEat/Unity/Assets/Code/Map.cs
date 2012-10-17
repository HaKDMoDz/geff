using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int Beat { get; set; }
    //public Cube[,] Cubes { get; set; }
    public int Size { get; set; }
    public int LayerCount { get; set; }
    public Dictionary<int, int> PlayedSample { get; set; }

    public Dictionary<int, List<Cube>> LayerCubes { get; set; }

    public Map(int layerCount)
    {
        this.LayerCount = layerCount;
        this.Size = LayerCount * 2 - 1;
        //this.Cubes = new Cube[size, size];
        this.LayerCubes = new Dictionary<int, List<Cube>>();
        this.DrainPlayedSample();
        CreateMap();
    }

    public void CreateMap()
    {
        /*ystem.Random rnd = new System.Random( );

        *
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                GameObject cubeObject = (GameObject)Instantiate(Resource.Cube, new Vector3(x * Resource.Cube.transform.localScale.x - (Size - 1) * Resource.Cube.transform.localScale.x / 2f, 1f, y * Resource.Cube.transform.localScale.z - (Size - 1) * Resource.Cube.transform.localScale.z / 2f), Quaternion.identity);

                Cube cube = cubeObject.GetComponentInChildren<Cube>();

                cube.IsEmpty = true;
                Cubes[x, y] = cube;

                if (!(x == Size / 2 && y == Size / 2) && rnd.NextDouble() > 0.809016994)
                {
                    int center = (Size / 2);
                    int distance = (int)Math.Sqrt((x - center) * (x - center) + (y - center) * (y - center));

                    //if (CurrentLibrarySample.ListSampleModel.Count - 1 < distance)
                    //{
                    //    distance = CurrentLibrarySample.ListSampleModel.Count - 1;
                    //}

                    //Cube.TypeSymbol = (TypeSymbol)Repository.rnd.Next(1, 4);
                    //Cube.Sample = new Sample(CurrentLibrarySample.ListSampleModel[distance - 1]);
                    cube.IsEmpty = false;
                }

                ComputeCubeLayerAndNumber(cube, x, y);
            }
        }*/


        for (int layer = 0; layer < LayerCount; layer++)
        {
            int numberOnLayer = 0;
            for (int side = 0; side < 2; side++)
            {
                for (int s = 0; s < layer*2 || (layer == 0 && s==0); s++)
                    //s < -1 + 2 * (layer + 1); s++)
                {
                    if (layer == 0 && side > 0)
                        continue;

                    int p0 = Size / 2 - layer;
                    int x = 0;
                    int y = 0;

                    GameObject cubeObject = (GameObject)Instantiate(Resource.Cube);//, new Vector3(x * Resource.Cube.transform.localScale.x - (Size - 1) * Resource.Cube.transform.localScale.x / 2f, 1f, y * Resource.Cube.transform.localScale.z - (Size - 1) * Resource.Cube.transform.localScale.z / 2f), Quaternion.identity);
                    Cube cube = cubeObject.GetComponentInChildren<Cube>();
                    cube.Layer = layer;
                    cube.NumberOnLayer = numberOnLayer;
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
                        x = p0 + s;
                        y = p0;
                    }
                    else if (side == 1)
                    {
                        x = p0+2*layer;
                        y = p0 + s;
                    }
                    else if (side == 2)
                    {
                        x = -1 + 2 * (layer+1) - s;
                        y = -1 + 2 * (layer+1);
                    }
                    else if (side == 3)
                    {
                        x = p0;
                        y = -1 + 2 * (layer+1) - s;
                    }


                    Debug.Log(String.Format("layer : {0} // side : {1} // step  : {2} // p0 : {3} // x : {4} // y : {5}", layer, side, s, p0, x, y));


                    cubeObject.transform.localPosition = new Vector3(cube.transform.localScale.x * (x - (Size / 2 - 1)), 0, cube.transform.localScale.z * (y - (Size / 2 - 1)));
                }
            }
        }
    }

    private void ComputeCubeLayerAndNumber(Cube cube, int x, int y)
    {
        Point center = new Point(Size / 2, Size / 2);
        Point point = new Point(x, y);
        Point delta = new Point(point.X - center.X, point.Y - center.Y);

        for (int layer = 1; layer <= Size / 2; layer++)
        {
            if (delta.Y == -layer && delta.X >= -layer && delta.X <= layer)
            {
                cube.Layer = layer;
                cube.NumberOnLayer = delta.X + layer;
            }
            else if (delta.Y == layer && delta.X >= -layer && delta.X <= layer)
            {
                cube.Layer = layer;
                cube.NumberOnLayer = layer * 5 - delta.X;
            }
            else if (delta.X == layer && delta.Y >= -layer && delta.Y <= layer)
            {
                cube.Layer = layer;
                cube.NumberOnLayer = layer * 3 + delta.Y;
            }
            else if (delta.X == -layer && delta.Y >= -layer && delta.Y <= layer)
            {
                cube.Layer = layer;
                cube.NumberOnLayer = layer * 7 - delta.Y;
            }
        }

        //--- Ajout du cube à la représentation par couche
        if (!this.LayerCubes.ContainsKey(cube.Layer))
            this.LayerCubes.Add(cube.Layer, new List<Cube>());

        this.LayerCubes[cube.Layer].Add(cube);
        //---

        cube.IsOnMeasure = cube.NumberOnLayer % 4 == 0;
    }

    public void DrainPlayedSample()
    {
        //PlayedSample = new Dictionary<int, int>();
        //for (int i = 0; i < CurrentLibrarySample.ListSampleModel.Count; i++)
        //{
        //    PlayedSample.Add(CurrentLibrarySample.ListSampleModel[i].SampleModelId, 0);
        //}
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
                    //---

                    if (numberOfCubeOnLayer == 0)
                        numberOfCubeOnLayer = 1;

                    //--- Beat  de la couche
                    int layerBeat = Beat % numberOfCubeOnLayer;
                    //---

                    //--- 
                    int minPlayedCube = (layerBeat / 4) * 4;
                    int maxPlayedCube = ((layerBeat / 4) + 1) * 4;

                    if (cube.NumberOnLayer >= minPlayedCube &&
                        cube.NumberOnLayer < maxPlayedCube)
                    {
                        PlayCube(cube);
                    }
                }
            }
        }
    }

    public void PlayCube(Cube cube)
    {
        int index = this.LayerCubes[cube.Layer].IndexOf(cube);

        cube.animation.wrapMode = WrapMode.Once;
        cube.animation.Play();
    }
}
