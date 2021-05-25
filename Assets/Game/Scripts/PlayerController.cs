using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int health;                          //player hit poinst

    void awake()
    {
        DontDestroyOnLoad(this.gameObject);     //makes sure this data is not destroyed
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
