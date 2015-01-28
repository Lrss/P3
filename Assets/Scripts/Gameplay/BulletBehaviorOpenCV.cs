using UnityEngine;
using System.Collections;

public class BulletBehaviorOpenCV : MonoBehaviour {

    //Script is applied to the arrow prefab fired by the archers.
    //It handles how the arrow behaves when it flies, such as power and rotation.

    float FirePower = 600;
    float angle;
    private string ringHit;
    private GameObject gameManager;
    private bool alreadyHit = false;

	// Use this for initialization
	void Start () 
    {
        //bullsEyePos = GameObject.Find("Target/Target/TargetMesh/Mesh1").transform;
        gameManager = GameObject.Find("GameManager");
	}

    void Update ()
    {
        Vector3 dir = rigidbody.velocity; // The current velocity of the arrow gets stored.
        if (dir != Vector3.zero) //If the current velocity is not movíng
        {
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; //Calculate how the arrow needs to rotate to simulate gravity.
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //Apply the calculated spin
        }

        if(gameObject.transform.position.y < -12.0f) //If the arrow falls out of the screen it gets destroyed.
            GameObject.Destroy(gameObject);    
    }

    
    void AddForce(float shotForce) //Function gets called from the WeaponBehavior script.
    {
        gameObject.transform.Rotate(new Vector3(0,0,0)); //Projectile gets added a rotation
        
        if (shotForce < 3)
            shotForce = 3;



        FirePower = shotForce * 4;
        rigidbody.AddForce(gameObject.transform.right * FirePower); //The calculated force gets applied to the arrow.
    }

    void OnTriggerEnter(Collider col)
    {
        gameObject.audio.Play();
        rigidbody.isKinematic = true;
        transform.parent = col.transform;
        gameObject.collider.enabled = false;
        //gameObject.transform.Translate(Vector3.right * 0.5f);
        if (alreadyHit == false)
        {
            ringHit = col.tag;
            broadcastRingHit(ringHit);
        }
        alreadyHit = true;
    }

    void broadcastRingHit(string ringHit)
    {
        Debug.Log(ringHit);
        if(ringHit == "ring_1")
            gameManager.SendMessage("addPoints", 1);

        if (ringHit == "ring_2")
            gameManager.SendMessage("addPoints", 2);

        if (ringHit == "ring_3")
            gameManager.SendMessage("addPoints", 3);

        if (ringHit == "ring_4")
            gameManager.SendMessage("addPoints", 4);

        if (ringHit == "ring_5")
            gameManager.SendMessage("addPoints", 5);
    }
}
