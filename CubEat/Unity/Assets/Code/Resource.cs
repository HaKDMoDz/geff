using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Resource
{
    private static GameObject _cube;
    public static GameObject Cube 
    { 
        get
        {
            if (_cube == null)
                _cube = (GameObject)Resources.Load("Prefab/Cube");

            return _cube;
        }
    }

    public static Dictionary<int, AudioClip> GetAudioSamples(string libraryName)
    {
        Dictionary<int, AudioClip> dicAudioSamples = new Dictionary<int, AudioClip>();

        for (int i = 1; i < 20; i++)
        {
            AudioClip audioClip = (AudioClip)Resources.Load("Samples/" + libraryName + "/" + i.ToString());
            if (audioClip != null)
                dicAudioSamples.Add(i, audioClip);
            else
                break;
        }

        return dicAudioSamples;
    }

}
