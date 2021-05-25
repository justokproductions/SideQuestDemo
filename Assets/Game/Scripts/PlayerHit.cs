using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the programming for the hit box that allows the player to hit targets
public class PlayerHit : MonoBehaviour
{
    public float thrust;        //how much force the player pushes a target back with
    public GameObject player;   //the player
    public float knockbackTime; //how long is the target bieng knocked back
    public int damage;          //how much damage the attack does

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");  //sets the player variable
    }

  

    private void OnTriggerEnter2D(Collider2D other)
    {
        //code for breaking objects
        if(other.CompareTag("breakable"))
        {
            other.GetComponent<Pot>().Smash();  //calls on the object's smash code
        }

        //code for attacking enemies
        if(other.CompareTag("enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();      //get the target rigid body
            if (enemy != null)                                          //confirm the enemy is real
            {
                Vector2 difference = enemy.transform.position - player.transform.position;  //calculate the vector direction
                difference = difference.normalized * thrust;                                //set the thrust value as the vector
                enemy.AddForce(difference, ForceMode2D.Impulse);                            //add force
                enemy.GetComponent<Enemy>().TakeDamage(damage, knockbackTime);              //deal damage
                enemy.GetComponent<Enemy>().currentState = EnemyState.stagger;              //set enemy to stagger state
            }
        }
    }

}
