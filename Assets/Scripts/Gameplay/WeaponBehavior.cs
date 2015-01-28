using UnityEngine;
using System;
using System.Collections;


public class WeaponBehavior : MonoBehaviour
{

    //This script handles both the angle and power calculations based on the position of the cursors. It also handles firing the arrow by sending a message to the arrow about the applied force.

    //public float openCVAngle;
    //public float openCVForce;
    public bool fire;
    GameObject arrow;
    GameObject playerObject;
    GameObject playerCursor;
    public GameObject playerArcher;
    private float timerRemaining = 3.7f;
    private float timerMax = 3.7f;

    private float playerAngle;
    private float playerPower;



    // Use this for initialization
    void Start()
    {
        playerCursor = GameObject.Find("Player1Cursor");
        playerObject = GameObject.Find("Player1/Archer_Green_120frameShot");
        playerArcher = GameObject.Find("Player1/Archer_GREEN");
    }

    // Update is called once per frame
    void Update()
    {
        //Uses pythagoras and trigonometry to calculate the power and angle of the arrow.
        //Player 1 Calcs
        float player1DeltaX = playerCursor.transform.position.y - playerObject.transform.position.y;
        float player1DeltaY = playerCursor.transform.position.x - playerObject.transform.position.x;
        playerAngle = Mathf.Atan2(player1DeltaY, player1DeltaX) * 180 / Mathf.PI;
        playerPower = Mathf.Sqrt(Mathf.Pow(player1DeltaX, 2) + Mathf.Pow(player1DeltaY, 2));

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
        else if (timerRemaining < 3.5)
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
        else if (timerRemaining > 3.5)
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
            clampAngle(playerAngle);
            arrow = (GameObject)Instantiate(Resources.Load("Prefab/Arrow_Green"), transform.position, Quaternion.Euler(0, 0, 90 - playerAngle)); //The arrow gets instantiated, gets assigned a position, and an angle based on the previous calculations.
            arrow.SendMessage("AddForce", playerPower);
            Controls.ResetPos();
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

    /*void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 75, 75), playerAngle.ToString());
        GUI.Label(new Rect(0, 100, 75, 75), playerPower.ToString());
    }*/
}