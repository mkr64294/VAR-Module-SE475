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
    public List<GameObject> OffBlue;
    public List<GameObject> OffRed;
    float BlueMax;
    float BlueMin;
    float RedMax;
    float RedMin;
    float BlueGK;
    float RedGK;
    
//    void OutlineTest() {
//        // Outline stuff
//        var outline = BluePlayers[7].AddComponent<Outline>();
//
//        outline.OutlineMode = Outline.Mode.OutlineAll;
//        outline.OutlineColor = Color.red;
//        outline.OutlineWidth = 5f;
//        
//        outline.enable();
//    }


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
                OffBlue.Add(BluePlayers[i]);
            }
            if (BluePlayers[i].transform.position.z < BlueMin)
            {
                BlueMin = BluePlayers[i].transform.position.z;
                OffBlue.Clear();
            }
            if (RedPlayers[i].transform.position.z > RedMax)
            {
                RedMax = RedPlayers[i].transform.position.z;
                OffRed.Add(RedPlayers[i]);
            }
            if (RedPlayers[i].transform.position.z < RedMin)
            {
                RedMin = RedPlayers[i].transform.position.z;
                OffRed.Clear();
            }
        }


        //BlueGK = BluePlayerPositions[10];
        //RedGK = RedPlayerPositions[10];

        // Blue player offsides
        if (BlueMax > RedMax && GreenDotBlue.activeSelf == true)
        {
            foreach (GameObject player in OffBlue)
                player.GetComponentInChildren<Outline>().enabled = true;
            RedDotBlue.SetActive(true);
            GreenDotBlue.SetActive(false);
        }
        if (BlueMax < RedMax && GreenDotBlue.activeSelf == false)
        {
            foreach (GameObject player in OffBlue)
                player.GetComponentInChildren<Outline>().enabled = false;
            RedDotBlue.SetActive(false);
            GreenDotBlue.SetActive(true);
        }
        if (BlueMin > RedMin && GreenDotRed.activeSelf == true)
        {
            foreach (GameObject player in OffRed)
                player.GetComponentInChildren<Outline>().enabled = true;
            RedDotRed.SetActive(true);
            GreenDotRed.SetActive(false);
        }
        if (BlueMin < RedMin && GreenDotRed.activeSelf == false)
        {
            foreach (GameObject player in OffRed)
                player.GetComponentInChildren<Outline>().enabled = false;
            RedDotRed.SetActive(false);
            GreenDotRed.SetActive(true);
        }
    }
}
