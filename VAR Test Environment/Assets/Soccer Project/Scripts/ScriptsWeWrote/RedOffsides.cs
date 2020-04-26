﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOffsides : MonoBehaviour
{
    public GameObject[] RedPlayers;
    public GameObject BallReference;
    public List<GameObject> OffsidesPlayers = new List<GameObject>();

    float reference;

    // Update is called once per frame
    void Update()
    {
        setReference(BallReference, ref reference);
        getOffsidesPlayers(RedPlayers, OffsidesPlayers, reference);
        checkOffsidesPlayers(OffsidesPlayers, reference);
    }

    private void setReference(GameObject Ball, ref float referenceToSet)
    {
        referenceToSet = Ball.transform.position.z;
    }

    private void getOffsidesPlayers(GameObject[] Players, List<GameObject> Offsides, float referenceMeasure)
    {
        for (int i = 0; i < Players.Length; ++i)
        {
            if (Players[i].transform.position.z < referenceMeasure && !Offsides.Contains(Players[i]))
            {
                EnableDisableOffsides(Players[i], true);
                Offsides.Add(Players[i]);
            }
        }
    }

    private void checkOffsidesPlayers(List<GameObject> Offsides, float reference)
    {
        for (int i = 0; i < Offsides.Count; ++i)
        {
            if (Offsides[i].transform.position.z > reference)
            {
                EnableDisableOffsides( Offsides[i], false);
                Offsides.Remove(Offsides[i]);
            }
        }
    }

    private void EnableDisableOffsides(GameObject player, bool offsides)
    {
        player.GetComponentInChildren<Outline>().enabled = offsides;
    }
}
