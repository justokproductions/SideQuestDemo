using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Moving between zones within a singular scene.  Between towns and wildernesses, or in dungeons will be between one room and another.
public class ZoneCollision : MonoBehaviour
{
    public ZoneValues targetZone;                       //the zone the hero is moving into
    public Vector3 playerChange;                        //how far forward the hero moves in the transition
    private CameraMovement cam;                         //the player camera
    public bool needText;                               //does text appear indicating where you are
    public GameObject banner;                           //the banner that displays the values
    public Text placeText;                              //what is the text that appears

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();   //pass the camera to the function
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //triggers when the hero enters this square
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();            //confirms the player is the one who entered
        if (playerMovement != null)
        {
            //change the location of the camera and player
            cam.minPosition = targetZone.GetComponent<ZoneValues>().cameraMin;                  //set minimum values for the camera movement
            cam.maxPosition = targetZone.GetComponent<ZoneValues>().cameraMax;                  //set maximum values for the camera movement
            other.transform.position += playerChange;                                           //teleports the player forward to the new starting square

            //if a title card is needed, it begins the coroutine to display the information
            if(needText)
            {
                StartCoroutine(placeNameCo());
            }
        }
    }

    //Coroutine to display then hide the banner showing you which zone you are in.
    private IEnumerator placeNameCo()
    {
        banner.SetActive(true);                                                     //unhides the banner
        placeText.text = targetZone.GetComponent<ZoneValues>().zoneName;            //updates the text
        yield return new WaitForSeconds(4f);                                        //waits 4 seconds
        banner.SetActive(false);                                                    //hides banner.
    }
}
