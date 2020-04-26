using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    public GameObject[] redPlayers;
    public GameObject[] bluePlayers;
    public GameObject ballPosition;
    private int playerInPosession = 1;
    private int teamInPosession = 1;
    public int bias = 65;
    private int draw;
    public float speed;
    float WPradius = 1;


    void Update()
    {

        if (Vector3.Distance(ballPosition.transform.position, redPlayers[playerInPosession].transform.position) < WPradius || Vector3.Distance(ballPosition.transform.position, bluePlayers[playerInPosession].transform.position) < WPradius)
        {
            draw = Random.Range(1, 100);
            playerInPosession = Random.Range(0, 10);
            if (draw >= 65)
            {
                teamInPosession = 3 - teamInPosession;
            }
        }

        if (teamInPosession == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, redPlayers[playerInPosession].transform.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, bluePlayers[playerInPosession].transform.position, Time.deltaTime * speed);
        }
    }
}
