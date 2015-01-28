using UnityEngine;
using System.Collections;

public class GroundMovement : MonoBehaviour {
    
    //This script handles the movement of the background by applying a translate to each of them separately and then moving them back in the stack when they get out of vision.
    
    float movementSpeed = -4f;

	// Update is called once per frame
	void FixedUpdate () {
        if (transform.localPosition.x <= -48.75977)
            transform.Translate(Vector3.right * 243.09237f);

        transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        Destroy(other.gameObject);
    }
}
