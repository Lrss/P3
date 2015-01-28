using UnityEngine;
using System.Collections;

public class ArcherColorControllerScript : MonoBehaviour {

    private float timer;
    private float timerMax;
    private bool gotHit;
	// Use this for initialization
	void Start () {
        timer = 0.2f;
        timerMax = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
        

        if(gotHit && timer  > 0)
        {
            gameObject.renderer.material.color = Color.red;
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.renderer.material.color = Color.white;
        }

    }

    void Hit()
    {
        gotHit = true;
        timer = timerMax;
    }
       
}
