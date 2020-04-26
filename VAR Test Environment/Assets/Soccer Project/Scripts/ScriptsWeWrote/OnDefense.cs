using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDefense : MonoBehaviour
{
    public GameObject[] Players;
    public List<Outline> PlayersOutline = new List<Outline>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Players.Length; ++i)
            PlayersOutline.Add(Players[i].GetComponentInChildren<Outline>());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PlayersOutline.Count; ++i)
            PlayersOutline[i].enabled = false;
    }
}
