using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    //Script that handles the health of the archer and the damage it takes. It also shows the healthbar and attaches a name label.

    private float healthArcher = 1f; // This is equal to 100% of the archer.
    private GameObject archerObj;
    private float headDmg = 0.3f;
    private float legDmg = 0.1f;
    private float torsoDmg = 0.2f; //How much damage, in % of total health, the archer takes from each arrow received to the face. 
    public Transform towerPos; //Used to obtain which player the health belongs to.
    Vector2 posArcher; //The position of the healthbar
    public GUIStyle progress_empty, progress_full; //Two different GUIStyles, one for a empty healthbar, and one for a full.
    float barDisplayArcher = 0; //Variable to keep track of how much of healthbar that should be full at a given time.
    private GameObject gameManager; //Contains a reference to the gamemanager which handles win/loss
    
    Vector2 sizeArcher = new Vector2(100, 20); //The size of the healthbar
    public Texture2D progressBarEmpty; //The texture for the empty healthbar
    public Texture2D progressBarFull; //The texture for the full healthbar

    void Start()
    {
        gameManager = GameObject.Find("GameManager"); //Finds and attaches the gamemanager
        if(towerPos.gameObject.name == "Player1")
        {
            archerObj = GameObject.Find("Player1/Archer_Green_120frameShot/Archer");
        }
        else
        {
            archerObj = GameObject.Find("Player2/Archer_Red_120frameShot/Archer");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        barDisplayArcher = healthArcher; //The display variable gets updated to contain the current amount of health
        if(healthArcher <= 0f)
        {
            loseGame(towerPos.gameObject.name); //If the health of the archer reaches 0the losegame function is started.
        }
    }

    void OnTriggerEnter2D(Collider2D other) //Checks if it has collided with an arrow
    {
        if (towerPos.gameObject.name == "Player2")
        {
            if (other.gameObject.tag == "arrowGreen")
            {
                archerObj.SendMessage("Hit");
                //Destroy(other.gameObject); //Destroys the arrow on contact
                healthArcher = healthArcher - torsoDmg; //Damages the archer by subtracting the predefined amount of health.
            }

        }
        else
        {
            if (other.gameObject.tag == "arrowRed")
            {
                archerObj.SendMessage("Hit");
                //Destroy(other.gameObject); //Destroys the arrow on contact
                healthArcher = healthArcher - torsoDmg; //Damages the archer by subtracting the predefined amount of health.
            }
        }
    }

    void OnGUI()
    {
        //Entire block is the Archer healthbar.
        if (towerPos.gameObject.name == "Player1")
            posArcher = new Vector2(25, Screen.height - 420);
        else
            posArcher = new Vector2(Screen.width - 125, Screen.height - 420);

        //draw the background:
        GUI.BeginGroup(new Rect(posArcher.x, posArcher.y, sizeArcher.x, sizeArcher.y));
            
            GUI.Box(new Rect(0, 0, sizeArcher.x, sizeArcher.y), progressBarEmpty, progress_empty);

            //draw the filled-in part:
            GUI.BeginGroup(new Rect(0, 0, sizeArcher.x * barDisplayArcher, sizeArcher.y));
                GUI.Box(new Rect(0, 0, sizeArcher.x, sizeArcher.y), progressBarFull, progress_full);
            GUI.EndGroup();
        GUI.EndGroup();

        GUI.Label(new Rect(posArcher.x + 30, posArcher.y - 20, 100, 20), "Archer");
    }

    private void loseGame(string loser)
    {
        //Sends a message to the gamemanager that this player has lost the game.
        gameManager.SendMessage("loseGame", loser);
    }

    private void damageTaken(string objHit)
    {
        if(objHit == "Head")
        {
            healthArcher = healthArcher - headDmg;
        }
        if(objHit == "Leg")
        {
            healthArcher = healthArcher - legDmg;
        }
    }
}
