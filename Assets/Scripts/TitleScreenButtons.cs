using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenButtons : MonoBehaviour
{
    // this will hold the instructions menu game object so that it can be turned off and on when required.
    public GameObject instructionsMenu;
    // this will hold the main menu game object so that it can be turned off and on when required.
    public GameObject mainMenu;
    
    // function will launch the game level.
    public void Play()
    {
        // load scene 1 from build options.
        SceneManager.LoadScene(1);
    }

    // function will quit the application.
    public void Quit()
    {
        // close the application.
        Application.Quit();
    }

    // function will deactivate the main menu and display intructions menu.
    public void Instructions()
    {
        // set the game object contained within instructionsMenu to active.
        instructionsMenu.SetActive(true);
        // set the game object contained within mainMenu to inactive.
        mainMenu.SetActive(false);
    }

    // function will achieve the opposite of the above funtion.
    public void Back()
    {
        instructionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
