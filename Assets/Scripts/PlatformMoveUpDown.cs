using UnityEngine;
using System.Collections;

public class PlatformMoveUpDown : MonoBehaviour {
	
	public float verticalSpan = 2;
	public float speed = 1;
	
	private bool goingUp = true;
	private Vector3 bottomPosition;
	private Vector3 upPosition;
	private Transform trans;
	
	private Rigidbody2D r2d;
	
	// Use this for initialization
	void Start () {
		// remember original position
		bottomPosition = transform.position;
		
		// remember up position
		upPosition = new Vector3(
			transform.position.x,
			transform.position.y + verticalSpan,
			transform.position.z
		);
		
		trans = this.transform;
		r2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (goingUp) {
			r2d.MovePosition(new Vector2(trans.position.x, trans.position.y + Time.deltaTime * speed));
			
			if (trans.position.y > upPosition.y) {
				goingUp = false;
				
			}	
		} else {
			r2d.MovePosition(new Vector2(trans.position.x, trans.position.y - Time.deltaTime * speed));
			
			
			if (trans.position.y < bottomPosition.y) {
				goingUp = true;
				
			}
		}
	}
}
