using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MuteSound : MonoBehaviour
{
    // these variables will be used to access the various sound effects.
    public AudioSource backgroundMusic, missileExplosion, missileLaunch;
    // allows the audio sources attached to the planet to be access in order to mute them.
    public GameObject planet1Effects, planet2Effects, planet3Effects;
    // checks if the game is paused.
    public bool isPaused;
    // these will offer a way to access the properties of the buttons in the script and will be assigned in the inspector.
    public Button muteButton, soundEffectsButton;
    // these will be used to store the mute and unmute icon images.
    public Sprite muteImage;
    public Sprite unmutedImage;

    private void Start()
    {
        // set the mute button variable == the button this script is attached to.
        muteButton = GetComponent<Button>();
    }

    // this will handle muting the games music.
    public void MuteMusic()
    {
        // check if background music audio source is playing if it is then run code.
        if (backgroundMusic.isPlaying)
        {
            // access the button components sprite property and set the sprite to = the sprite contained in the muteImage variable.
            muteButton.image.sprite = muteImage; 
            // set pause game == true.
            isPaused = true;
            // call on the audio source and pause it.
            backgroundMusic.Pause();
        }
        // if backgroundMusic is not playing run code.
        else
        {
            // code works same as above but opposite.
            muteButton.image.sprite = unmutedImage;
            isPaused = false;
            backgroundMusic.Play();
        }
    }
    // this will mute the sound effects in the game when called.
    public void MuteSoundEffects()
    {
        // if one sound effect is not muted then all must not be muted so run code.
        if(missileExplosion.mute == false)
        {
            // works same as above to change image.
            soundEffectsButton.image.sprite = muteImage;
            // calls on the missileExplosion/ launch audio source and mutes it.
            missileExplosion.mute = true;
            missileLaunch.mute = true;
            // sets the game objects contained in these variables to false which in this case are game object which contain various audio sources.
            planet1Effects.SetActive(false);
            planet2Effects.SetActive(false);
            planet3Effects.SetActive(false);
        }
        // this works same as above but opposite.
        else
        {
            soundEffectsButton.image.sprite = unmutedImage;
            missileExplosion.mute = false;
            missileLaunch.mute = false;
            planet1Effects.SetActive(true);
            planet2Effects.SetActive(true);
            planet3Effects.SetActive(true);
        }
    }
}
