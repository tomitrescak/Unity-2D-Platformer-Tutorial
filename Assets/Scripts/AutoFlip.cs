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
