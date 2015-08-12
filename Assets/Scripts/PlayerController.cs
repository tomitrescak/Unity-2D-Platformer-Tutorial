using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	
	public float maximumSpeed = 10f;

	Rigidbody2D r2d;
	
	// Use this for initialization
	void Start () {
		r2d = GetComponent<Rigidbody2D> ();
	}
	
	private void Update()
	{
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		
        // Pass all parameters to the character control script.
		Move(h);
	}
	
	void Move(float move) {
		
		r2d.velocity = new Vector2 (
			move * maximumSpeed, 
			r2d.velocity.y);
			
		// AUTO FLIP
		if (move > 0) {
			if (transform.localScale.x != 1) {
				transform.localScale = new Vector3(1f, 1f, 1f);
			}
		} 
		// going right
		else if (move < 0) {
			if (transform.localScale.x != -1) {
				transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}		
	}
}
