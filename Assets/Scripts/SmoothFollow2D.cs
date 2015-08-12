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
