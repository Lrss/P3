using UnityEngine;
using System.Collections;

public class OpenCVControls : MonoBehaviour {

    private string inputPrefix; //Used to assign controls to the correct player
    public float speed; //Determines the movement speed of the cursor
    public GameObject playerObject; //Contains the object of the player being controlled
    //public WeaponBehavior weaponBehaviorScript;
    private Vector2 blue_coord_p1 = new Vector2(0,0);  //Power determiner for player 1
    private Vector2 blue_coord_p2 = new Vector2(0, 0);  //Power determiner for player 2
    private Vector2 green_coord = new Vector2(0, 0);    //Angle determiner for player 1
    private Vector2 red_coord = new Vector2(0, 0);      //Angle determiner for player 2
    private Vector3 player1Aim; //Vector containing angle and power for player 1;
    private Vector3 player2Aim; //Vector containing angle and power for player 2;
    
	// Use this for initialization
	void Start () {
        player1Aim = green_coord - blue_coord_p1;
        player2Aim = red_coord - blue_coord_p2;
        if (gameObject.name == "Player1Cursor")
        {
            inputPrefix = "P1";
            playerObject = GameObject.Find("Player1");
        }
        else
        {
            inputPrefix = "P2";
            playerObject = GameObject.Find("Player2");
        }
        speed = 10f;
        //weaponBehaviorScript = playerObject.GetComponentInChildren<WeaponBehavior>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        

        if (inputPrefix == "P1")
        {
            transform.position = player1Aim + playerObject.transform.position; //Controls the position of the cursor based on the power vector calculated based on the given OpenCV coordinates

            //Out of bounds movement check.
            if (transform.position.x < playerObject.transform.position.x)
                transform.position = new Vector3(playerObject.transform.position.x, transform.position.y, transform.position.z);
            if (transform.position.y < playerObject.transform.position.y)
                transform.position = new Vector3(transform.position.x, playerObject.transform.position.y, transform.position.z);
            if (transform.position.x > 24)
                transform.position = new Vector3(24, transform.position.y, transform.position.z);
            if (transform.position.y > 14)
                transform.position = new Vector3(transform.position.x, 14, transform.position.z);
        }
        else
        {
            transform.position = player2Aim; //Controls the position of the cursor based on the power vector calculated based on the given OpenCV coordinates

            //Out of bounds movement check
            if (transform.position.x > playerObject.transform.position.x)
                transform.position = new Vector3(playerObject.transform.position.x, transform.position.y, transform.position.z);
            if (transform.position.y < playerObject.transform.position.y)
                transform.position = new Vector3(transform.position.x, playerObject.transform.position.y, transform.position.z);
            if (transform.position.x < -23.5)
                transform.position = new Vector3(-23.5f, transform.position.y, transform.position.z);
            if (transform.position.y > 14)
                transform.position = new Vector3(transform.position.x, 14, transform.position.z);
        }

        

        /*if (Input.GetButtonDown(inputPrefix + "Fire"))
        {
            weaponBehaviorScript.fire = true;
        }
        else
            weaponBehaviorScript.fire = false;*/
	}
}
