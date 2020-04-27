using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown("space") && Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Time.timeScale = 1;
        }
    }
}
