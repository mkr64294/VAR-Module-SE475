using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offsides : MonoBehaviour
{
    public GameObject[] BluePlayers;
    public GameObject[] RedPlayers;
    //float[] BluePlayerPositions;
    //float[] RedPlayerPositions;
    public GameObject RedDotBlue;
    public GameObject GreenDotBlue;
    public GameObject RedDotRed;
    public GameObject GreenDotRed;
    public GameObject testDot;
    float BlueMax;
    float BlueMin;
    float RedMax;
    float RedMin;
    float BlueGK;
    float RedGK;

    // Update is called once per frame
    void Update()
    {
        BlueMax = 0;
        BlueMin = 0;
        RedMax = 0;
        RedMin = 0;

        for (int i = 0; i < 10; i++)
        {
            //BluePlayerPositions[i] = BluePlayers[i].transform.position.z;
           // RedPlayerPositions[i] = RedPlayers[i].transform.position.z;

            if (BluePlayers[i].transform.position.z > BlueMax)
            {
                BlueMax = BluePlayers[i].transform.position.z;
            }
            if (BluePlayers[i].transform.position.z < BlueMin)
            {
                BlueMin = BluePlayers[i].transform.position.z;
            }
            if (RedPlayers[i].transform.position.z > RedMax)
            {
                RedMax = RedPlayers[i].transform.position.z;
            }
            if (RedPlayers[i].transform.position.z < RedMin)
            {
                RedMin = RedPlayers[i].transform.position.z;
            }
        }


        //BlueGK = BluePlayerPositions[10];
        //RedGK = RedPlayerPositions[10];

        if (BlueMax > RedMax && GreenDotBlue.activeSelf == true)
        {
            RedDotBlue.SetActive(true);
            GreenDotBlue.SetActive(false);
        }
        if (BlueMax < RedMax && GreenDotBlue.activeSelf == false)
        {
            RedDotBlue.SetActive(false);
            GreenDotBlue.SetActive(true);
        }
        if (BlueMin > RedMin && GreenDotRed.activeSelf == true)
        {
            RedDotRed.SetActive(true);
            GreenDotRed.SetActive(false);
        }
        if (BlueMin < RedMin && GreenDotRed.activeSelf == false)
        {
            RedDotRed.SetActive(false);
            GreenDotRed.SetActive(true);
        }
    }
}
