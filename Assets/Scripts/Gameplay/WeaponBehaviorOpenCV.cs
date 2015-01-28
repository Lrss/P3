using UnityEngine;
using System;
using System.Collections;


public class WeaponBehaviorOpenCV : MonoBehaviour
{

    //This script handles both the angle and power calculations based on the position of the cursors. It also handles firing the arrow by sending a message to the arrow about the applied force.

    //public float openCVAngle;
    //public float openCVForce;
    public bool fire;
    public int playerNr;
    GameObject arrow;
    GameObject player1Object;
    //GameObject player1Cursor;
    GameObject player2Object;
    //GameObject player2Cursor;
    private float timerRemaining = 3.7f;
    private float timerMax = 3.7f;

    float player1Angle;
    float player2Angle;
    float player1Power;
    float player2Power;
    


    // Use this for initialization
    void Start()
    {
        //openCVAngle = 5.0F;
        //openCVForce = 10.0F;
        playerNr = Convert.ToInt32(gameObject.transform.parent.gameObject.name.Substring(6, 1));
        //player1Cursor = GameObject.Find("Player1Cursor");
        //player2Cursor = GameObject.Find("Player2Cursor");
        player1Object = GameObject.Find("Player1");
        player2Object = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        
		

		if (Listener.shooting[0] && playerNr == 1 || Listener.shooting[1] && playerNr == 2)
			fire = true;
		else
			fire = false;

       /* if (Input.GetButton("P" + playerNr + "Fire"))
            fire = true;
        else
            fire = false;*/
		player1Angle = Listener.angle [0];
		player2Angle = Listener.angle [1];
		player1Power = Listener.force [0];
		player2Power = Listener.force [1];

        timerRemaining -= Time.deltaTime;
        
        if (timerRemaining < 0 && fire == true)
        {
            GreenArcherController.idle = false;
            GreenArcherController.readyToFire = false;
            GreenArcherController.releasingArrow = true;
            GreenArcherController.drawingBow = false;
            GreenBowController.idle = false;
            GreenBowController.readyToFire = false;
            GreenBowController.releasingArrow = true;
            GreenBowController.drawingBow = false;
        }
        else if (timerRemaining < 0)
        {
            GreenArcherController.idle = false;
            GreenArcherController.readyToFire = true;
            GreenArcherController.releasingArrow = false;
            GreenArcherController.drawingBow = false;
            GreenBowController.idle = false;
            GreenBowController.readyToFire = true;
            GreenBowController.releasingArrow = false;
            GreenBowController.drawingBow = false;
        }
        else if (timerRemaining < 3.5f)
        {
            GreenArcherController.idle = false;
            GreenArcherController.readyToFire = false;
            GreenArcherController.releasingArrow = false;
            GreenArcherController.drawingBow = true;
            GreenBowController.idle = false;
            GreenBowController.readyToFire = false;
            GreenBowController.releasingArrow = false;
            GreenBowController.drawingBow = true;
        }
        else if (timerRemaining > 3.5f)
        {
            GreenArcherController.idle = true;
            GreenArcherController.readyToFire = false;
            GreenArcherController.releasingArrow = false;
            GreenArcherController.drawingBow = false;
            GreenBowController.idle = true;
            GreenBowController.readyToFire = false;
            GreenBowController.releasingArrow = false;
            GreenBowController.drawingBow = false;
        }
                   
        //Fires the arrow if it receives the message to do so
        if (fire == true && timerRemaining < 0)
        {
            //Angle gets changed if the the cursor is at the same position as the player as it would cause weird angles.
            clampAngle(player1Angle);
            //The arrow gets instantiated, gets assigned a position, and an angle based on the previous calculations.
            arrow = (GameObject)Instantiate(Resources.Load("Prefab/Arrow_GreenOpenCV"), transform.position, Quaternion.Euler(0, 0, player1Angle));
            //The calculated power gets sent to the arrow.
            arrow.SendMessage("AddForce", player1Power);
            timerRemaining = timerMax;
        }

    }

    float clampAngle(float playerAngle)
    {
        
        if (playerAngle < 10)
            playerAngle = 10;

        if (playerAngle > 90)
            playerAngle = 90;

        return (playerAngle);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 75, 75), player1Angle.ToString());
        GUI.Label(new Rect(Screen.width - 100, 0, 75, 75), player2Angle.ToString());
        GUI.Label(new Rect(0, 100, 75, 75), player1Power.ToString());
        GUI.Label(new Rect(Screen.width - 100, 100, 75, 75), player2Power.ToString());
    }
}