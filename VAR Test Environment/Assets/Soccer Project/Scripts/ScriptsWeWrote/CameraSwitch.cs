using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera RedCamera;
    public Camera BlueCamera;
    public GameObject Ball;

    // Start is called before the first frame update
    void Start()
    {
        BlueCamera.enabled = true;
        RedCamera.enabled = false;
    }
    public void BlueAttacking()
    {
        BlueCamera.gameObject.SetActive(false);
        RedCamera.gameObject.SetActive(true);
    }

    public void RedAttacking()
    {
        RedCamera.gameObject.SetActive(false);
        BlueCamera.gameObject.SetActive(true);
    }
}
