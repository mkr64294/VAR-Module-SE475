using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOffsides : MonoBehaviour
{
    public GameObject[] RedPlayers;
    public GameObject[] BluePlayers;
    public GameObject BallReference;
    private List<GameObject> OffsidesPlayers = new List<GameObject>();

    float ballReference;
    float lastDef;

    // Update is called once per frame
    void Update()
    {
        setReference(BallReference, ref ballReference);
        if (ballReference < 0)
        {
            setLastDef(BluePlayers, ref lastDef);
            getOffsidesPlayers(RedPlayers, OffsidesPlayers, ballReference, lastDef);
            checkOffsidesPlayers(OffsidesPlayers, ballReference, lastDef);
        }
        else
            removeOffsidesPlayers(OffsidesPlayers);
    }

    private void setReference(GameObject Ball, ref float referenceToSet)
    {
        referenceToSet = Ball.transform.position.z;
    }

    private void setLastDef(GameObject[] Defenders, ref float lastDefender)
    {
        lastDefender = 0;
        for (int i = 0; i < Defenders.Length; i++)
        {
            if (Defenders[i].transform.position.z < lastDefender)
                lastDefender = Defenders[i].transform.position.z;
        }
    }

    private void getOffsidesPlayers(GameObject[] Players, List<GameObject> Offsides, float referenceMeasure, float defRef)
    {
        for (int i = 0; i < Players.Length; ++i)
        {
            if (Players[i].transform.position.z < defRef && Players[i].transform.position.z < referenceMeasure && !Offsides.Contains(Players[i]))
            {
                EnableDisableOffsides(Players[i], true);
                Offsides.Add(Players[i]);
            }
        }
    }

    private void checkOffsidesPlayers(List<GameObject> Offsides, float reference, float defRef)
    {
        for (int i = 0; i < Offsides.Count; ++i)
        {
            if (Offsides[i].transform.position.z > defRef || Offsides[i].transform.position.z > reference)
            {
                EnableDisableOffsides(Offsides[i], false);
                Offsides.Remove(Offsides[i]);
            }
        }
    }

    private void EnableDisableOffsides(GameObject player, bool offsides)
    {
        player.GetComponentInChildren<Outline>().enabled = offsides;
    }

    private void removeOffsidesPlayers(List<GameObject> Offsides)
    {
        for (int i = 0; i < OffsidesPlayers.Count; ++i)
        {
            EnableDisableOffsides(Offsides[i], false);
            Offsides.Remove(Offsides[i]);
        }
    }
}
