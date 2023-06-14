using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // an array is set up to hold the various audios to be played.
    public AudioClip[] clips;
    // reference to audio source
    public AudioSource audioSource;
    // reference to mute sound script.
    public MuteSound mute;
    // used to cycle through music clips.
    int clipNum;
    // Start is called before the first frame update
    void Start()
    {
        // set the audio source to not loop.
        audioSource.loop = false;
    }

    private AudioClip GetClip()
    {
        // add one to clip num.
        clipNum += 1;
        // once array length has been reached set clip num back to 0.
        if (clipNum > clips.Length)
        {
            clipNum = 0;
        }
        // return the clip held at index of clipNum value.
        return clips[clipNum];
    }

    // Update is called once per frame
    void Update()
    {
        // if the music clip has ended run code.
        if (!audioSource.isPlaying)
        {
            // check that sound has not been muted by the player.
            if (mute.GetComponent<MuteSound>().isPaused == false)
            {
                // audio will equal the result of this function call.
                audioSource.clip = GetClip();
                // play the chosen audio source.
                audioSource.Play();
            }
        }
    }
}
