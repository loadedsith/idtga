using UnityEngine;
using System.Collections;

public class OneWayGizmo : MonoBehaviour {
	
	float transX;
	float transY;
	float transZ;
	
	
	// Use this for initialization
	void OnDrawGizmos(){
		transX = transform.localScale.x;
		transY = transform.localScale.y;
		transZ = transform.localScale.z;
		Gizmos.color = Color.red;
		
		Gizmos.DrawWireCube (transform.position, new Vector3 (transX,transY,transZ));

	}

}
