using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offsides : MonoBehaviour
{
    public CameraSwitch cameraSwitch;
    public GameObject ball;

    void Start()
    {
        cameraSwitch = ball.GetComponent<CameraSwitch>();
    }

    void Update()
    {

        if (ball.transform.position.z > 0)
            cameraSwitch.BlueAttacking();
        else
            cameraSwitch.RedAttacking();
    }
}