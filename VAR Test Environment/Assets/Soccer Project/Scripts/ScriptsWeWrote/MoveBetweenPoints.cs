using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public GameObject[] positions;
    public GameObject player;
    int current = 0;
    public float speed;
    float WPradius = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(positions[current].transform.position, transform.position) < WPradius)
        {
            current = Random.Range(0, positions.Length);
            if (current >= positions.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, positions[current].transform.position, Time.deltaTime * speed);

    }
}
