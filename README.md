# Unity 2D Platformer Tutorial

Watch and learn how to create a simple 2D platformer in Unity 5.0

# Instructions

1. Download Files
2. Add Standard Assets -> 2D
3. Open scene "Default"
4. Watch instruction videos from the link below

<a href="https://www.youtube.com/playlist?list=PLo35hhxbk9J6xOECWUQbVainn5kCJQNbt" 
target="_blank"><img src="http://img.youtube.com/vi/3J-U-ATy4gY/0.jpg" 
alt="Tutorial Video" width="240" height="180" border="10" /></a>

https://www.youtube.com/playlist?list=PLo35hhxbk9J6xOECWUQbVainn5kCJQNbt

Enjoy!

Dr. LLama

# Scripts

Level Manager

```csharp
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelManager: MonoBehaviour {

	public enum Orientation {
		XY,
		XZ
	}

	// holds the definition of a tile
	[Serializable]
	public class Tile {
		// id of the tile, should be a single letter
		public char id;
		// reference to the tile
		public GameObject tile;
	}
	
	public Orientation orientation;
	public List<Tile> tiles;
	public string startLevel;

	void Start() {
		if (!string.IsNullOrEmpty (startLevel)) {
			this.LoadWorld(startLevel);
		}
	}

	public void LoadWorld(string definitionFile) {
		// load file from the resource folder (Assets/Resources)
		// the file has text format
		var world = Resources.Load<TextAsset> ("Levels/" + definitionFile);

		// the structure of the file is
		// 1.......2........
		// ......1111......4

		var worldText = world.text;
		var worldLines = world.text.Split('\n');

		// browse all lines and instantiate objects at desired positions
		for (var reverseRowIndex=worldLines.Length - 1; reverseRowIndex>=0; reverseRowIndex--) {
			var line = worldLines[reverseRowIndex];
			var rowIndex = worldLines.Length - reverseRowIndex;
			
			for (var columnIndex=0; columnIndex<line.Length; columnIndex++) {
				// assign tileId
				var tileId = line[columnIndex];

				// if it is an empty space, move on
				if (tileId == '.' || tileId == ' ') {
					continue;
				}
				// find the tile
				var tile = this.tiles.Find(w => w.id == tileId);

				// if tile does not exists, notify developer
				if (tile == null) {
					Debug.LogErrorFormat("Tile with id '{0}' does not exists!", tileId);
				}

				// instantiate tile at a given position according to orientation
				if (this.orientation == Orientation.XY) {
					Instantiate(tile.tile, new Vector3(columnIndex, rowIndex, 0), Quaternion.identity);
				} else if (this.orientation == Orientation.XZ) {
					Instantiate(tile.tile, new Vector3(columnIndex, 0, rowIndex), Quaternion.identity);
				}
			
			}
		}
		
		// make camera follow player
		var follow = Camera.main.GetComponent<SmoothFollow2D>();
		follow.target = GameObject.FindGameObjectWithTag("Player").transform;

	}

}
```

SmoothFollow2D

```csharp
using UnityEngine;


public class SmoothFollow2D : MonoBehaviour
{
	
	// The target we are following
	[SerializeField]
	public Transform target;
	// the height we want the camera to be above the target
	[SerializeField]
	private float height = 0.0f;
	
	[SerializeField]
	private float moveDamping = 1;
	
	private Transform trans;
	
	// Use this for initialization
	void Start() { 
		this.trans = transform;
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		// Early out if we don't have a target
		if (!target)
			return;
		
		// Calculate the current rotation angles
		var wantedHeight = target.position.y + height;
		
		// Damp the height
		trans.position = Vector3.Lerp(trans.position, new Vector3(target.position.x, wantedHeight, trans.position.z), moveDamping * Time.deltaTime);
	}
}
```

```csharp
using UnityEngine;
using System.Collections;

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
			jump = Input.GetButtonDown("Jump");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxis("Horizontal");
		
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
```

```csharp
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
```

```csharp
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
```

```csharp
using UnityEngine;
using System.Collections;

public class AutoFlip : MonoBehaviour {

	float lastX;
	
	// initialise
	void Start() {
		lastX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		
		// going left
		if (lastX > transform.position.x) {
			if (transform.localScale.x != 1) {
				transform.localScale = new Vector3(1f, 1f, 1f);
			}
			
		} 
		// going right
		else {
			
			if (transform.localScale.x != -1) {
				transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		lastX = transform.position.x;
	}
}
```

