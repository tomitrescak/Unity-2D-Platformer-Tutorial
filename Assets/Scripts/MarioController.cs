using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MarioController : MonoBehaviour {

	public float maximumSpeed = 10f;
	public float jumpHeight = 3f;
	public LayerMask whatIsGround;  
	public float airControlHandicap = 0.5f;
	
	bool grounded = false;
	Rigidbody2D r2d;
	Animator anim;
	bool jump;
	
	// Use this for initialization
	void Start () {
		r2d = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren <Animator> ();
	}
	
	private void Update()
	{
		if (!jump)
		{
            // Read the jump input in Update so button presses aren't missed.
			jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		
		grounded = false;
		// find all colliders that are under player's feet
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				grounded = true;
		}
		
		anim.SetBool("Grounded", grounded);
		
        // Pass all parameters to the character control script.
		Move(h, jump);
		
		// set that we are not jumping anymore
		jump = false;
	}
	
	void Move(float move, bool jump) {
				
		// if we are not in air, we allow character to move
		if (grounded || airControlHandicap > 0) {
			
			// set velocity which player is moving
			// if player is in the air handicap is added

			r2d.velocity = new Vector2 (
				move * maximumSpeed * (grounded ? 1 : airControlHandicap), 
				r2d.velocity.y);

			anim.SetFloat ("Speed", Mathf.Abs (move));
			
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
		
		// If the player should jump...
		if (grounded && jump && anim.GetBool("Grounded"))
		{
            // Add a vertical force to the player.
			grounded = false;
			anim.SetBool("Grounded", false);
			r2d.AddForce(new Vector2(0f, jumpHeight));
		}
	}
	
	
}
