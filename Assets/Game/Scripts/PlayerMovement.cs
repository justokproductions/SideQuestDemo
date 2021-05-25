using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//an enum that describes the players various states for interactivity
public enum PlayerState
{
    idle,           //not doing anything
    walk,           //moving by player controls
    attack,         //attacking
    stagger,        //is being moved/knocked back by enemies or traps
    interact        //interacting with objects or villagers.
}

public class PlayerMovement : MonoBehaviour
{
    public List<SpawnPointScript> spawnPoints;   //list of potential spawn objects
    public PlayerState currentState;            //sets current state
    public float moveSpeed = 5f;                //sets player speed

    public Rigidbody2D myRigidbody;             //rigid body
    public Animator animator;                   //animator

    public static int spawnPoint = 0;           //which location does the player spawn at when moving to a new zone.

    Vector2 movement;

    bool isInteract = false;                    //This is affected by other objects, but tells the player if he is in front of an interactable location.

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();  //sets the rigidbody
        MoveToStartingPosition();                   //sets the player to the proper spawn point
        currentState = PlayerState.walk;            //sets movement to walk

        //gives player a starting direction to face
        animator.SetFloat("Horizontal", 0);         
        animator.SetFloat("Vertical", -1);              }


    //switches inbetween interacting with objects and ending interaction.
    public void ToggleInteract()
    {
        if (currentState == PlayerState.interact)   //if they are interacting, switch back to walk
            currentState = PlayerState.walk;
        else                                        //may eventually have states that cannot trigger interact
            currentState = PlayerState.interact;    //if they are doing anything else, switch to interacting
    }

    // Update is called once per frame
    void Update()
    {
        //converts the direction input into a variable
        movement.x = Input.GetAxisRaw("Horizontal");    
        movement.y = Input.GetAxisRaw("Vertical");

        //checks if player is ready to interact or attack
        if (Input.GetButtonDown("Fire1") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if (isInteract)                     //if they are in an interaction zone
                ToggleInteract();               //interact with it.
            else                                //otherwise
                StartCoroutine(AttackCo());     //start attack
        }
        //otherwise if player is able to walk
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();           //walk
            currentState = PlayerState.walk;    //make sure state is walking state
        }
    }

    //attack courotine.
    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);        //set the animator to attacking
        currentState = PlayerState.attack;          //set the state to attacking
        yield return null;
        animator.SetBool("Attacking", false);       //switch animator off attacking
        yield return new WaitForSeconds(.33f);      //waits for it
        currentState = PlayerState.walk;            //set state to walk
    }

    //movement commands
    void UpdateAnimationAndMove()
    {
        animator.SetFloat("Speed", movement.sqrMagnitude);  //sets animation based off if customer is moving or not.

        //assuming movement is not 0
        if (movement.sqrMagnitude != 0)
        {
            //updates animation based on movement directions
            animator.SetFloat("Horizontal", movement.x); 
            animator.SetFloat("Vertical", movement.y);
        }

        //flips facing based on left or right movement.
        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //determines the movement speed and moves the character appropriately.
        movement.Normalize();
        myRigidbody.MovePosition(myRigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

    //triggers knockback Coroutine
    public void Knockback(float knockBackTime)
    {
        StartCoroutine(KnockbackCo(knockBackTime));
    }

    //resolves knockback motion
    private IEnumerator KnockbackCo(float knockBackTime)
    {
            yield return new WaitForSeconds(knockBackTime);                                 //wait the determined time.
            myRigidbody.velocity = Vector2.zero;                                            //stops the knockback movement
            myRigidbody.GetComponent<PlayerMovement>().currentState = PlayerState.idle;     //switches player back to free movement
    }

    //sets the starting point based on where they left the previous screen
    public void SetStartingPosition(int i)
    {
        spawnPoint = i;     //updates starting point
    }


    //when spawning, detect the starting location and move there.
    void MoveToStartingPosition()
    {
            transform.position = spawnPoints[spawnPoint].transform.position;      //move the player to the matching spawn point.
    }

    //checks when entering/exiting interaction zones
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("interact"))
        {
            isInteract = true;                  //marks entering an interaction zone
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("interact"))
        {
            isInteract = false;                 //marks exiting an interaction zone
        }
    }


}
