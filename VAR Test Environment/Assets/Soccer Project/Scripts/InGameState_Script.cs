using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class InGameState_Script : MonoBehaviour {

	
	public enum InGameState {
		
		PLAYING,
		KICK_OFF,
		GOAL,
		SAQUE_BANDA,
		SAQUE_BANDA_APUNTANDO,
		SAQUE_BANDA_SACANDO,
		SAQUE_BANDA_SACADO,
		CORNER,
		CORNER_APUNTANDO,
		CORNER_SACANDO,
		CORNER_SACANDO_2,
		CORNER_SACADO,
		SAQUE_PUERTA,
		SAQUE_PUERTA_CARRERILLA,
		SAQUE_PUERTA_SACANDO
		
	};
	
	public InGameState state;
	private GameObject[] players;
	private GameObject[] oponents;
	private GameObject keeper;
	private GameObject keeper_oponent;
	public GameObject lastTouched;
	public float timeToChangeState = 0.0f;
	public Vector3 postionBanda;
	public Sphere sphere;
	public Transform center;
	public Vector3 target_saque_banda;
	
	private GameObject whoLastTouched;
	public GameObject candidateTosaqueBanda;
	private float timeToSaqueOponent = 3.0f;
	
	public Transform cornerSource;
	public GameObject areaCorner;
	public Transform saque_porteria;
	public GameObject goalKeeper;
	public GameObject cornerTrigger;
	
	public Mesh[] Meshes;
	public Material[] Mat;
	
	private float timeToKickOff = 4.0f;
	public GameObject lastCandidate = null;
	
	public int score_local = 0;
	public int score_visiting = 0;
	public GUIText scorer_local;
	public GUIText scorer_visiting;
	
	public GameObject[] playerPrefab;
	public GameObject goalKeeperPrefab;
	public GameObject ballPrefab;
	
	public Material mat_temp;
	public Transform target_oponent_goal;
	

	
	void Awake() {
		
	}
		// Use this for initialization
	void Start () {
	
		players = GameObject.FindGameObjectsWithTag("PlayerTeam1");
		oponents = GameObject.FindGameObjectsWithTag("OponentTeam");
		keeper = GameObject.FindGameObjectWithTag("GoalKeeper");
		keeper_oponent = GameObject.FindGameObjectWithTag("GoalKeeper_Oponent");
		
		state = InGameState.PLAYING;
	
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
		// update scorer
		scorer_local.text = "" + score_local;
		scorer_visiting.text = "" + score_visiting;
		
		
		timeToChangeState -= Time.deltaTime;
		
		if ( timeToChangeState < 0.0f ) {
		
			switch (state) {
				
				case InGameState.PLAYING:
			
				break;
	
				case InGameState.SAQUE_BANDA:
				
					whoLastTouched = lastTouched;	
				
					foreach ( GameObject go in players ) {
						go.GetComponent<Player_Script>().state = Player_Script.Player_State.RESTING;
					}
					foreach ( GameObject go in oponents ) {
						go.GetComponent<Player_Script>().state = Player_Script.Player_State.RESTING;
					}
				
				
					sphere.owner = null;
				
					if ( whoLastTouched.tag == "PlayerTeam1" )
						candidateTosaqueBanda = SearchPlayerNearBall( oponents );
					else	
						candidateTosaqueBanda = SearchPlayerNearBall( players );
				
				
					candidateTosaqueBanda.transform.position = new Vector3( postionBanda.x, candidateTosaqueBanda.transform.position.y, postionBanda.z);
				
					if ( whoLastTouched.tag == "PlayerTeam1" ) {
					
						candidateTosaqueBanda.GetComponent<Player_Script>().temporallyUnselectable = true;
						candidateTosaqueBanda.GetComponent<Player_Script>().timeToBeSelectable = 1.0f;
				
						candidateTosaqueBanda.transform.LookAt( SearchPlayerNearBall( oponents ).transform.position);
					}
					else
						candidateTosaqueBanda.transform.LookAt( center ); 
	
				
					candidateTosaqueBanda.transform.Rotate(0, sphere.fHorizontal*10.0f, 0);
					candidateTosaqueBanda.GetComponent<Player_Script>().state = Player_Script.Player_State.SAQUE_BANDA;
				
					sphere.GetComponent<Rigidbody>().isKinematic = true;
					sphere.gameObject.transform.position = candidateTosaqueBanda.GetComponent<Player_Script>().hand_bone.position;
				
					target_saque_banda = candidateTosaqueBanda.transform.position + candidateTosaqueBanda.transform.forward;
				
				
					candidateTosaqueBanda.GetComponent<Animation>().Play("saque_banda");
					candidateTosaqueBanda.GetComponent<Animation>()["saque_banda"].time = 0.1f;
					candidateTosaqueBanda.GetComponent<Animation>()["saque_banda"].speed = 0.0f;
				
					state = InGameState.SAQUE_BANDA_APUNTANDO;
				
				break;
				case InGameState.SAQUE_BANDA_APUNTANDO:
				

					candidateTosaqueBanda.transform.position = new Vector3( postionBanda.x, candidateTosaqueBanda.transform.position.y, postionBanda.z);
					candidateTosaqueBanda.transform.LookAt( target_saque_banda );
					candidateTosaqueBanda.GetComponent<Player_Script>().state = Player_Script.Player_State.SAQUE_BANDA;
				
					sphere.GetComponent<Rigidbody>().isKinematic = true;
					sphere.gameObject.transform.position = candidateTosaqueBanda.GetComponent<Player_Script>().hand_bone.position;

					if ( whoLastTouched.tag != "PlayerTeam1" ) {
				
						target_saque_banda += new Vector3( 0,0,sphere.fHorizontal/10.0f);
					
						if (sphere.bPassButton) {
							candidateTosaqueBanda.GetComponent<Animation>().Play("saque_banda");
							state = InGameState.SAQUE_BANDA_SACANDO;
		
						}
						
					} else {
					
						timeToSaqueOponent -= Time.deltaTime;
					
						if ( timeToSaqueOponent < 0.0f ) {					
							timeToSaqueOponent = 3.0f;
							sphere.gameObject.GetComponent<Rigidbody>().isKinematic = true;
							candidateTosaqueBanda.GetComponent<Animation>().Play("saque_banda");
							state = InGameState.SAQUE_BANDA_SACANDO;
						}
					
					}
				
				break;	
				
				case InGameState.SAQUE_BANDA_SACANDO:
					
					candidateTosaqueBanda.GetComponent<Animation>()["saque_banda"].speed = 1.0f;

					if ( candidateTosaqueBanda.GetComponent<Animation>()["saque_banda"].normalizedTime < 0.5f && sphere.gameObject.GetComponent<Rigidbody>().isKinematic == true ) {
						sphere.gameObject.transform.position = candidateTosaqueBanda.GetComponent<Player_Script>().hand_bone.position;
					}

					if ( candidateTosaqueBanda.GetComponent<Animation>()["saque_banda"].normalizedTime >= 0.5f && sphere.gameObject.GetComponent<Rigidbody>().isKinematic == true ) {
						sphere.gameObject.GetComponent<Rigidbody>().isKinematic = false;
						sphere.gameObject.GetComponent<Rigidbody>().AddForce( candidateTosaqueBanda.transform.forward*4000.0f + new Vector3(0.0f, 1300.0f, 0.0f) );					
					} 
				
				
				
					if ( candidateTosaqueBanda.GetComponent<Animation>().IsPlaying("saque_banda") == false ) {
						state = InGameState.SAQUE_BANDA_SACADO;
					}
				
				
				break;

				case InGameState.SAQUE_BANDA_SACADO:
					candidateTosaqueBanda.GetComponent<Player_Script>().state = Player_Script.Player_State.MOVE_AUTOMATIC;
					state = InGameState.PLAYING;
				
				break;
				
				
				
				
				case InGameState.CORNER:
				
					whoLastTouched = lastTouched;	
				
					if ( whoLastTouched.tag == "GoalKeeper_Oponent" )
						whoLastTouched.tag = "OponentTeam";
					if ( whoLastTouched.tag == "GoalKeeper" )
						whoLastTouched.tag = "PlayerTeam1";
				
				
				
					// decidimos si es Corner o Saque de puerta
				
					if ( cornerTrigger.tag == "Corner_Oponent" && whoLastTouched.tag == "PlayerTeam1") {
						state = InGameState.SAQUE_PUERTA;
						break;
					}
					if ( cornerTrigger.tag != "Corner_Oponent" && whoLastTouched.tag == "OponentTeam" ) {
						state = InGameState.SAQUE_PUERTA;
						break;
					}
				
				
				
				
					foreach ( GameObject go in players ) {
						go.GetComponent<Player_Script>().state = Player_Script.Player_State.RESTING;
					}
					foreach ( GameObject go in oponents ) {
						go.GetComponent<Player_Script>().state = Player_Script.Player_State.RESTING;
					}
				
				
					sphere.owner = null;
				
					if ( whoLastTouched.tag == "PlayerTeam1" ) {
						PutPlayersInCornerArea( players, Player_Script.TypePlayer.DEFENDER );
						PutPlayersInCornerArea( oponents, Player_Script.TypePlayer.ATTACKER );
						candidateTosaqueBanda = SearchPlayerNearBall( oponents );
					}
					else {	
						PutPlayersInCornerArea( oponents, Player_Script.TypePlayer.DEFENDER );
						PutPlayersInCornerArea( players, Player_Script.TypePlayer.ATTACKER );
						candidateTosaqueBanda = SearchPlayerNearBall( players );
					}				
				
					candidateTosaqueBanda.transform.position = new Vector3 ( cornerSource.position.x, candidateTosaqueBanda.transform.position.y, cornerSource.position.z);
					
				
					if ( whoLastTouched.tag == "PlayerTeam1" ) {
					
						candidateTosaqueBanda.GetComponent<Player_Script>().temporallyUnselectable = true;
						candidateTosaqueBanda.GetComponent<Player_Script>().timeToBeSelectable = 1.0f;
				
						candidateTosaqueBanda.transform.LookAt( SearchPlayerNearBall( oponents ).transform.position);

					}
					else
						candidateTosaqueBanda.transform.LookAt( center ); 
	
				
	
				
					candidateTosaqueBanda.transform.Rotate(0, sphere.fHorizontal*10.0f, 0);
					candidateTosaqueBanda.GetComponent<Player_Script>().state = Player_Script.Player_State.SAQUE_CORNER;
				
					sphere.GetComponent<Rigidbody>().isKinematic = true;
//					sphere.gameObject.transform.position = candidateTosaqueBanda.GetComponent<Player_Script>().hand_bone.position;
				
					sphere.gameObject.transform.position = cornerSource.position;
				
				
					target_saque_banda = candidateTosaqueBanda.transform.position + candidateTosaqueBanda.transform.forward;
				
				
					candidateTosaqueBanda.GetComponent<Animation>().Play("reposo");
					state = InGameState.CORNER_APUNTANDO;
				
				break;

					
			case InGameState.CORNER_APUNTANDO:


				candidateTosaqueBanda.transform.LookAt( target_saque_banda );
				candidateTosaqueBanda.GetComponent<Player_Script>().state = Player_Script.Player_State.SAQUE_CORNER;
				
				sphere.GetComponent<Rigidbody>().isKinematic = true;

				if ( whoLastTouched.tag != "PlayerTeam1" ) {
				
					target_saque_banda += Camera.main.transform.right*(sphere.fHorizontal/10.0f);
					
					if (sphere.bPassButton) {
						candidateTosaqueBanda.GetComponent<Animation>().Play("pasos_atras");
						state = InGameState.CORNER_SACANDO;
		
					}
						
				} else {
					
					timeToSaqueOponent -= Time.deltaTime;
					
					if ( timeToSaqueOponent < 0.0f ) {					
						timeToSaqueOponent = 3.0f;
						sphere.gameObject.GetComponent<Rigidbody>().isKinematic = true;
						candidateTosaqueBanda.GetComponent<Animation>().Play("pasos_atras");
						state = InGameState.CORNER_SACANDO;
					}
					
				}

				
				
			break;

				
			case InGameState.CORNER_SACANDO:
			
				candidateTosaqueBanda.transform.position -= candidateTosaqueBanda.transform.forward * Time.deltaTime;
				
				if ( candidateTosaqueBanda.GetComponent<Animation>().IsPlaying("pasos_atras") == false ) {
					
					candidateTosaqueBanda.GetComponent<Animation>().Play("saque_esquina");
					state = InGameState.CORNER_SACANDO_2;
				}
				
			break;				
				
				
			case InGameState.CORNER_SACANDO_2:
				

				if ( candidateTosaqueBanda.GetComponent<Animation>()["saque_esquina"].normalizedTime >= 0.5f && sphere.gameObject.GetComponent<Rigidbody>().isKinematic == true ) {
					sphere.gameObject.GetComponent<Rigidbody>().isKinematic = false;
					sphere.gameObject.GetComponent<Rigidbody>().AddForce( candidateTosaqueBanda.transform.forward*7000.0f + new Vector3(0.0f, 3300.0f, 0.0f) );					
				} 
				
				
				if ( candidateTosaqueBanda.GetComponent<Animation>().IsPlaying("saque_esquina") == false ) {
					state = InGameState.CORNER_SACADO;
				}
				
				
				
			break;
				
				
				
			case InGameState.CORNER_SACADO:
				
				candidateTosaqueBanda.GetComponent<Player_Script>().state = Player_Script.Player_State.MOVE_AUTOMATIC;				
				state = InGameState.PLAYING;
				
			break;
				
				
			case InGameState.SAQUE_PUERTA:
				
				sphere.transform.position = saque_porteria.position;
				sphere.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				goalKeeper.transform.rotation = saque_porteria.transform.rotation;
				goalKeeper.transform.position = new Vector3( saque_porteria.transform.position.x, goalKeeper.transform.position.y ,saque_porteria.transform.position.z)- (goalKeeper.transform.forward*1.0f);
				goalKeeper.GetComponent<GoalKeeper_Script>().state = GoalKeeper_Script.GoalKeeper_State.SAQUE_PUERTA;
							
		
				foreach ( GameObject go in players ) {
					go.GetComponent<Player_Script>().state = Player_Script.Player_State.GO_ORIGIN;
				}
				foreach ( GameObject go in oponents ) {
					go.GetComponent<Player_Script>().state = Player_Script.Player_State.GO_ORIGIN;
				}
				
				sphere.owner = null;

			
				goalKeeper.GetComponent<Animation>().Play("pasos_atras");	
				state = InGameState.SAQUE_PUERTA_CARRERILLA;
				
				
			break;
			case InGameState.SAQUE_PUERTA_CARRERILLA:
				
				goalKeeper.transform.position -= goalKeeper.transform.forward * Time.deltaTime;
				
				if ( goalKeeper.GetComponent<Animation>().IsPlaying("pasos_atras") == false ) {
					goalKeeper.GetComponent<Animation>().Play("saque_esquina");	
					state = InGameState.SAQUE_PUERTA_SACANDO;
				}
			
				
			break;	
				
			case InGameState.SAQUE_PUERTA_SACANDO:
				
				goalKeeper.transform.position += goalKeeper.transform.forward * Time.deltaTime;

				if ( goalKeeper.GetComponent<Animation>()["saque_esquina"].normalizedTime >= 0.5f && sphere.gameObject.GetComponent<Rigidbody>().isKinematic == true) {
					sphere.gameObject.GetComponent<Rigidbody>().isKinematic = false;
					float force = Random.Range(5000.0f, 12000.0f);
					sphere.gameObject.GetComponent<Rigidbody>().AddForce( (goalKeeper.transform.forward*force) + new Vector3(0,3000.0f,0) );
				}
	
				if ( goalKeeper.GetComponent<Animation>().IsPlaying("saque_esquina") == false ) {

					goalKeeper.GetComponent<GoalKeeper_Script>().state = GoalKeeper_Script.GoalKeeper_State.GO_ORIGIN;	
					state = InGameState.PLAYING;
					
				}
				
			break;

			case InGameState.GOAL:
				
				
				foreach ( GameObject go in players ) {
					go.GetComponent<Player_Script>().state = Player_Script.Player_State.SAQUE_BANDA;
					go.GetComponent<Animation>().Play("reposo");
				}
				foreach ( GameObject go in oponents ) {
					go.GetComponent<Player_Script>().state = Player_Script.Player_State.SAQUE_BANDA;
					go.GetComponent<Animation>().Play("reposo");
				}
				
					keeper_oponent.GetComponent<GoalKeeper_Script>().state = GoalKeeper_Script.GoalKeeper_State.RESTING;
					keeper.GetComponent<GoalKeeper_Script>().state = GoalKeeper_Script.GoalKeeper_State.RESTING;
				
				timeToKickOff -= Time.deltaTime;
				
				if ( timeToKickOff < 0.0f ) {
					timeToKickOff = 4.0f;
					state = InGameState_Script.InGameState.KICK_OFF;
				}
				
				
			break;


				
			case InGameState.KICK_OFF:
				
				
				foreach ( GameObject go in players ) {
					go.GetComponent<Player_Script>().state = Player_Script.Player_State.MOVE_AUTOMATIC;
					go.transform.position = go.GetComponent<Player_Script>().initialPosition;
				}
				foreach ( GameObject go in oponents ) {
					go.GetComponent<Player_Script>().state = Player_Script.Player_State.MOVE_AUTOMATIC;
					go.transform.position = go.GetComponent<Player_Script>().initialPosition;
				}
				
				keeper.GetComponent<GoalKeeper_Script>().state = GoalKeeper_Script.GoalKeeper_State.RESTING;
				keeper_oponent.GetComponent<GoalKeeper_Script>().state = GoalKeeper_Script.GoalKeeper_State.RESTING;
				
				sphere.owner = null;
				sphere.gameObject.transform.position = center.position;
				sphere.gameObject.GetComponent<Rigidbody>().drag = 0.5f;
				state = InGameState_Script.InGameState.PLAYING;
				
			break;				
				
				
				
			}
		
		}
		
	}
	
	GameObject SearchPlayerNearBall( GameObject[] arrayPlayers) {
		
	    GameObject candidatePlayer = null;
		float distance = 1000.0f;
		foreach ( GameObject player in arrayPlayers ) {			
			
			if ( !player.GetComponent<Player_Script>().temporallyUnselectable ) {
				
				Vector3 relativePos = sphere.transform.InverseTransformPoint( player.transform.position );		
				float newdistance = relativePos.magnitude;
				
				if ( newdistance < distance ) {
				
					distance = newdistance;					
					candidatePlayer = player;					

				}
			}
			
		}
						
		return candidatePlayer;	
	}
	
	
	
	
	void PutPlayersInCornerArea( GameObject[] arrayPlayers, Player_Script.TypePlayer type) {
	
		
		foreach ( GameObject player in arrayPlayers ) {			
			
			if ( player.GetComponent<Player_Script>().type == type ) {
			
			
				float xmin = areaCorner.GetComponent<BoxCollider>().bounds.min.x;
				float xmax = areaCorner.GetComponent<BoxCollider>().bounds.max.x;
				float zmin = areaCorner.GetComponent<BoxCollider>().bounds.min.z;
				float zmax = areaCorner.GetComponent<BoxCollider>().bounds.max.z;
				
				float x = Random.Range( xmin, xmax );
				float z = Random.Range( zmin, zmax );
				
				player.transform.position = new Vector3( x, player.transform.position.y ,z);
				
				
			}
			
			
		}		
		
		
		
	}
	
	
	
}
