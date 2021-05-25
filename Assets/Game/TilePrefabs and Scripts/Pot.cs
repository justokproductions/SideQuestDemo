using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for destructables.
public class Pot : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();        //sets the animator for th epot.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Smash()
    {
        anim.SetBool("isBroken", true);         //begins break animation.
        Debug.Log("Smashy smashy!");
    }
}
