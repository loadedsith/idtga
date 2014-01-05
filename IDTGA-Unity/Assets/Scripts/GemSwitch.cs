using UnityEngine;
using System.Collections;

public class GemSwitch : MonoBehaviour {
	
	public float fps;
	private int lastIndex=0;
	private int iX=1;
	private int iY=1;
	public int spriteTilesX = 3;
	public int spriteTilesY = 1;
	private Vector2 scale;
	private Renderer myRenderer;
	
	private enum AnimStates{on, between, off};
	private AnimStates spriteState;
	private AnimStates lastSpriteState;
	
	static public int gemSwitchesHit;
	private bool _isActive = false;
	public bool isActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			if(_isActive != value)
			{
				_isActive = value;
				if(_isActive)
				{
					GemSwitch.gemSwitchesHit++;
					Debug.Log("Gem Switches Hit :" + GemSwitch.gemSwitchesHit);
					renderer.material.color = Color.green;
				}
				else
				{
					GemSwitch.gemSwitchesHit--;
					renderer.material.color = Color.red;
				}
			}
		}
		
	}
	void Start(){
		setupSprite();	
	}
	void Update(){
			swapSpriteUsingIsActive();
	}
	void setupSprite(){
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);

		myRenderer = renderer;
 
        if(myRenderer == null) enabled = false;
 		
        myRenderer.material.mainTextureScale =  scale;
		//Debug.Log("Renderer = "+renderer+", Scale : "+scale);
		
	}
	void OnLevelWasLoaded(int LevelLoaded)
	{
		gemSwitchesHit = 0;
		Enemy.enemyDeathCount = 0; 
	}
	
	void swapSpriteUsingIsActive(){
		
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		
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
}
