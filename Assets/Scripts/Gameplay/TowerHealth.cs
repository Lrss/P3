using UnityEngine;
using System.Collections;

public class TowerHealth : MonoBehaviour
{
    //Almost identical to the PlayerHealth.cs. Check for detailed commenting.
    private float healthTower = 1f;
    private float towerDmg = 0.05f;
    public Transform towerPos;
    Vector2 posTower;
    public GUIStyle progress_empty, progress_full;
    float barDisplayTower = 0;
    public int positionModifier = 0;
    private GameObject gameManager;

    Vector2 sizeTower = new Vector2(100, 20);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        barDisplayTower = healthTower;
        if (healthTower <= 0f)
        {
            loseGame(towerPos.gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (towerPos.gameObject.name == "Player2")
        {
            if (other.gameObject.tag == "arrowGreen")
            {
                //Destroy(other.gameObject); //Destroys the arrow on contact
                healthTower = healthTower - towerDmg; //Damages the archer by subtracting the predefined amount of health.
            }

        }
        else
        {
            if (other.gameObject.tag == "arrowRed")
            {
                //Destroy(other.gameObject); //Destroys the arrow on contact
                healthTower = healthTower - towerDmg; //Damages the archer by subtracting the predefined amount of health.
            }
        }
    }

    void OnGUI()
    {
        //Entire block is the player healthbar
        if (towerPos.gameObject.name == "Player1")
            posTower = new Vector2(25, Screen.height - 300);
        else
            posTower = new Vector2(Screen.width - 125, Screen.height - 300);
        //draw the background:
        GUI.BeginGroup(new Rect(posTower.x, posTower.y, sizeTower.x, sizeTower.y));
        GUI.Box(new Rect(0, 0, sizeTower.x, sizeTower.y), progressBarEmpty, progress_empty);
        //draw the filled-in part:
        GUI.BeginGroup(new Rect(0, 0, sizeTower.x * barDisplayTower, sizeTower.y));
        GUI.Box(new Rect(0, 0, sizeTower.x, sizeTower.y), progressBarFull, progress_full);
        GUI.EndGroup();
        GUI.EndGroup();
        GUI.Label(new Rect(posTower.x + 30, posTower.y - 20, 100, 20), "Tower");
    }

    private void loseGame(string loser)
    {
        gameManager.SendMessage("loseGame", loser);
    }
}
