using UnityEngine;
using System.Collections;

public class LoadButtonScript : MonoBehaviour {

    //Script is used in the main menu to create the buttons, and the square to contain them. Also makes the buttons load different levels.

    public Texture keyboardTexture;
    public Texture openCVTexture;
    public Texture quitTexture;
    public GUISkin ButtonSkin;

    void OnGUI()
    {
        GUI.skin = ButtonSkin;
        if (GUI.Button(new Rect(Screen.width/5.1f, Screen.height/3 * 1.05f, Screen.width/10 * 1.3f, Screen.height/10), keyboardTexture))
        {
            Application.LoadLevel("KeyboardScene");
        }
        if (GUI.Button(new Rect(Screen.width / 4.7f, Screen.height / 3 * 1.47f, Screen.width/10, Screen.height/10), openCVTexture))
        {
            Application.LoadLevel("OpenCVScene");
        }

        if (GUI.Button(new Rect(Screen.width / 4.5f, Screen.height / 3 * 1.98f, Screen.width/10 * 0.7f, Screen.height/10 * 0.7f), quitTexture))
        {
            Application.Quit();
        }
    }
}
