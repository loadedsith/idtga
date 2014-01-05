using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergySpout : MonoBehaviour {
	//TODO: Reconfigure to take a list of orbSpawnNodes, using gizmos like the pathnodes
	public float orbsDespenced = 0;
	public float orbsDespencedAtATime = 1;
	public bool unlimitedOrbs = false;
	private float maxOrbs;
	
	public bool isActive = false;
	public bool isTimed = true;
	public bool isAlter = true;
	private bool isAdjacent = false;
	
	public GameObject orb;
	
	public List<GameObject> orbs = new List<GameObject>();
	private int orbLayoutIndex = 0;
	
	public int spriteTilesX = 8;
	public int spriteTilesY = 1;
	private Vector2 scale;
	private Renderer myRenderer;
	
	public float fps;
	private int lastIndex=0;
	private int iX=2;
	private int iY=1;
	private int lastIX=1;
	
	
	// Use this for initialization
	void Start () {
		setupSprite();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isAlter == true){
			if(isAdjacent == true){
				isActive = true;
			}
		}
		
		//Animate the spout
		
		//TODO: Sprites for the animation.
		
		swapSpriteUsingOrbCount();
		
		if(isActive == true){
			
			placeOrb();
			
			isActive= false;
			isAdjacent=false;
		}
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
	void swapSpriteUsingOrbCount(){
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		maxOrbs = orbs.Count;
		float orbRatio= (maxOrbs - orbsDespenced)/maxOrbs;
		
	
		if(unlimitedOrbs)
		{
			
	
			orbRatio=1;
			iX = spriteTilesX;
		}
	//	Debug.Log("iX: "+iX+", Orb ratio: "+orbRatio+", max-orbsDespenced: "+ (maxOrbs-orbsDespenced) +", orbs.Count : "+orbs.Count);
	//	if(iX != lastIX){
		
	        if(index != lastIndex)
	        {
				//Debug.Log("EnergySpout: iX : "+iX+", index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
				
	            //Vector2 offset = new Vector2(iX* scale.x, 1-(scale.y*iY));
				Vector2 offset = new Vector2(iX* scale.x, 1-(scale.y*iY));
				
				if(iX<(int)orbRatio*spriteTilesX-1){
					iX++;
				}else if(iX>(int)orbRatio*spriteTilesX){
					iX--;	
				}
	            
	 			//Debug.Log("EnergySpout: offset :"+offset);
				
	            myRenderer.material.mainTextureOffset =  offset;
	 
	        }
			   
	//	}
    	lastIndex = index;
		lastIX = iX;
	}
	public void placeOrb(){
		
		bool placeOrbs = true;
		if(orbsDespenced+orbsDespencedAtATime <= orbs.Count && unlimitedOrbs == false){
			orbsDespenced += orbsDespencedAtATime;
			for(int i = 0; i<orbsDespencedAtATime;i++){
			
				GameObject theOrb = orbs[orbLayoutIndex%orbs.Count];
				orbLayoutIndex++;
				Instantiate(orb, theOrb.transform.position , transform.rotation);
				
			}
			
		}else if(unlimitedOrbs == true)
		{
			orbsDespenced += orbsDespencedAtATime;
			for(int i = 0; i<orbsDespencedAtATime;i++){
			
				GameObject theOrb = orbs[orbLayoutIndex%orbs.Count];
				orbLayoutIndex++;
				Instantiate(orb, theOrb.transform.position , transform.rotation);
				
			}
		}
		else{
			placeOrbs = false;
		}

		//TODO finish this placement script
		
	}
	public void SetToActive(){
	
		if(isTimed == true){
			if(isActive == false){
				isActive= true;	
			}
			
		}else{
			isActive= !isActive;
		}
		
	}
	void OnTriggerEnter(Collider inputCollider){
		if(inputCollider.tag == "Player")
		{
			isAdjacent = true;
			
		}
	}
	void OnTriggerExit(Collider inputCollider){
		if(inputCollider.tag == "Player")
		{
			isAdjacent = false;
		}
		
	}
}
