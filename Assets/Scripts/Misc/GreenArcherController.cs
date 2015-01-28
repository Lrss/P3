using UnityEngine;
using System.Collections;

public class GreenArcherController : MonoBehaviour {

    Animator animator;

    public static bool idle;
    public static bool readyToFire;
    public static bool releasingArrow;
    public static bool drawingBow;




	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
        animator.speed = 2;
	}
	
	// Update is called once per frame
	void Update () {
	    if(animator)
        {
            if(idle)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Ready to fire", false);
                animator.SetBool("Releasing Arrow", false);
                animator.SetBool("Drawing Bow", false);
            }
            if(readyToFire)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Ready to fire", true);
                animator.SetBool("Releasing Arrow", false);
                animator.SetBool("Drawing Bow", false);
            }
            if(releasingArrow)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Ready to fire", false);
                animator.SetBool("Releasing Arrow", true);
                animator.SetBool("Drawing Bow", false);
            }
            if(drawingBow)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Ready to fire", false);
                animator.SetBool("Releasing Arrow", false);
                animator.SetBool("Drawing Bow", true);
            }

            
        }
	}
}
