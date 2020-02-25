using UnityEngine;
using System;
using System.Collections;
public class Player_Script : MonoBehaviour {

// player name
public string Name;
public TypePlayer type = TypePlayer.DEFENDER;
public float Speed = 1.0f;	
public float Strong = 1.0f;
public float Control = 1.0f;
	

private const float STAMINA_DIVIDER = 64.0f;
private const float STAMINA_MIN = 0.5f;	
private const float STAMINA_MAX = 1.0f;	
	
	
public enum TypePlayer {
		DEFENDER,
		MIDDLER,
		ATTACKER
	};
	
public Vector3 actualVelocityPlayer;
private Vector3 oldVelocityPlayer;
public Sphere sphere;
private GameObject[] players;
private GameObject[] oponents;
public Vector3 initialPosition;
private float inputSteer;
private const float initialDisplacement = 20.0f;	
public Transform goalPosition;
public Transform headTransform;	
[HideInInspector]	
public bool temporallyUnselectable = true;
[HideInInspector]	
public float timeToBeSelectable = 1.0f;	
public float maxDistanceFromPosition = 20.0f;	
	
public enum Player_State { 
	   RESTING,
	   GO_ORIGIN,
	   CONTROLLING,
	   PASSING,
	   SHOOTING,
	   MOVE_AUTOMATIC,
	   ONE_STEP_BACK,
	   STOLE_BALL,
	   OPONENT_ATTACK,
	   PICK_BALL,
	   CHANGE_DIRECTION,
	   SAQUE_BANDA,
	   SAQUE_CORNER
	  };
   
public Player_State state;
public Material material_bound;
private float timeToRemove = 3.0f;	
private float timeToPass = 1.0f;
	
// hand of player in squeleton hierarchy
public Transform hand_bone;
	
public InGameState_Script inGame;
	
public Texture barTexture;
public Texture barStaminaTexture;
private int barPosition=0;
private Quaternion initialRotation;	
	
public float stamina = 64.0f;	
void  Awake (){
	animation.Stop();
 	state = Player_State.MOVE_AUTOMATIC; 
}



void  Start (){
		
	// change size of player randomly to get different look
//	transform.localScale = new Vector3( UnityEngine.Random.Range(0.9F, 1.1F), UnityEngine.Random.Range(0.9F, 1.1F), UnityEngine.Random.Range(0.9F, 1.1F) );
	players = GameObject.FindGameObjectsWithTag("PlayerTeam1");
	oponents = GameObject.FindGameObjectsWithTag("OponentTeam");

	if ( gameObject.tag == "PlayerTeam1" )
		initialPosition = new Vector3( transform.position.x, transform.position.y, transform.position.z+initialDisplacement ); 

	if ( gameObject.tag == "OponentTeam" )
		initialPosition = new Vector3( transform.position.x, transform.position.y, transform.position.z-initialDisplacement ); 

	// set animations speed to fit perfect movements		
	animation["retroceder_bucle"].speed = 1.5f;
	animation["arranque"].speed = 1.0f;
	animation["arranque_balon"].speed = 1.0f;
	animation["corriendo"].speed = 1.2f;
	animation["correr_balon"].speed = 1.0f;
	animation["pase"].speed = 1.8f;
	animation["reposo"].speed = 1.0f;
	animation["cambio_sentido"].speed = 1.3f;

	animation["entrada"].speed = 1.2f;	
	// para el movimiento de la cabeza de los jugadores
		
	animation.Play("reposo");	
		
	initialRotation = transform.rotation * headTransform.rotation;
	}
	
	
// control of actual player	
void Case_Controlling() {

	if ( sphere.inputPlayer == gameObject ) {
				
		
		if ( sphere.fVertical != 0.0f || sphere.fHorizontal != 0.0f ) {
					
			oldVelocityPlayer = actualVelocityPlayer;
			
			Vector3 right = inGame.transform.right;
			Vector3 forward = inGame.transform.forward;
				
			right *= sphere.fHorizontal;
			forward *= sphere.fVertical;
				
			Vector3 target = transform.position + right + forward;
			target.y = transform.position.y;
						
			float speedForAnimation = 5.0f;
			
			// if is owner of Ball....
			if ( sphere.owner == gameObject ) {
			
				if ( animation.IsPlaying("reposo") ) {
					animation.Play("arranque_balon");
					speedForAnimation = 1.0f;
				}
					
				if ( animation.IsPlaying("arranque_balon") == false )
					animation.Play("correr_balon");
			
			}
			else {
					
				if ( animation.IsPlaying("reposo") ) {
					animation.Play("arranque");
					speedForAnimation = 1.0f;
				}
					
				if ( animation.IsPlaying("arranque") == false )
					animation.Play("corriendo");
					
			}
				
				
			transform.LookAt( target );
			float staminaTemp = Mathf.Clamp ((stamina/STAMINA_DIVIDER), STAMINA_MIN ,STAMINA_MAX );
			actualVelocityPlayer = transform.forward*speedForAnimation*Time.deltaTime*staminaTemp*Speed;
			transform.position += actualVelocityPlayer;
			
				
			// if get a radical diferent direction of player change animation...		
			float dotp = Vector3.Dot( oldVelocityPlayer.normalized, actualVelocityPlayer.normalized );
			
			if ( dotp < 0.0f && sphere.owner == gameObject ) {
		
				animation.Play("cambio_sentido");
				state = Player_State.CHANGE_DIRECTION;
				transform.forward = -transform.forward;
				sphere.owner = null;
				gameObject.GetComponent<CapsuleCollider>().enabled = false;
				sphere.gameObject.GetComponent<Rigidbody>().AddForce(  -transform.forward.x*1500.0f, -transform.forward.y*1500.0f, -transform.forward.z*1500.0f );
			}
				
				
		} else {
	
			animation.Play("reposo");
		}
			
			
		// pass
		if ( sphere.bPassButton && sphere.owner == gameObject ) {
			animation.Play("pase");
			timeToBeSelectable = 2.0f;
			state = Player_State.PASSING;
			sphere.pressiPhonePassButton = false;
		}
				
		// shoot
		if ( sphere.bShootButtonFinished && sphere.owner == gameObject ) {
			animation.Play("tiro");
			timeToBeSelectable = 2.0f;
			state = Player_State.SHOOTING;
			sphere.pressiPhoneShootButton = false;
			sphere.bShootButtonFinished = false;
		}
			
				
						
	} else {
	
		state = Player_State.MOVE_AUTOMATIC;
			
	}
		
}
	
bool NoOneInFront( GameObject[] team_players ) {
	
		
	foreach( GameObject go in team_players ) {

		Vector3 relativePos = transform.InverseTransformPoint( go.transform.position ); 
		
		if ( relativePos.z > 0.0f )
			return true;		
	}
		
	return false;
		
}
	
	
// NPC control
void Case_Oponent_Attack() {
		
		actualVelocityPlayer = transform.forward*5.0f*Time.deltaTime;
		animation.Play("correr_balon");
		Vector3 RelativeWaypointPosition = transform.InverseTransformPoint(goalPosition.position);
		inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
		transform.Rotate(0, inputSteer*10.0f , 0);
		float staminaTemp = Mathf.Clamp ((stamina/STAMINA_DIVIDER), STAMINA_MIN ,STAMINA_MAX );
		transform.position += transform.forward*4.0f*Time.deltaTime*staminaTemp*Speed;
		
		timeToPass -= Time.deltaTime;
		
		if ( timeToPass < 0.0f && NoOneInFront( oponents ) ) {
			timeToPass = UnityEngine.Random.Range( 1.0f, 5.0f);	
			state = Player_State.PASSING;
			animation.Play("pase");
			timeToBeSelectable = 1.0f;
			temporallyUnselectable = true;
		}
		
		float distance = (goalPosition.position - transform.position).magnitude;
		Vector3 relative = transform.InverseTransformPoint(goalPosition.position);
		
		if ( distance < 20.0f && relative.z > 0 ) {

			state = Player_State.SHOOTING;
			animation.Play("tiro");
			timeToBeSelectable = 1.0f;
			temporallyUnselectable = true;
			
		}
		
}
	
void LateUpdate() {
		

		Vector3 relativePos = transform.InverseTransformPoint( sphere.gameObject.transform.position );

		
		if ( relativePos.z > 0.0f ) {
	
			Quaternion lookRotation = Quaternion.LookRotation (sphere.transform.position + new Vector3(0, 1.0f,0) - headTransform.position);
			headTransform.rotation = lookRotation * initialRotation ;			
			headTransform.eulerAngles = new Vector3( headTransform.eulerAngles.x, headTransform.eulerAngles.y,  -90.0f);
			
		}
				
	}
	
void  Update() {
				
	stamina += 2.0f * Time.deltaTime;

		
	stamina = Mathf.Clamp(stamina, 1, 64);		
	switch ( state ) {
				
			
		case Player_State.SAQUE_BANDA:
			
		break;

		case Player_State.SAQUE_CORNER:
			
		break;
			
		case Player_State.CHANGE_DIRECTION:
			
			if ( !animation.IsPlaying("cambio_sentido")) {
				gameObject.GetComponent<CapsuleCollider>().enabled = true;
				transform.forward = -transform.forward;
				animation.Play("reposo");
				state = Player_State.CONTROLLING;
			}
			
			
		break;
			
			
 		case Player_State.CONTROLLING:
		
			if ( gameObject.tag == "PlayerTeam1" ) 
				Case_Controlling();			
		break;

		case Player_State.OPONENT_ATTACK:
	
			Case_Oponent_Attack();			

		break;
			
			
		case Player_State.PICK_BALL:
			transform.position += transform.forward * Time.deltaTime * 5.0f;
						
			if (animation.IsPlaying("entrada") == false) {
				
				if ( gameObject.tag == "OponentTeam" )
					state = Player_State.OPONENT_ATTACK;
				else
					state = Player_State.MOVE_AUTOMATIC;
					
			}

		break;
			

		case Player_State.SHOOTING:
			
			if (animation.IsPlaying("tiro") == false)
				state = Player_State.MOVE_AUTOMATIC;

			
			if (animation["tiro"].normalizedTime > 0.2f && sphere.owner == this.gameObject) {
				state = Player_State.MOVE_AUTOMATIC;
				sphere.owner = null;
				if ( gameObject.tag == "PlayerTeam1" ) {
					sphere.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x*30.0f, 5.0f, transform.forward.z*30.0f );
				    barPosition = 0;
				}
				else {
				
					float valueRndY = UnityEngine.Random.Range( 4.0f, 10.0f );
					sphere.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x*30.0f, valueRndY, transform.forward.z*30.0f );
				}
				
			}
		break;
			
		case Player_State.PASSING:

			if (animation.IsPlaying("pase") == false)
				state = Player_State.MOVE_AUTOMATIC;
	
				
			if (animation["pase"].normalizedTime > 0.3f && sphere.owner == this.gameObject) {
				sphere.owner = null;
								
				GameObject bestCandidatePlayer = null;
				float bestCandidateCoord = 1000.0f;
				
				
				if ( gameObject.tag == "PlayerTeam1" ) {
				
					foreach ( GameObject go in players ) {
						
						if ( go != gameObject ) {
							Vector3 relativePos = transform.InverseTransformPoint( new Vector3( go.transform.position.x, go.transform.position.y, go.transform.position.z  ) );
									
							float magnitude = relativePos.magnitude;
							float direction = Mathf.Abs(relativePos.x);
							
							if ( relativePos.z > 0.0f && direction < 5.0f && magnitude < 15.0f && (direction < bestCandidateCoord) ) {
								bestCandidateCoord = direction;
								bestCandidatePlayer = go;
								
							}
						}
							
					}
				
				} else {
				
					foreach ( GameObject go in oponents ) {
						
						if ( go != gameObject ) {
							Vector3 relativePos = transform.InverseTransformPoint( new Vector3( go.transform.position.x, go.transform.position.y, go.transform.position.z  ) );
									
							float magnitude = relativePos.magnitude;
							float direction = Mathf.Abs(relativePos.x);
							
							if ( relativePos.z > 0.0f && direction < 15.0f && (magnitude+direction < bestCandidateCoord) ) {
								bestCandidateCoord = magnitude+direction;
								bestCandidatePlayer = go;		
							}
					
						}
							
					}
					
				}
				
				
				
					
				if ( bestCandidateCoord != 1000.0f ) {
				
					sphere.inputPlayer = bestCandidatePlayer;
					Vector3 directionBall = (bestCandidatePlayer.transform.position - transform.position).normalized;
					float distanceBall = (bestCandidatePlayer.transform.position - transform.position).magnitude*1.4f;
					distanceBall = Mathf.Clamp( distanceBall, 15.0f, 40.0f );
					sphere.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(directionBall.x*distanceBall, distanceBall/5.0f, directionBall.z*distanceBall );
				
				} else {
					// if not found a candidate just throw the ball forward....
					sphere.gameObject.GetComponent<Rigidbody>().velocity = transform.forward*20.0f;
					
				}
	
			
			
			}
			break;
 		case Player_State.GO_ORIGIN:
			
			animation.Play("corriendo");
			// now we just find the relative position of the waypoint from the car transform,
			// that way we can determine how far to the left and right the waypoint is.
			Vector3 RelativeWaypointPosition = transform.InverseTransformPoint(new Vector3( 
														initialPosition.x, 
														initialPosition.y, 
														initialPosition.z ) );
	
			// by dividing the horizontal position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
			inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;

			if ( inputSteer == 0 && RelativeWaypointPosition.z < 0 )
				inputSteer = 10.0f;
			
			transform.Rotate(0, inputSteer*10.0f , 0);
			float staminaTemp = Mathf.Clamp ((stamina/STAMINA_DIVIDER), STAMINA_MIN ,STAMINA_MAX );
			transform.position += transform.forward*3.0f*Time.deltaTime*staminaTemp*Speed;			transform.position += transform.forward*3.0f*Time.deltaTime;

			if ( RelativeWaypointPosition.magnitude < 1.0f ) {
				state = Player_State.MOVE_AUTOMATIC;					
			}
				
 							
		break;

		case Player_State.MOVE_AUTOMATIC:
			
			timeToRemove += Time.deltaTime;				
			float distance = (transform.position - initialPosition).magnitude;
			
			// know the distance of ball and player	
			float distanceBall = (transform.position - sphere.transform.position).magnitude;
			
			// if we get out of bounds of our player we come back to initial position
			if ( distance > maxDistanceFromPosition ) {
			
				Vector3 ball = sphere.transform.position;
				Vector3 direction = (ball - transform.position).normalized;
				
				Vector3 RelativeWaypointP = transform.InverseTransformPoint(new Vector3( 
															initialPosition.x, 
															initialPosition.y, 
															initialPosition.z ) );

				
				inputSteer = RelativeWaypointP.x / RelativeWaypointP.magnitude;
					
		
				if ( inputSteer == 0 && RelativeWaypointP.z < 0 )
					inputSteer = 10.0f;
					
				transform.Rotate(0, inputSteer*20.0f , 0);
				animation.Play("corriendo");
				float staminaTemp2 = Mathf.Clamp ((stamina/STAMINA_DIVIDER), STAMINA_MIN ,STAMINA_MAX );
				transform.position += transform.forward*5.5f*Time.deltaTime*staminaTemp2*Speed;
									
			} // if not we go to Ball...
			else {
		
				Vector3 ball = sphere.transform.position;
				Vector3 direction = (ball - transform.position).normalized;
				Vector3 posFinal = initialPosition + ( direction * maxDistanceFromPosition ); 
				
				Vector3 RelativeWaypointP = new Vector3(posFinal.x, posFinal.y, posFinal.z);
				
				// go to Ball position....
				if ( distanceBall > 5.0f ) {
					RelativeWaypointP = transform.InverseTransformPoint(new Vector3( 
																posFinal.x, 
																posFinal.y, 
																posFinal.z ) );

				} else if ( distanceBall < 5.0f && distanceBall > 2.0f ) {
				
					// if we are less than 5 meters of ball we stop
					RelativeWaypointP = transform.InverseTransformPoint(new Vector3( 
																transform.position.x, 
																transform.position.y, 
																transform.position.z ) );
	
				// if we are too close we go back with special animation
				} else if ( distanceBall < 2.0f ) {
					
					animation.Play("retroceder_bucle");
					state = Player_State.ONE_STEP_BACK;
					break;
					
				}
				
				inputSteer = RelativeWaypointP.x / RelativeWaypointP.magnitude;
	
				if ( inputSteer == 0 && RelativeWaypointP.z < 0 )
					inputSteer = 10.0f;

				if ( inputSteer > 0.0f )
					transform.Rotate(0, inputSteer*20.0f , 0);
				
			
				// this just checks if the player's position is near enough.
				if ( RelativeWaypointP.magnitude < 1.5f ) {
										
					transform.LookAt( new Vector3( sphere.GetComponent<Transform>().position.x, transform.position.y ,sphere.GetComponent<Transform>().position.z)  );
					animation.Play("reposo");		
					timeToRemove = 0.0f;
					
				}	else {			

					
					if ( timeToRemove > 1.0f ) {					
						animation.Play("corriendo");
						staminaTemp = Mathf.Clamp ((stamina/STAMINA_DIVIDER), STAMINA_MIN , STAMINA_MAX );
						transform.position += transform.forward*5.5f*Time.deltaTime*staminaTemp*Speed;
					}
				}
		
				
			}
			
		break;

			
 
 		case Player_State.RESTING:

			transform.LookAt( new Vector3( sphere.GetComponent<Transform>().position.x, transform.position.y ,sphere.GetComponent<Transform>().position.z)  );
			animation.Play("reposo"); 		  
 		
 		break;
			
 
			
			
		case Player_State.ONE_STEP_BACK:
		
			if (animation.IsPlaying("retroceder_bucle") == false)
				state = Player_State.MOVE_AUTOMATIC;

			transform.position -= transform.forward*Time.deltaTime*4.0f;	
			
		break;
			
			
		case Player_State.STOLE_BALL:
			
			Vector3 relPos = transform.InverseTransformPoint( sphere.transform.position );
			inputSteer = relPos.x / relPos.magnitude;
			transform.Rotate(0, inputSteer*20.0f , 0);
			
			animation.Play("corriendo");
			float staminaTemp3 = Mathf.Clamp ((stamina/STAMINA_DIVIDER), STAMINA_MIN ,STAMINA_MAX );
			transform.position += transform.forward*4.5f*Time.deltaTime*staminaTemp3*Speed;
			
			
		break;
			
			
	};

		
	// after pass or shoot player get in a Unselectable state some little time
	timeToBeSelectable -= Time.deltaTime;
			
	if ( timeToBeSelectable < 0.0f )
		temporallyUnselectable = false;
	else
		temporallyUnselectable = true;

}
	
	
	void OnCollisionStay( Collision coll ) {
	
		
		
		if ( coll.collider.transform.gameObject.tag == "Ball" && !gameObject.GetComponent<Player_Script>().temporallyUnselectable ) {
			
			
			inGame.lastTouched = gameObject;
		
			

			// are we stoling ball to oponent?
			if ( sphere.owner && (sphere.owner.tag != gameObject.tag) ) {
				
				
				
				if ( (gameObject.tag == "PlayerTeam1" && sphere.bPassButton) || gameObject.tag == "OponentTeam" ) {
				
					// comparacion entre fuerza de ambos jugadores...
					
					float myStrong = (this.Strong + UnityEngine.Random.Range(0.0f, 1.0f));
					float oponentStrong = (sphere.owner.GetComponent<Player_Script>().Strong + UnityEngine.Random.Range(0.0f, 1.0f))/* + 10.0f*/;
					if ( myStrong < oponentStrong  ) {
					
						Debug.Log("regate ganado por jugador de POSESION  " + myStrong + " vs " + oponentStrong );
						
						this.gameObject.animation.Play("retroceder_bucle");
						this.gameObject.GetComponent<Player_Script>().state = Player_State.ONE_STEP_BACK;
						this.gameObject.GetComponent<Player_Script>().timeToBeSelectable = 0.5f;
						this.gameObject.GetComponent<Player_Script>().temporallyUnselectable = true;
						
						return;
					} else {

						Debug.Log("regate ganado por jugador de ENTRADA " + myStrong + " vs " + oponentStrong);
						
					}					sphere.owner.GetComponent<Player_Script>().state = Player_State.ONE_STEP_BACK;
					sphere.owner.GetComponent<Player_Script>().timeToBeSelectable = 2.0f;
					sphere.owner.GetComponent<Player_Script>().temporallyUnselectable = true;
					sphere.owner = gameObject;
					sphere.owner.GetComponent<Player_Script>().state = Player_State.PICK_BALL;
					sphere.owner.animation.Play("entrada");	
					
					return;
				} else {
				
					
					return;
					
				}
				
			}
			
			
			Vector3 relativePos = transform.InverseTransformPoint( sphere.gameObject.transform.position );
			
					
			// only "glue" the ball to player if the collision is at bottom
			if ( relativePos.y < 0.35f ) { 
			
				coll.rigidbody.rotation = Quaternion.identity;
				GameObject ball = coll.collider.transform.gameObject;
				ball.GetComponent<Sphere>().owner = gameObject;
				
				if ( gameObject.tag == "OponentTeam" ) {
					state = Player_Script.Player_State.OPONENT_ATTACK;
				}
				
				
			}
		}
		
	}
			
	void OnGUI() {
	
		
			if ( sphere.timeShootButtonPressed > 0.0f && sphere.inputPlayer == this.gameObject) {
				
				Vector3 posBar = Camera.main.WorldToScreenPoint( headTransform.position + new Vector3(0,0.8f,0) );
				GUI.DrawTexture( new Rect( posBar.x-30, (Screen.height-posBar.y), barPosition, 10 ), barTexture );
				
				barPosition = (int)(sphere.timeShootButtonPressed * 128.0f);
				if ( barPosition >= 63 )
					barPosition = 63;
				
			}
			
			
			
			if ( sphere.owner == this.gameObject ) {
			
				Vector3 posBar = Camera.main.WorldToScreenPoint( headTransform.position + new Vector3(0,1.0f,0) );
				GUI.DrawTexture( new Rect( posBar.x-30, (Screen.height-posBar.y), (int)stamina, 10 ), barStaminaTexture );
				stamina -= 1.5f * Time.deltaTime;
				
			}
		
			
		
		}
	
	}
	
