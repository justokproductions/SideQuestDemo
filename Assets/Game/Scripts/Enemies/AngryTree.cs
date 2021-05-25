using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//child class of Enemy.  Most of the work is done there.
public class AngryTree : Enemy
{



    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();    //since this enemy reacts when hero is close enough it runs the check distance command.
    }




}
