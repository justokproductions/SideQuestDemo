using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent script for all enemies

//enum for activities
public enum EnemyState
{
    idle,       //not doing anything
    walk,       //moving
    attack,     //attacking
    stagger     //reeling from a hit or otherwise stunned.
}

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D myRigidbody;      //rigid body
    public Animator anim;                   //animator

    public string enemyName;                //name
    public EnemyState currentState;         //it's current activity
    public int health;                      //it's remaining hit points
    public int baseAttack;                  //damage from it's basic attack
    public float moveSpeed;                 //how fast is it

    public float thrust;                    //how much knockback does it do
    public float knockbackTime;             //how long is it's target stunned

    public Transform target;                //the player
    public float chaseRadius;               //if it has a reaction, how far does it look
    public float AttackRadius;              //how close does it need to get to attack you.
    public Transform homePosition;          //if used, where does it return to



    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentState = EnemyState.idle;                         //starts off doing nothing

        target = GameObject.FindWithTag("Player").transform;    //sets the player as it's target
        myRigidbody = GetComponent<Rigidbody2D>();              //sets the rigid body
        anim = GetComponent<Animator>();                        //sets the animator
    }


    //checks for animations
    private void ChangeAnim(Vector2 direction)
    {
        //determines which direction it is moving in and passes it to the animator.
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);

    }

    //changes activity state
    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)   //checks if it's already that state first
            currentState = newState;
    }

    //checks how far away from the hero the enemy is and follows him if close enough
    protected void CheckDistance()
    {
        //checks if the hero is in between the attack and chase radius.
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > AttackRadius)
        {
            // also checks if the enemy is in the right state to attack
            if (currentState == EnemyState.idle || currentState == EnemyState.walk )
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);    //calculates the direction he should be moving
                ChangeAnim(temp - transform.position);                                                                  //passes this data to the ChangeAnim for animation
                myRigidbody.MovePosition(temp);                                                                         //moves that direction
                ChangeState(EnemyState.walk);                                                                           //switches to walk
                anim.SetBool("asleep", false);                                                                          //tells animation to wake up if necessary
            }
        }
        //it the hero is outside the chase radius, goes back to sleep
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            anim.SetBool("asleep", true);           //switches animation to sleep
            ChangeState(EnemyState.idle);           //sets action back to idle.
        }
    }

    //calculates damage and stun 
    public void TakeDamage(int damage, float knockBackTime) //damage and knockBackTime are passed in from the hero or triggering effect
    {
        //cannot be damaged if currently stunned.
        if (currentState != EnemyState.stagger)
        {
            ChangeState(EnemyState.stagger);            //switches to stagger
            health -= damage;                           //loses health equal to the incoming damage.
            //checks for death
            if (health <= 0)
            {
                OnDeath();                              //calls death sequence
            }
            StartCoroutine(KnockbackCo(knockBackTime)); //calls on the coroutine for knockback
        }

    }

    //creates the stun period
    private IEnumerator KnockbackCo(float knockBackTime)
    {
            yield return new WaitForSeconds(knockBackTime);                     //waits until knockBackTime is complete
            myRigidbody.velocity = Vector2.zero;                                //sets velocity back to zero so he stops sliding backwards
            myRigidbody.GetComponent<Enemy>().currentState = EnemyState.idle;   //returns to idle
    }

    //creates death sequence. Children will likely have more complex versions.
    private void OnDeath()
    {
        Debug.Log("Urk! Bleh!");            //debug test
        Destroy(this.gameObject);           //destroys object
    }

    //if the monster hits the hero, deals damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))                                                        //confirms it's the hero
        {
            Rigidbody2D player = other.GetComponent<Rigidbody2D>();                             //sets the target values
            if (player != null)
            {
                Vector2 difference = player.transform.position - transform.position;            //calculates direction of knockback
                difference = difference.normalized * thrust;                                    //multiplies it by force of knockback
                player.AddForce(difference, ForceMode2D.Impulse);                               //applies calculated force.
                player.GetComponent<PlayerMovement>().Knockback(knockbackTime);                 //calls players knockback subroutine.
                player.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;       //sets player to staggered.
            }
        }
    }

}
