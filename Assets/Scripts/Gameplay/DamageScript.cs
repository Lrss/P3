using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

    private GameObject playerObj;
    private GameObject archerObj;
    private string objHit;
    private string playerColour;
    private GameObject mainCollider;
	// Use this for initialization
	void Start () {

        

        if(gameObject.name == "HeadColliderGreen" || gameObject.name == "LegColliderGreen")
        {
            playerColour = "Green";
            playerObj = GameObject.Find("Player1");
            archerObj = GameObject.Find("Player1/Archer_Green_120frameShot/Archer");
            
        }
        else
        {
            playerColour = "Red";
            playerObj = GameObject.Find("Player2");
            archerObj = GameObject.Find("Player2/Archer_Red_120frameShot/Archer");
        }

        mainCollider = GameObject.Find("TorsoCollider" + playerColour);
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.name == "LegCollider" + playerColour)
            objHit = "Leg";

        if (gameObject.name == "HeadCollider" + playerColour)
            objHit = "Head";
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "arrowRed" || other.gameObject.tag == "arrowGreen")
        {
            mainCollider.SendMessage("damageTaken", objHit);
            archerObj.SendMessage("Hit");
            //Destroy(other.gameObject);
        }
    }
}
