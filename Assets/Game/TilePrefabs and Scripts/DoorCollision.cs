using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCollision : MonoBehaviour
{

    public string target;           //target screen.
    public int targetNumber;        //target spawn point.

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            other.gameObject.GetComponent<PlayerMovement>().SetStartingPosition(targetNumber);          //sets the spawn point on the player so he shows up at the right location.
            SceneManager.LoadScene(target, LoadSceneMode.Single);                                       //loads the new screen.
        }
    }
}
