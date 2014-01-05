using UnityEngine;
using System.Collections;

public class Switchable : MonoBehaviour {
	public bool isActive = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void activateSwitch(){
		isActive = true;	
	}
}
