using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    //Controls handles the control of the cursor on the screen when playing with a keyboard.
    //It is applied directly to the cursor object in the scene.

    private string inputPrefix; //The name of the player the cursor belongs to.
    public float speed; //The speed the cursor moves at
    private GameObject playerObject; //The gameobject the cursor is anchored to
    private WeaponBehavior weaponBehaviorScript; //The script that handles firing of the arrow gets importet
    public static Vector3 initialPosition;
    Listener OpenCVData;
    GameObject GameManager;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        if (gameObject.name == "Player1Cursor") //Checks the name of the player the cursor belongs to
        {
            inputPrefix = "1";
            playerObject = GameObject.Find("Player1");
        }
        GameManager = GameObject.FindWithTag("GameController");
        OpenCVData = playerObject.GetComponentInChildren<Listener>();
        speed = 10f;
        weaponBehaviorScript = playerObject.GetComponentInChildren<WeaponBehavior>();
	}

	
	// Update is called once per frame
	void Update ()
    {

        //Horizontal and vertical translation input is received directly via the input manager. It is then multiplied by the speed variable and then by Time.deltaTime which ensures the movement is framerate independent
        float horizontalTranslation = Input.GetAxis("P" + inputPrefix + "Horizontal") * speed * Time.deltaTime;
        float verticalTranslation = Input.GetAxis("P" + inputPrefix + "Vertical") * speed * Time.deltaTime;

        //The input is then applied to the cursor.

        transform.Translate(horizontalTranslation, 0, 0);
        transform.Translate(0, verticalTranslation, 0);

        if (inputPrefix == "1")
        {
            //Constrains the cursor to move in an area of relevance to player 1
            if (transform.position.x < playerObject.transform.position.x)
                transform.position = new Vector3(playerObject.transform.position.x, transform.position.y, transform.position.z);

            if (transform.position.y < playerObject.transform.position.y)
                transform.position = new Vector3(transform.position.x, playerObject.transform.position.y, transform.position.z);

            if (transform.position.x > 0)
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            if (transform.position.y > 14)
                transform.position = new Vector3(transform.position.x, 14, transform.position.z);
        }
        else
        {
            //Constrains the cursor to move in an area of relevance to player 2
            if (transform.position.x > playerObject.transform.position.x)
                transform.position = new Vector3(playerObject.transform.position.x, transform.position.y, transform.position.z);

            if (transform.position.y < playerObject.transform.position.y)
                transform.position = new Vector3(transform.position.x, playerObject.transform.position.y, transform.position.z);

            if (transform.position.x < 0)
                transform.position = new Vector3(0f, transform.position.y, transform.position.z);
            if (transform.position.y > 14)
                transform.position = new Vector3(transform.position.x, 14, transform.position.z);
        }


        //if (OpenCVData.shooting[(int)inputPrefix[0]] == true)
        //    weaponBehaviorScript.fire = true;

        if (Input.GetButton("P" + inputPrefix + "Fire"))
        {
            weaponBehaviorScript.fire = true;
        }
        else
            weaponBehaviorScript.fire = false;
	}

    public static void ResetPos()
    {
        GameObject.Find("Player1Cursor").transform.position = initialPosition;
    }
}
