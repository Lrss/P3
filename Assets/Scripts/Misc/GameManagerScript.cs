using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class GameManagerScript : MonoBehaviour {

    //Script that handles win/lose conditions and makes sure the proper gui is applied to the viewport.

    public GUISkin guiSkin; //A skin applied to all lables and such to make sure things like hover is removed.
    private float gameOverTimer = 90;
    private string gameOverString;
    private float timer = 5; //Time before players are kicked to menu
    private bool menuKickEminent; 
    public Texture bootscreenBG; //Texture of the square with the lose/win text and the countdown.
    private int gameOverSeconds;
    private int gameOverMinutes;
    private int secondsGbl; //A global version of the local variable called seconds.
    private int minutesGbl;
    private int pointsGained = 0;

    string data;

    StreamWriter file;

    string FileName;

	// Use this for initialization
    void Awake()
    {
        menuKickEminent = false;
    }

	void Start () {
        FileName = DateTime.Now.ToString("MM_dd_HH_mm_ss");
        FileName += ".txt";
	}
	
	// Update is called once per frame
	void Update () {
        if (menuKickEminent)
        {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;
            int minutes = (int)Mathf.Floor(timer / 60);
            int seconds = (int)timer % 60;
            minutesGbl = minutes;
            secondsGbl = seconds;
            if (seconds <= 0)
            {
                Application.LoadLevel("Menu01");
            }
        }

        gameOverTimer -= Time.deltaTime;
        if (gameOverTimer < 0) gameOverTimer = 0;
        gameOverMinutes = (int)Mathf.Floor(gameOverTimer / 60);
        gameOverSeconds = (int)gameOverTimer % 60;
        if (gameOverTimer <= 0)
        {
            menuKickEminent = true;
        }

        
	}
    
    void addPoints(int pointsToAdd)
    {
        data += gameOverTimer + " " + pointsToAdd + "\n";
        Debug.Log("Point added");
        if( Application.platform != RuntimePlatform.OSXWebPlayer || Application.platform != RuntimePlatform.WindowsWebPlayer)
            using (file = new StreamWriter(FileName, true))
                file.WriteLine(gameOverTimer + " " + pointsToAdd + "\n");
            
        pointsGained += pointsToAdd;
        
    }  

    string buildString(int minutes, int seconds)
    {
        if(minutes > 0)
        {
            return ("Game over in " + minutes) + (" minute and " + seconds + " seconds");
        }
        else
            return ("Game over in ") + (seconds + " seconds");
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;
        GUI.Label(new Rect(Screen.width / 15 + 90, Screen.height / 6, 200, 100), "Points: " + pointsGained);
        GUI.Label(new Rect(Screen.width / 15, Screen.height / 6 + 100, 400, 100), buildString(gameOverMinutes, gameOverSeconds));

        if (menuKickEminent)
        {
            TextEditor te = new TextEditor();
            te.content = new GUIContent(data);
            te.SelectAll();
            te.Copy();
            //GUI Stuff.
            GUI.skin = guiSkin;
            GUI.BeginGroup(new Rect(Screen.width / 2 - bootscreenBG.width / 2, Screen.height / 2 - bootscreenBG.height / 2, bootscreenBG.width, bootscreenBG.height));
            GUI.Box(new Rect(0, 0, bootscreenBG.width, bootscreenBG.height), bootscreenBG);
            GUI.Label(new Rect(bootscreenBG.width / 2 - 100, bootscreenBG.height / 3 - 40, 200, 100), pointsGained.ToString() + " points gained!");
            //GUI.Label(new Rect(bootscreenBG.width / 2 - 100, (bootscreenBG.height / 3) * 2 - 40, 200, 100), "Time to get first points: " + firstPointGained + " seconds");
            GUI.EndGroup();
            //Debug.Break();
        }
    }
}
