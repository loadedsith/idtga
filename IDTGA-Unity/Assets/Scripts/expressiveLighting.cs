using UnityEngine;
using System.Collections;

public class expressiveLighting : MonoBehaviour {
	
	
	private Light proximityLight;
	private Light poiLight;
	private Light playerOneLight;
	private Light playerTwoLight;
	public GameObject playerOne;
	public GameObject playerTwo;
	public float minProximity = 1;
	public float maxProximity = 1;
	public float minProximityRange = 5f;
	public float maxProximityRange = 25f;
	public bool playerOneSpotLight = true;
	public bool playerTwoSpotLight = true;
	public bool isDirectional = false;
	public bool spotsMoveWithPlayers=true;
	
	// Use this for initialization
	void Start () {
		//Light[] lights = );
		//FindObjectOfType<GameObject>();
		foreach(Light aLight in GetComponentsInChildren(typeof(Light))){
			switch(aLight.gameObject.name){
			case "PlayerLightOne":
				playerOneLight = aLight;
					break;
				
			case "PlayerLightTwo":
				playerTwoLight = aLight;
					break;
				
			case "ProximityLight":
					proximityLight = aLight;
					break;
				
			case "POILight":
					poiLight = aLight;
					break;
				
			default:
				
				break;
				
				
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(proximityLight);
		if(isDirectional == true){
			proximityLight.type = LightType.Directional;
			proximityLight.intensity = .8f;
		}
		//proximityLight.light.spotAngle = 10;
		Vector3 averagePosition = (playerOne.transform.position + playerTwo.transform.position) / 2;
		
		proximityLight.transform.LookAt( averagePosition);
		
		
		Vector3 proximityLightPos = new Vector3(averagePosition.x,25,averagePosition.z);
		proximityLight.transform.position = proximityLightPos;
		
		float playerDistance = Vector3.Distance( playerOne.transform.position, playerTwo.transform.position);
		
		
		//Debug.Log("Light Intensity = "+Remap(playerDistance,50f, 0f, 1f)+ " = "+playerDistance);
		
		proximityLight.light.intensity =  (float)Remap(playerDistance, maxProximityRange, minProximityRange, maxProximity, minProximity);
		
		//proximityLight.light.intensity = (float) 1-playerDistance/100f/.5f;
		proximityLight.light.spotAngle = Remap(playerDistance,50f, 0f, 140f, 80f);
		
		
		playerOneLight.intensity = Remap(playerDistance,maxProximityRange, minProximityRange, .8f, 0f);
		playerTwoLight.intensity = Remap(playerDistance,maxProximityRange, minProximityRange, .8f, 0f);
		playerOneLight.spotAngle = Remap(playerDistance,maxProximityRange, minProximityRange, 40f, 50f);
		playerTwoLight.spotAngle = Remap(playerDistance,maxProximityRange, minProximityRange, 40f, 50f);
		//playerOneLight.transform.( playerOne.transform.position);
		if(playerOneSpotLight ==true){
			playerOneLight.enabled = true;
			Vector3 playerOnePos = new Vector3(playerOne.transform.position.x,playerOne.transform.position.y+3,playerOne.transform.position.z);
			
			playerOneLight.transform.LookAt( playerOnePos);
			if(spotsMoveWithPlayers==true){
				Vector3 playerOneLightPos = new Vector3(playerOne.transform.position.x,20,playerOne.transform.position.z);
				playerOneLight.transform.position = playerOneLightPos;
			}
		}else{
			playerOneLight.enabled = false;
		}
		
		if(playerTwoSpotLight ==true){
			playerTwoLight.enabled =true;
			Vector3 playerTwoPos = new Vector3(playerTwo.transform.position.x,playerTwo.transform.position.y+3,playerTwo.transform.position.z);
			playerTwoLight.transform.LookAt(playerTwoPos);
			if(spotsMoveWithPlayers==true){
				Vector3 playerTwoLightPos = new Vector3(playerTwo.transform.position.x,20,playerTwo.transform.position.z);
				playerTwoLight.transform.position = playerTwoLightPos;
			}
		}else{
			playerTwoLight.enabled =false;
		}
	}
	
	 float Remap ( float value, float from1, float to1, float from2, float to2) {

    	return (value - from1) / (to1 - from1) * (to2 - from2) + from2;

	}
}
