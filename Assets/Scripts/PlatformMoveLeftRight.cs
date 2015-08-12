using UnityEngine;
using System.Collections;

public class PlatformMoveLeftRight : MonoBehaviour {
	
	public float span = 2;
	public float speed = 1;
	
	private bool goingRight = true;
	private Vector3 leftPosition;
	private Vector3 rightPosition;
	private Transform trans;
	private Rigidbody2D r2d;
	
	// Use this for initialization
	void Start () {
		// remember original position
		leftPosition = transform.position;
		r2d = GetComponent<Rigidbody2D>();
		
		// remember up position
		rightPosition = new Vector3(
			transform.position.x + span,
			transform.position.y,
			transform.position.z
		);
		
		trans = this.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (goingRight) {
			//trans.Translate(Time.deltaTime * speed, 0f, 0f);
			
			r2d.MovePosition(new Vector2(trans.position.x + Time.deltaTime * speed, trans.position.y));
			if (trans.position.x > rightPosition.x) {
				goingRight = false;
				
			}	
		} else {
			
			r2d.MovePosition(new Vector2(trans.position.x - Time.deltaTime * speed, trans.position.y));
			//trans.Translate(- Time.deltaTime * speed, 0f, 0f);
			
			
			if (trans.position.x < leftPosition.x) {
				goingRight = true;
				
			}
		}
	}
}
