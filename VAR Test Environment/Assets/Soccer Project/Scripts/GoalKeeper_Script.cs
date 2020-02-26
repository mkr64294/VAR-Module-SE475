using UnityEngine;
using System.Collections;

public class GoalKeeper_Script : MonoBehaviour {

	public string Name;	
	
	public enum GoalKeeper_State { 
	   RESTING,
	   GO_ORIGIN,
	   STOLE_BALL,
	   GET_BALL_DOWN,
	   UP_WITH_BALL,
	   PASS_HAND,
	   SAQUE_PUERTA,
	   JUMP_LEFT,
	   JUMP_RIGHT,
	   JUMP_LEFT_RASO,
	   JUMP_RIGHT_RASO	  
	};
	
	public GoalKeeper_State state;
	public Transform centro_campo;
	public Sphere sphere;
	public Vector3 initial_Position;
	public Transform hand_bone;
	private float timeToSacar = 1.0f;
	public CapsuleCollider capsuleCollider;	
	// Use this for initialization
	void Start () {
	
		initial_Position = transform.position;
		state = GoalKeeper_State.RESTING;
		GetComponent<Animation>()["corriendo"].speed = 1.0f;
		
		GetComponent<Animation>()["portero_despeje_lateral_derecho_alto"].speed = 1.0f;
		GetComponent<Animation>()["portero_despeje_lateral_izquierdo_alto"].speed = 1.0f;
		GetComponent<Animation>()["portero_despeje_lateral_derecho_raso"].speed = 1.0f;
		GetComponent<Animation>()["portero_despeje_lateral_izquierdo_raso"].speed = 1.0f;
		
//		capsuleCollider = gameObject.GetComponent<CapsuleCollider>();	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		switch (state) {
	
			case GoalKeeper_State.JUMP_LEFT:
				
				
				capsuleCollider.direction = 0;
			
				if ( GetComponent<Animation>()["portero_despeje_lateral_izquierdo_alto"].normalizedTime < 0.45f  ) {
					transform.position -= transform.right * Time.deltaTime * 7.0f;
				}
			
			
				if ( !GetComponent<Animation>().IsPlaying("portero_despeje_lateral_izquierdo_alto") ) {
					state = GoalKeeper_State.STOLE_BALL;		
					capsuleCollider.direction = 1;

				}
			
			break;
	
			case GoalKeeper_State.JUMP_RIGHT:

				capsuleCollider.direction = 0;

				if ( GetComponent<Animation>()["portero_despeje_lateral_derecho_alto"].normalizedTime < 0.45f  ) {
					transform.position += transform.right * Time.deltaTime * 7.0f;
				}		
				if ( !GetComponent<Animation>().IsPlaying("portero_despeje_lateral_derecho_alto") ) {
					state = GoalKeeper_State.STOLE_BALL;		
					capsuleCollider.direction = 1;

				}
				
				
			break;
			
			case GoalKeeper_State.JUMP_LEFT_RASO:
				
				
				capsuleCollider.direction = 0;
			
				if ( GetComponent<Animation>()["portero_despeje_lateral_izquierdo_raso"].normalizedTime < 0.45f  ) {
					transform.position -= transform.right * Time.deltaTime * 4.0f;
				}
			
			
				if ( !GetComponent<Animation>().IsPlaying("portero_despeje_lateral_izquierdo_raso") ) {
					state = GoalKeeper_State.STOLE_BALL;		
					capsuleCollider.direction = 1;

				}
			
			break;
	
			case GoalKeeper_State.JUMP_RIGHT_RASO:

				capsuleCollider.direction = 0;

				if ( GetComponent<Animation>()["portero_despeje_lateral_derecho_raso"].normalizedTime < 0.45f  ) {
					transform.position += transform.right * Time.deltaTime * 4.0f;
				}		
				if ( !GetComponent<Animation>().IsPlaying("portero_despeje_lateral_derecho_raso") ) {
					state = GoalKeeper_State.STOLE_BALL;		
					capsuleCollider.direction = 1;

				}
				
				
			break;
						
						case GoalKeeper_State.SAQUE_PUERTA:
		
			break;			
			
			case GoalKeeper_State.PASS_HAND:
		
				if ( GetComponent<Animation>()["portero_saque_mano_alto"].normalizedTime < 0.65f && sphere.gameObject.GetComponent<Rigidbody>().isKinematic == true ) {
					sphere.gameObject.transform.position = hand_bone.position;
					sphere.gameObject.transform.rotation = hand_bone.rotation;
				}
		
				if ( GetComponent<Animation>()["portero_saque_mano_alto"].normalizedTime > 0.65f && sphere.gameObject.GetComponent<Rigidbody>().isKinematic == true ) { 
					sphere.gameObject.GetComponent<Rigidbody>().isKinematic = false;
					sphere.gameObject.GetComponent<Rigidbody>().AddForce( transform.forward*5000.0f + new Vector3(0.0f, 1300.0f, 0.0f) );
				}
		
				if ( !GetComponent<Animation>().IsPlaying("portero_saque_mano_alto") || !GetComponent<Animation>().IsPlaying("portero_saque_mano_alto") ) {
					state  = GoalKeeper_State.GO_ORIGIN;			
				}
			
			break;
			

			case GoalKeeper_State.UP_WITH_BALL:
			
			
				if ( !GetComponent<Animation>().IsPlaying("portero_levanta_balon") ) {
				
					sphere.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					sphere.gameObject.transform.position = hand_bone.position;
					sphere.gameObject.transform.rotation = hand_bone.rotation;
	
					timeToSacar -= Time.deltaTime;
					
					if ( timeToSacar < 0.0f ) {
						timeToSacar = UnityEngine.Random.Range( 2.0f, 5.0f );
						GetComponent<Animation>().Play("portero_saque_mano_alto");
						state = GoalKeeper_State.PASS_HAND;
					}
				
				
				} else {
				
					sphere.gameObject.transform.position = hand_bone.position;
					sphere.gameObject.transform.rotation = hand_bone.rotation;
				
/*				
					Vector3 relativeCenter = transform.InverseTransformPoint( centro_campo.position );
					if ( relativeCenter.x > 10 )
						transform.Rotate( 0, 10, 0);
					else if ( relativeCenter.x < -10 )
						transform.Rotate( 0, -10, 0);
*/
				
					transform.LookAt( centro_campo.position );
				
				
				}

			break;			
			
			case GoalKeeper_State.GET_BALL_DOWN:
			
				sphere.gameObject.transform.position = hand_bone.position;
				sphere.gameObject.transform.rotation = hand_bone.rotation;
				
				if ( !GetComponent<Animation>().IsPlaying("portero_parada_frontal_rasa") ) {
					GetComponent<Animation>().Play("portero_levanta_balon");
					state = GoalKeeper_State.UP_WITH_BALL;
				}
			
			break;
			
			
			case GoalKeeper_State.RESTING:
			
				capsuleCollider.direction = 1;
				if ( !GetComponent<Animation>().IsPlaying("portero_guardia_reposo") )
					GetComponent<Animation>().Play("portero_guardia_reposo");
				
				transform.LookAt( new Vector3( sphere.gameObject.transform.position.x, transform.position.y , sphere.gameObject.transform.position.z)  );
			
				float distanceBall = (transform.position - sphere.gameObject.transform.position).magnitude;
		
				if ( distanceBall < 10.0f ) {
					state = GoalKeeper_Script.GoalKeeper_State.STOLE_BALL;
				} 
			
			
			break;

			case GoalKeeper_State.STOLE_BALL:
				GetComponent<Animation>().Play("corriendo");

				Vector3 RelativeWaypointPosition = transform.InverseTransformPoint( sphere.gameObject.transform.position );
	
				float inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
			
				transform.Rotate(0, inputSteer*10.0f , 0);
				transform.position += transform.forward*6.0f*Time.deltaTime;

		
				if ( RelativeWaypointPosition.magnitude < 1.0f ) {
			
//					state = GoalKeeper_State.RESTING;					
				}

			break;
	
	
			case GoalKeeper_State.GO_ORIGIN:

				GetComponent<Animation>().Play("corriendo");
				RelativeWaypointPosition = transform.InverseTransformPoint( initial_Position );
	
				inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
			
				transform.Rotate(0, inputSteer*10.0f, 0);
				transform.position += transform.forward*6.0f*Time.deltaTime;

		
				if ( RelativeWaypointPosition.magnitude < 1.0f ) {
			
					state = GoalKeeper_State.RESTING;					
				}
		
			
			
			break;
			
			
			
		}
		
	}
	
	
	
	// to know if GoalKeeper is touching Ball
	void OnCollisionStay( Collision coll ) {
		
		if ( Camera.main.GetComponent<InGameState_Script>().state == InGameState_Script.InGameState.PLAYING ) {
		
			if ( coll.collider.transform.gameObject.tag == "Ball" && state != GoalKeeper_State.UP_WITH_BALL && state != GoalKeeper_State.PASS_HAND && state != GoalKeeper_State.SAQUE_PUERTA &&
				 state != GoalKeeper_State.JUMP_LEFT && state != GoalKeeper_State.JUMP_RIGHT &&
				 state != GoalKeeper_State.JUMP_LEFT_RASO && state != GoalKeeper_State.JUMP_RIGHT_RASO) {
							
				Camera.main.GetComponent<InGameState_Script>().lastTouched = gameObject;
			
	
				Vector3 relativePos = transform.InverseTransformPoint( sphere.gameObject.transform.position );
				
				// only get ball if the altitude is 0.35f (relative)
				if ( relativePos.y < 0.35f ) { 
				
					sphere.owner = null;
		
					GetComponent<Animation>().Play("portero_parada_frontal_rasa");
					state = GoalKeeper_State.GET_BALL_DOWN;
					
				}
				
				
			}
		
		}
		
	}
	
}
