using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Door : MonoBehaviour {
	
	public bool isActive; 
	public bool oneWaydoor;
	public Collider doorBlock;
	private DoorExit doorExit; 
	
	public bool opened; 
	private enum AnimStates{off, between, on};
	private AnimStates spriteState;
	private AnimStates lastSpriteState;
	
	public int spriteTilesX = 3;
	public int spriteTilesY = 1;
	private Vector2 scale;
	private Renderer myRenderer;
	
	public float fps;
	private int lastIndex=0;
	private int iX=1;
	private int iY=1;
	
	private bool lastActive;

	
	
	
	void Start()
	{
		doorExit = GetComponentInChildren<DoorExit>(); 
		setupSprite();
		if(opened){
			lastSpriteState = AnimStates.on;
			spriteState = AnimStates.on;
		}else{
			lastSpriteState = AnimStates.off;
			spriteState = AnimStates.off;
		}
	}
	
	void Update()
	{
		swapSpriteUsingIsActive();
		
		if(isActive && opened == false)
		{
			//renderer.material.color = Color.green;
			//Debug.Log("Door is active but closed");
		}
	}
	
	void swapSpriteUsingIsActive(){
		
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		
		
		if(isActive==true){
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
			//Debug.Log("index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            iX = (int) spriteState;
			Vector2 offset = new Vector2(iX* scale.x,
                                         1-(scale.y*iY));
			
            
            if(iX / spriteTilesX == 1)
            {
                iX=0;
            }
 			//Debug.Log("traps offset :"+offset);
			//Debug.Log("traps renderer :"+myRenderer);
            myRenderer.material.mainTextureOffset =  offset;
 
        }
            lastIndex = index;   
			lastSpriteState = spriteState;
	}
	void setupSprite(){
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);

		myRenderer = renderer;
 
        if(myRenderer == null) enabled = false;
 		
        myRenderer.material.mainTextureScale =  scale;
		//Debug.Log("Renderer = "+renderer+", Scale : "+scale);
		
	}
	void OnTriggerEnter(Collider Other)
	{
		lastActive = isActive; 
		openDoor();
	}
	void openDoor(){
		if(opened && isActive)
			{	
				opened = true;
				doorBlock.collider.enabled = false;
			}
			else if(opened == false && isActive)
			{
				opened = true;
				doorBlock.collider.enabled = false;
			}
			else if(isActive == false)
			{
				opened = false;
				doorBlock.collider.enabled = true;
			}
	}
	void OnTriggerStay(Collider Other){
		//Debug.Log("Stay : "+Other.tag);
		
		//if closed while the door is switched on and player is in trigger 
		if(lastActive!= isActive)
		{
		
			openDoor();
		}
		
		lastActive = isActive; 
	}
	void OnTriggerExit(Collider OtherObject)
	{
		//if the door is a one way and open and active
		// move down 
		if(oneWaydoor && opened && OtherObject.tag == "Player" && isActive && doorExit.exitTrigger == true)
		{	
			//renderer.material.color = Color.red;
			opened = false;
			
			doorBlock.collider.enabled = true;
			isActive = false;
		}
		else if(oneWaydoor && opened && OtherObject.tag == "Player" && isActive && doorExit.exitTrigger == false)
		{
			//renderer.material.color = Color.red;
			opened = false;
			
			doorBlock.collider.enabled = false;
		}
		//regular door and open 
		else if(oneWaydoor == false && opened && OtherObject.tag == "Player" && isActive)
		{ 
			opened = true;
			//renderer.material.color = Color.red;
			
			doorBlock.collider.enabled = false;
		}
		else if(isActive == false)
		{
			opened = false;
			//renderer.material.color = Color.red;
			
			doorBlock.collider.enabled = true;
		}
		
	}
	
	
}
