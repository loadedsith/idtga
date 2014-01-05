using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//TODO: Enemy death animations!
public class Enemy : MonoBehaviour {
	
	public enum EnemyTypes
	{
		Normal,
		Ordered,
		Linked,
		Gemmed,
	};
	//public enum Directions { East, West};
	public EnemyTypes type;

	//If this enemy is linked or ordered among a set of enemies:
	public List<GameObject> attachedEnemies = new List<GameObject>();
	private List<Enemy> attachedEnemyScripts = new List<Enemy>(); 
	
	public CharacterController charController;
	
	//to be set in the move code
	private Quaternion heading;
	private float headingAngle;
	
	public float fps;
	private int lastIndex=0;
	private int iX=1;
	private int iY=1;
	private int lastIY=-1;
	public int spriteTilesX = 8;
	public int spriteTilesY = 2;
	private Vector2 scale;
	private Renderer myRenderer;
	
	public float deathCheckDelay;
	public float deathCheckTimer;

	
	public bool alive;
	public bool waiting;
	
	public bool isWalkingPath = true;
	
	public float speed = 1;
	
	public enum EnemyStates {Alive, Dead};
	public EnemyStates state = EnemyStates.Alive;
	
	public List<GameObject> pathNodes;
	public CharacterController enemyController;
	private int pathIndex = 0;
	
	private float pathDelay=1;
    private float pathTimeScale=1;
	public float pathDelayDuration =1;
	
	private bool animatingDeath = true;
	
	private int iXDead=1;
	private int spriteTilesXDeath=8;
	
	static public int enemyDeathCount = 0; 
	
	public ParticleSystem poof; 
	
	// Use this for initialization
	void Start ()
	{
		alive = true;
		
		heading = Quaternion.LookRotation( Vector3.Normalize(pathNodes[1].transform.position - transform.position), Vector3.up);
		headingAngle = Mathf.LerpAngle( transform.eulerAngles.y, heading.eulerAngles.y - 90, 1 );
		
		if(type == EnemyTypes.Linked){
		foreach(GameObject g in attachedEnemies)
			{
				attachedEnemyScripts.Add(g.GetComponent<Enemy>());
			}
		}
		
		
		if(pathNodes.Count==0){
			//Debug.Log("walking Nodes disabled due to lack of nodes.");
			isWalkingPath = false;
		}
	
    	setupSprite();
		
	}
	// Update is called once per frame
	void Update ()
	{
		if(state != EnemyStates.Dead)
		{	
			swapSpriteUsingHeading();
		}else{
			if(animatingDeath==true){
				
				deathAnimation();	
			}
		}
		
		
	}
	
	void FixedUpdate()
	{
		if(waiting){
			checkForOtherEnemyDeaths();
		}
		if(isWalkingPath){
			moveToNextPath();
		}
		
		
	}
	void setupSprite(){
		
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);
 
        myRenderer = renderer;
 
        if(myRenderer == null) enabled = false;
 
        myRenderer.material.mainTextureScale =  scale;
		
	}
	void swapSpriteUsingHeading(){
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		
		if(headingAngle>=270||headingAngle<=90){
				iY=1;
	        	//to the right
			}else {
				iY=0;
	            //to the left
			}
		if(iY != lastIY){
			lastIndex = 0;
		}
		
        if(index != lastIndex)
        {
			//Debug.Log("E: index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            Vector2 offset = new Vector2(iX* scale.x,
                                         1-(scale.y*iY));
			
            iX++;
            if(iX / spriteTilesX == 1)
            {
               
                iX=0;
                
            }
 			//Debug.Log("E: offset :"+offset);
			
            myRenderer.material.mainTextureOffset =  offset;
 
 
 
        }
            lastIndex = index;   
			lastIY = iY;
	}
	void deathAnimation(){
		
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		iY=5;
		if(index != lastIndex)
        {
			//Debug.Log("Player: index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            Vector2 offset = new Vector2(iXDead* scale.x, 1-(scale.y*iY));
			
            iXDead++;
            if(iXDead / spriteTilesXDeath == 1)
            {
                animatingDeath = false;
            }
			
 			Debug.Log("deathAnim (iXDead, iY): "+iXDead+", "+iY);			
            myRenderer.material.mainTextureOffset =  offset;

        }
            lastIndex = index;
		
	}
	void moveToNextPath(){
		//Debug.Log("path index: "+pathIndex+"walking path");
		if(pathNodes.Count != 0){
		if(pathIndex >= pathNodes.Count){
			pathIndex = 0;
			//Debug.Log("reset path index");
		}
		
			if(Vector3.Distance(transform.position, pathNodes[pathIndex].transform.position) < .1)
			{
				if(pathDelay > 0){
		            pathDelay -= (Time.deltaTime * pathTimeScale);
					heading = Quaternion.LookRotation( Vector3.Normalize(pathNodes[ (pathIndex+1) % pathNodes.Count ].transform.position - transform.position),Vector3.up);
					headingAngle = Mathf.LerpAngle( transform.eulerAngles.y, heading.eulerAngles.y - 90, 1 );
		           // Debug.Log("pathDelay :" + pathDelay);
		      	  }else{
					pathDelay = 0;
					pathIndex++;
				}
			}else{
				
				//move toward the next point
				//Debug.Log("transform.position: "+transform.position+"pathNodes[pathIndex].transform.position"+pathNodes[pathIndex].transform.position);
				resetDelay();
				
				//speed is not needed in the following rotation, because its rotating the game object, not the sprite, 
				//but the rotation is important because the sprites use it to know which way to face.
				heading = Quaternion.LookRotation( Vector3.Normalize(pathNodes[pathIndex].transform.position - transform.position),Vector3.up);
				headingAngle = Mathf.LerpAngle( transform.eulerAngles.y, heading.eulerAngles.y - 90, 1 );
	
				transform.position =  Vector3.MoveTowards(transform.position, pathNodes[pathIndex].transform.position, speed);
				//Debug.Log("heading angle :" + headingAngle);
				
			}
		}
	}
	void checkForOtherEnemyDeaths(){
			
		
			deathCheckTimer -= Time.deltaTime;
			if(deathCheckTimer <= 0)
			{
				if(type == EnemyTypes.Linked || type == EnemyTypes.Ordered)
				{
					foreach(Enemy s in attachedEnemyScripts)
					{
						if(s == null)
						{
							attachedEnemyScripts.Remove(s);
						}
					}
					foreach(GameObject g in attachedEnemies)
					{
						if(g == null)
						{
							attachedEnemies.Remove(g);
						}
					}
				}
				
				switch(type)
				{
				case EnemyTypes.Ordered:
					//check if enemies have been killed in correct order (HOW? I DON'T KNOW!)
					break;
				case EnemyTypes.Linked:
					int deadEnemy =0;
					foreach(Enemy theEnemyScript in attachedEnemyScripts)
					{
						//Debug.Log("["+theEnemyScript+"].alive :"+attachedEnemyScripts[i].alive);
						if(theEnemyScript.alive == true)
						{
							break;
						}else{
							
							deadEnemy++;
						}
						if(deadEnemy == attachedEnemyScripts.Count-1)
						{
							DestroyList();
							Destroy(this.gameObject);
						}
					}
					break;
				}
				deathCheckTimer = deathCheckDelay;
				waiting = false;
				alive = true;
				this.renderer.enabled = true;
				transform.GetChild(0).renderer.enabled = true;
				transform.GetChild(1).renderer.particleSystem.Play(); 
			}
		
	}
	public void EnemyDestruction()
	{
		if(alive)
		{
			switch(type)
			{
			case EnemyTypes.Normal:
				//Death animation plays
				
				alive = false;
				
				Instantiate(poof, gameObject.transform.position, poof.transform.rotation);
				Destroy(this.gameObject);
				enemyDeathCount++;
				Debug.Log("Enemy Death Count NORMAL: " + enemyDeathCount);
				this.renderer.enabled = false;
				
				break;
			case EnemyTypes.Gemmed:
				//Do Nothing, Gemmed enemies don't die
				break;
			case EnemyTypes.Ordered:
				alive = false;
				//check if this is the first enemy in the list. If it isn't, respawn after a few seconds 
				break;
			case EnemyTypes.Linked:
				//Death animation plays
				alive = false;
				this.renderer.enabled = false;
				transform.GetChild(0).renderer.enabled = false;
				transform.GetChild(1).renderer.particleSystem.Stop(); 
				waiting = true;
				break;
			default:
				//Death animation plays
				alive = false;
				this.renderer.enabled = false;
				
				Instantiate(poof, gameObject.transform.position, poof.transform.rotation);
				Destroy(this.gameObject);
				
				
				break;
			}
		}
	}
	void OnTriggerEnter(Collider otherObject)
	{
		//Debug.Log("Kill player :"+otherObject.tag);
		if(otherObject.tag == "Player")
		{
			
			otherObject.gameObject.GetComponent<Player>().OnDeath();

		}
	}
	
	public void DestroyList()
	{
		foreach(GameObject g in attachedEnemies)
		{
			Instantiate(poof, gameObject.transform.position, poof.transform.rotation);
			Destroy(g);
			enemyDeathCount++;
			Debug.Log("Enemy Death Count LINKED: " + enemyDeathCount);
		}
	}
	
	public void resetDelay(){
        
        pathDelay = pathDelayDuration;    
     }

}
