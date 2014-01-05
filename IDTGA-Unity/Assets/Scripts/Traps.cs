using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Traps : MonoBehaviour {
	
	public bool isActive;
	public bool isTimed;
	public string enemy1Tag, enemy2Tag, enemy3Tag, enemy4Tag;
	public float timeToTurnOff, timeToTurnOn;
	public ParticleSystem trapPS; 
	
	public List<GameObject> inTrapList = new List<GameObject>();
	
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
	
	private bool doOnce; 
	public enum EnemyTypes
	{
		Normal,
		Ordered,
		Linked,
		Gemmed,
	};
	
	//traps 
	//fire, spikes, darts, gas 
	
	void Start() 
	{
		setupSprite();
	}

	
	void Update () 
	{
		if(isActive)
		{
			 StartCoroutine(ActiveTrap(timeToTurnOff, timeToTurnOn));
		}
		else
		{
			trapPS.Stop();
			doOnce = false;
		}
		swapSpriteUsingIsActive();
	}
	void setupSprite(){
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);

        Transform aTransform = transform.Find("SpriteHead");
		myRenderer = aTransform.GetComponentInChildren<Renderer>();
 
        if(myRenderer == null) enabled = false;
 		
        myRenderer.material.mainTextureScale =  scale;
		//Debug.Log("Renderer = "+renderer+", Scale : "+scale);
		
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
			//Debug.Log("index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            Vector2 offset = new Vector2(iX* scale.x,
                                         1-(scale.y*iY));
			
            iX = (int) spriteState;
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

	IEnumerator ActiveTrap (float waitTimeOff, float waitTimeOn)
	{
		//if trap is not on, turn it on  
		if(trapPS.isPlaying == false)
		{
			yield return new WaitForSeconds(waitTimeOn);
			trapPS.Play();
		}	
		 
		
		if(trapPS.isPlaying)
		{
			
			if(doOnce == false)
			{
				yield return new WaitForSeconds(1f);
				doOnce = true; 
			}
			
			for(int i = 0; i < inTrapList.Count; i++)
			{
				if(inTrapList[i] !=null){
					if(inTrapList[i].tag == enemy1Tag || inTrapList[i].tag == enemy2Tag || inTrapList[i].tag == enemy3Tag || inTrapList[i].tag == enemy4Tag && inTrapList[i] != null)
					{
							inTrapList[i].GetComponent<Enemy>().EnemyDestruction();
					}
					else if (inTrapList[i].tag == "Player")
					{
						if(inTrapList[i].GetComponent<Player>().state != Player.States.Invincible)
						{	
							inTrapList[i].GetComponent<Player>().state = Player.States.Dead;
							inTrapList[i].GetComponent<Player>().OnDeath();
						}
					}
				}
				
			}
		}
			
		
		if(isTimed)
		{	
			yield return new WaitForSeconds(waitTimeOff);
		
			trapPS.Stop();
		
			isActive = false;
			doOnce = false; 
		}
	}
	
	
	
	
	void OnTriggerEnter(Collider otherObject)
	{
		Debug.Log(otherObject.tag);
		if(otherObject.tag == enemy1Tag || otherObject.tag == enemy2Tag || otherObject.tag == enemy3Tag || otherObject.tag == enemy4Tag || otherObject.tag =="Player")
		{
			inTrapList.Add(otherObject.gameObject); 
		}
		
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == enemy1Tag || other.tag == enemy2Tag || other.tag == enemy3Tag || other.tag == enemy4Tag || other.tag =="Player")
		{
			inTrapList.Remove(other.gameObject); 
		}
	
	}
	
	public void SetToActive()
	{
		if(isTimed)
		{
			if(isActive == false)
			{
				isActive = true;
			}
		}
		else
		{
			isActive = !isActive;
		}
		
	}
	
}
