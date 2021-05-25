using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;            //select the camera to control
    public float smoothing;             //controls how smoothly the camera follows the player.
    public Vector2 maxPosition;         //the maximum x and y values the camera position can have, creating boundaries to the screen.
    public Vector2 minPosition;         //the minimum x and y values the camera position can have, creating boundaries to the screen.

    // Start is called before the first frame update
    void Start()
    {
        //start the camera exactly at the hero's position within the acceptable range.
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);       //set the target position
        targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);                         //clamp the target position's x value
        targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);                         //clamp the target position's y value
        transform.position = targetPosition;                                                                    //set the camera directly to this position.
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //only move the camera if the camera is not already in position to prevent camera shaking
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);       //set the target position
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);                         //clamp the target position's x value
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);                         //clamp the target position's y value
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);                       //lerp the camera causing it to move slightly towards the destination to give an organic movement to the camera.
        }

    }
}
