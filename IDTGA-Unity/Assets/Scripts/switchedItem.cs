using UnityEngine;
using System.Collections;

public class switchedItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("removeMe", 5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void removeMe(){
		Destroy(this.gameObject);	
	}
}
