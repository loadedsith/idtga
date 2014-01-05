using UnityEngine;
using System.Collections;

public class OrbGizmo : MonoBehaviour {
	
	float outerRadious;

	
	
	// Use this for initialization
	void OnDrawGizmos(){
		outerRadious = transform.localScale.x;
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (transform.position,outerRadious);

	}

}
