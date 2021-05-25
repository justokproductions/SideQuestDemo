using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour
{

    public GameObject dialogBox;            //pointer to the dialgoue box
    public Text dialogText;                 //translates string to text
    public string dialog;                   //string for the display.
    public bool playerInRange;              //checks if player is in range to interact

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the player hit the interact button and is in range.
        if (Input.GetButtonDown("Fire1") && playerInRange)      
        {
            //if the dialog box is already open, close it.
            if (dialogBox.activeInHierarchy)                    
            {
                dialogBox.SetActive(false);
            }
            //otherwise, close the box.
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
    }

    //checks if the object entering the zone is the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if it is, turn the player check is on
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    //checks if the player is leaving the zone
    private void OnTriggerExit2D(Collider2D other)
    {
        //checks if it's th eplayer
        if (other.CompareTag("Player"))
        {
            playerInRange = false;      //turns the player check off
            dialogBox.SetActive(false); //if the box is on, turn it off.  This should ulitimately be redundant, as the player should not be able to move while interacting.
        }
    }
}
