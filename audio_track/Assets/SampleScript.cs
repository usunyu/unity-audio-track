using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class SampleScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        string mediaFile = Application.dataPath + "/../../Track.mp4";
        AudioTrackInfo[] tracks = AudioTrack.Instance.QueryAudioTrack(mediaFile);
        foreach (AudioTrackInfo track in tracks)
        {
            Debug.Log(track.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}