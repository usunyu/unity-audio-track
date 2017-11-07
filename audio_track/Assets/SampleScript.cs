using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio;

public class SampleScript : MonoBehaviour
{
    public Text[] texts;
    private AudioTrackInfo[] _tracks = null;

    // Use this for initialization
    void Start()
    {
        string mediaFile = Application.dataPath + "/../../Track.mp4";
        //string mediaFile = "c:/Users/usuny/Desktop/Track.mp4";
        _tracks = AudioTrack.Instance.QueryAudioTrack(mediaFile);
        int i = 0;
        foreach (AudioTrackInfo track in _tracks)
        {
            Debug.Log(track.ToString());
            if (i < texts.Length)
                texts[i].text = track.ToString();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}