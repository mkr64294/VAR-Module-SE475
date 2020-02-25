using UnityEngine;
using System.Collections;

public class Communicator : MonoBehaviour {
	
	
	public static string nameCPU;
	public static string namePlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Awake () {
		DontDestroyOnLoad(this);
		
	}
}
