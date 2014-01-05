using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Switch : MonoBehaviour {
	
	public GameObject switchedObject;
	public GameObject switchedObject2;
	public GameObject switchedObject3;
	public GameObject switchedObject4;
	public GameObject switchedObject5;
	public GameObject switchedObject6;
	public GameObject switchedObject7;
	public GameObject switchedObject8;
	public GameObject switchedObject9;
	public GameObject switchedObject10;
	public GameObject switchedObject11;
	public GameObject switchedObject12;
	
	public List<GameObject> objectControlledBySwitch = new List<GameObject>(); 
	
	public GameObject switchedObjectEffect;
	
    //private Switchable switchScript;
    public bool isActive=false;
    public bool isTimed=false;
    private float delay=0;
    private float timeScale=1;
	
	public float delayMax =3;
    
	private enum AnimStates{on, between, off};
	private AnimStates spriteState;
	
	
	public int spriteTilesX = 3;
	public int spriteTilesY = 1;
	private Vector2 scale;
	private Renderer myRenderer;
	
	public float fps;
	private int lastIndex=0;
	private int iX=3;
	private int iY=1;
	private AnimStates lastSpriteState;
	
	// Use this for initialization
    void Start (){
 		lastSpriteState = spriteState;
        setupSprite();
		
		if(switchedObject != null)
		{
			objectControlledBySwitch.Add(switchedObject); 
		}
		if(switchedObject2 != null)
		{
			objectControlledBySwitch.Add(switchedObject2);
		}
		if(switchedObject3 != null)
		{
			objectControlledBySwitch.Add(switchedObject3);
		}
		if(switchedObject4 != null)
		{
			objectControlledBySwitch.Add(switchedObject4);
		}
		if(switchedObject5 != null)
		{
			objectControlledBySwitch.Add(switchedObject5);
		}
		if(switchedObject6 != null)
		{
			objectControlledBySwitch.Add(switchedObject6);
		}
		if(switchedObject7 != null)
		{
			objectControlledBySwitch.Add(switchedObject7);
		}
		if(switchedObject8 != null)
		{
			objectControlledBySwitch.Add(switchedObject8);
		}
		if(switchedObject9 != null)
		{
			objectControlledBySwitch.Add(switchedObject9);
		}
		if(switchedObject10 != null)
		{
			objectControlledBySwitch.Add(switchedObject10);
		}
		if(switchedObject11 != null)
		{
			objectControlledBySwitch.Add(switchedObject11);
		}
		if(switchedObject12 != null)
		{
			objectControlledBySwitch.Add(switchedObject12);
		}
		
		
		
    }
    
    // Update is called once per frame
    void Update () {
		swapSpriteUsingIsActive();
        if(delay > 0){
            delay -= (Time.deltaTime * timeScale);
            Debug.Log("delay :" + delay);
        }
        if(isActive==true)
		{
		
		for(int i = 0; i < objectControlledBySwitch.Count; i++)
		{
			if(switchedObjectEffect != null){
					Instantiate(switchedObjectEffect, objectControlledBySwitch[i].transform.position, objectControlledBySwitch[i].transform.rotation);
				}
            switch(objectControlledBySwitch[i].tag){
                case "Trap":  
                  	objectControlledBySwitch[i].GetComponent<Traps>().SetToActive();

                    break;
                case "Door": 
                	//TODO impliment a SetToActive on the door script
					objectControlledBySwitch[i].GetComponent<Door>().isActive = !objectControlledBySwitch[i].GetComponent<Door>().isActive;
					Debug.Log("Switching Door");
					

                    break;
                case "EnergySpout":   
                  //  Debug.Log("activate energy spout");
                    objectControlledBySwitch[i].GetComponent<EnergySpout>().SetToActive();
                    break;

                }
            isActive=false;
		}
         
			
        }
    }
 	void removeThisLight(GameObject theLight){
		Destroy(theLight);	
	}
	void setupSprite(){
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);
 
        myRenderer = renderer;
 
        if(myRenderer == null) enabled = false;
 
        myRenderer.material.mainTextureScale =  scale;
		//Debug.Log("Scale = "+scale);
	}
	
	void swapSpriteUsingIsActive(){
		
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		int increment = 0;
		
		if(isActive==false){
				spriteState = AnimStates.on;
	        	//ON
			}else {
				spriteState = AnimStates.off;
	            //OFF
			}
		
		if(spriteState != lastSpriteState){
			//this forces an update if the state changes inbetween the fps... 
			iX=(int) AnimStates.between;
		}
		
        if(index != lastIndex)
        {
//			Debug.Log("index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            Vector2 offset = new Vector2(iX* scale.x,
                                         1-(scale.y*iY));
			
            iX = (int) spriteState;
            if(iX / spriteTilesX == 1)
            {
               
                iX=0;
                
            }
 			//Debug.Log("Switch offset :"+offset);
			//Debug.Log("Switch offset :"+myRenderer.material);
            myRenderer.material.mainTextureOffset =  offset;
 
 
 
        }
            lastIndex = index;   
			lastSpriteState = spriteState;
	}
    void OnTriggerStay(Collider inputCollider){
        //Debug.Log("Switch collider :"+ inputCollider.tag );
    }
    public void SetToActive(){
		 if(delay <= 0){
			Debug.Log(this.ToString() + "SetToActive :" );
			Debug.Log("Switch is switching");
			Camera.main.GetComponent<CameraManager>().ActivateOutwardZoom(objectControlledBySwitch);
	        if(isTimed == true){
	            if(isActive == false){
	                isActive= true; 
	            }
	            
	        }else{
	            isActive= true;
	        }
			
			delay=delayMax; 
		}
    }

}
