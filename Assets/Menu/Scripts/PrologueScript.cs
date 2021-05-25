using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueScript : MonoBehaviour
{
    // Start is called before the first frame update
    // Place holder data.  Right now it instantly loads the main game.
    void Start()
    {
        SceneManager.LoadScene("Guy_House", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
