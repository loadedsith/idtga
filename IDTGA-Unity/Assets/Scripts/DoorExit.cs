using UnityEngine;
using System.Collections;

public class DoorExit : MonoBehaviour {
	
	public bool exitTrigger; 

	void Start()
	{
		exitTrigger = false; 
	}
	void OnTriggerExit(Collider otherObject)
	{
		if(otherObject.tag == "Player")
		{
			exitTrigger = true;
		}
	}
}
