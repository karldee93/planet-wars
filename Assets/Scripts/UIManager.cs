using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    // these variable will be used to store various numeric information passed over from the missile script so that it can be displayed on the HUD.
    public float thrustAmount;
    public float xCoor;
    public float zCoor;
    public float score = 0;

    // these are text mesh pro classes which are set to public so that they can be given a text mesh pro object in the inspector.
    public TextMeshProUGUI thrustAmountTxt;
    public TextMeshProUGUI xCoorTxt;
    public TextMeshProUGUI zCoorTxt;
    public TextMeshProUGUI playerScore1;
    public TextMeshProUGUI finalScore;
    // Update is called once per frame
    void Update()
    {
        // this will access the text mesh pro component and get the text property and set it to whatever has been typed inside the "" and then also display the numeric floats and string data on the screen.
        // as it is in upfate it will always be up to date with the most recent numbers... this works the same for each statement.
        thrustAmountTxt.GetComponent<TextMeshProUGUI>().text = "Thrust Amount: " + thrustAmount.ToString();
        // the ("f2") will display the number to 2 decimal places as the coordinates otherwise would be too long to display on screen.
        xCoorTxt.GetComponent<TextMeshProUGUI>().text = "X Coordinates: " + xCoor.ToString("f2");
        zCoorTxt.GetComponent<TextMeshProUGUI>().text = "Z Coordinates: " + zCoor.ToString("f2");
        playerScore1.GetComponent<TextMeshProUGUI>().text = score.ToString();
        finalScore.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }
}
