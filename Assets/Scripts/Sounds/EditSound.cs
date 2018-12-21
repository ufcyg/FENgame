using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSound : MonoBehaviour {

    public float start;
    public float stop;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<AudioSource>().clip = MakeSubclip(this.GetComponent<AudioSource>().clip, start, stop);
    }

    private AudioClip MakeSubclip(AudioClip clip, float start, float stop)
    {
        /* Create a new audio clip */
        int frequency = Mathf.RoundToInt(clip.frequency * 1);
        float timeLength = stop - start;
        int samplesLength = (int)(frequency * timeLength);
        AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 2, frequency, false);
        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength];
        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));
        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);
        /* Return the sub clip */
        return newClip;
    }
}
