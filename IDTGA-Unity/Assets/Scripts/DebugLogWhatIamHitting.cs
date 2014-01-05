using UnityEngine;
using System.Collections;

public class DebugLogWhatIamHitting : MonoBehaviour {
	
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnCollisionEnter(Collision Other)
	{
		Debug.Log(Other.gameObject.tag);
	}
}
