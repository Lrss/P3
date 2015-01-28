using UnityEngine;
using System.Collections;

public class GameUnderscaling : MonoBehaviour {

	public float scaleSpeed;
    public GameObject Player1;
    public GameObject Player2;

	// Use this for initialization
	void Start () {
		camera.orthographic = true;
        camera.orthographicSize = 5;

	}

    void Update() 
    {
        
    }
	// Update is called once per frame
	void FixedUpdate () {
        Player1.transform.Translate(new Vector2(1.6f, 0f) * scaleSpeed * Time.deltaTime);
        Player2.transform.Translate(new Vector2(-1.6f, 0f) * scaleSpeed * Time.deltaTime);

        transform.Translate(new Vector2(0, -1) * scaleSpeed * Time.deltaTime);
        camera.orthographicSize -= scaleSpeed * Time.deltaTime;
	}
}
