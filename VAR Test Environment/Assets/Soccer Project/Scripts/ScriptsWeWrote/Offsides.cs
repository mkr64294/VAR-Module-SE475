using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offsides : MonoBehaviour
{
    public GameObject ball;
    public GameObject BlueGoalKeeper;
    public GameObject RedGoalKeeper;
    public BlueOffsides BlueOnAttack;
    public RedOffsides RedOnAttack;
    public OnDefense SwitchHalves;

    void Start()
    {
        BlueOnAttack = RedGoalKeeper.GetComponent<BlueOffsides>();
        RedOnAttack = BlueGoalKeeper.GetComponent<RedOffsides>();
        SwitchHalves = ball.GetComponent<OnDefense>();
    }

    void Update()
    {

        if (ball.transform.position.z > 0 && !BlueOnAttack.enabled)
            BlueAttack();
        else if (ball.transform.position.z < 0 && !RedOnAttack.enabled)
            RedAttack();
        else
            Midline();
    }

    public void RedAttack()
    {
        RedOnAttack.enabled = true;
    }

    public void BlueAttack()
    {
        BlueOnAttack.enabled = true;
    }

    public void Midline()
    {
        RedOnAttack.enabled = false;
        BlueOnAttack.enabled = false;
        SwitchHalves.enabled = true;
        SwitchHalves.enabled = false;
    }
}
//    public GameObject[] BluePlayers;
//    public GameObject[] RedPlayers;
//    public GameObject RedDotBlue;
//   public GameObject GreenDotBlue;
//    public GameObject RedDotRed;
//    public GameObject GreenDotRed;
//    public List<GameObject> OffBlue = new List<GameObject>();
//    public List<GameObject> OffRed = new List<GameObject>();
//    float BlueMax;
//    float BlueMin;
//    float RedMax;
//    float RedMin;

    // Update is called once per frame
//    void Update()
//    {
        //BlueMin = 0;
        //RedMax = 0;

        ////Get second to last player of each team
        //setMAXMIN(BluePlayers, ref BlueMin);
        //setMAXMIN(RedPlayers, ref RedMax);

        ////Get offsides blue and red players
        //getOffsidesPlayers(BluePlayers, OffBlue, RedMax);
        //getOffsidesPlayers(RedPlayers, OffRed, BlueMin);

        ////Check if offsides players are now onsides
        //checkOffsidesPlayers(OffBlue, RedMax); 
        //checkOffsidesPlayers(OffRed, BlueMin);          

        //for (int i = 0; i < 10; i++)
        //{
        //    if (BluePlayers[i].transform.position.z > BlueMax)
        //    {
        //        BlueMax = BluePlayers[i].transform.position.z;
        //        OffBlue.Add(BluePlayers[i]);
        //    }
        //    if (BluePlayers[i].transform.position.z < BlueMin)
        //    {
        //        BlueMin = BluePlayers[i].transform.position.z;
        //        OffBlue.Clear();
        //    }
        //    if (RedPlayers[i].transform.position.z > RedMax)
        //    {
        //        RedMax = RedPlayers[i].transform.position.z;
        //        OffRed.Add(RedPlayers[i]);
        //    }
        //    if (RedPlayers[i].transform.position.z < RedMin)
        //    {
        //        RedMin = RedPlayers[i].transform.position.z;
        //        OffRed.Add(RedPlayers[i]);
        //    }
        //}

        //BlueGK = BluePlayerPositions[10];
        //RedGK = RedPlayerPositions[10];

        //// Blue player offsides
        //if (BlueMax > RedMax && GreenDotBlue.activeSelf == true)
        //{
        //    foreach (GameObject player in OffBlue)
        //        player.GetComponentInChildren<Outline>().enabled = true;
        //    RedDotBlue.SetActive(true);
        //    GreenDotBlue.SetActive(false);
        //}
        //if (BlueMax < RedMax && GreenDotBlue.activeSelf == false)
        //{
        //    foreach (GameObject player in OffBlue)
        //        player.GetComponentInChildren<Outline>().enabled = false;
        //    RedDotBlue.SetActive(false);
        //    GreenDotBlue.SetActive(true);
        //}
        //if (BlueMin > RedMin && GreenDotRed.activeSelf == true)
        //{
        //    foreach (GameObject player in OffRed)
        //        player.GetComponentInChildren<Outline>().enabled = true;
        //    RedDotRed.SetActive(true);
        //    GreenDotRed.SetActive(false);
        //}
        //if (BlueMin < RedMin && GreenDotRed.activeSelf == false)
        //{
        //    foreach (GameObject player in OffRed)
        //        player.GetComponentInChildren<Outline>().enabled = false;
        //    RedDotRed.SetActive(false);
        //    GreenDotRed.SetActive(true);
        //}
//    }

    //private void setMAXMIN(GameObject[] Players, ref float BRMaxMin)
    //{
    //    for(int i = 0; i < Players.Length - 1; ++i)
    //    {
    //        if (System.Math.Abs(Players[i].transform.position.z) > System.Math.Abs(BRMaxMin))
    //            BRMaxMin = System.Math.Abs(Players[i].transform.position.z);
    //    }
    //}

    //private void getOffsidesPlayers(GameObject[] Players, List<GameObject> Offsides, float reference)
    //{ 
    //    for(int i = 0; i < Players.Length - 1; ++i)
    //    {
    //        if (System.Math.Abs(Players[i].transform.position.z) > reference && !Offsides.Contains(Players[i]))
    //        {
    //            Players[i].GetComponentInChildren<Outline>().enabled = true;
    //            //EnableDisableOffsides(Players[i], true);
    //            Offsides.Add(Players[i]);
    //        }
    //    }
    //}

    //private void checkOffsidesPlayers(List<GameObject> Offsides, float reference)
    //{
    //    for (int i = 0; i < Offsides.Count; ++i)
    //    {
    //        if (System.Math.Abs(Offsides[i].transform.position.z) < reference)
    //        {
    //            Offsides[i].GetComponentInChildren<Outline>().enabled = false;
    //            //EnableDisableOffsides( Offsides[i], false);
    //            Offsides.Remove(Offsides[i]);
    //        }
    //    }
    //}

    //private void EnableDisableOffsides(GameObject player, bool offsides)
    //{
    //    player.GetComponentInChildren<Outline>().enabled = offsides;
    //}