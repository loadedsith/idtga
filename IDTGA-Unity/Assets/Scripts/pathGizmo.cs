using UnityEngine;
using System.Collections;

public class pathGizmo : MonoBehaviour {
	
	float outerRadious;
	
	
	// Use this for initialization
	void OnDrawGizmos(){
	outerRadious = transform.localScale.x;
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position,outerRadious);
	}

}
