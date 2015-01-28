using UnityEngine;
using System.Collections;

public class GreenBowController : MonoBehaviour {

    Animator animator;

    public static bool idle;
    public static bool readyToFire;
    public static bool releasingArrow;
    public static bool drawingBow;

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.speed = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator)
        {
            if (idle)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("ReadyToFire", false);
                animator.SetBool("ReleasingArrow", false);
                animator.SetBool("DrawingBow", false);
            }
            if (readyToFire)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("ReadyToFire", true);
                animator.SetBool("ReleasingArrow", false);
                animator.SetBool("DrawingBow", false);
            }
            if (releasingArrow)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("ReadyToFire", false);
                animator.SetBool("ReleasingArrow", true);
                animator.SetBool("DrawingBow", false);
            }
            if (drawingBow)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("ReadyToFire", false);
                animator.SetBool("ReleasingArrow", false);
                animator.SetBool("DrawingBow", true);
            }


        }
    }
}
