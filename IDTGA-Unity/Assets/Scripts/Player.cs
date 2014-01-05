using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

	public float orbCount = 0;
	public float orbMax = 3;
	public GameObject player;
	public int playerId = 0;
	
	public enum Directions {North, East, South, West};
	public enum States {Walking, Idle, Inactive, Interacting, Damaged, LaserLeft, LaserRight, Invincible, Dead};
	public Directions direction;
	public States state;
	
	public float speedScale;
	private enum controls {Horizontal, Vertical, Fire};
	public KeyCode playerOneInteractKey;
	public KeyCode playerTwoInteractKey;
	private string[] playerOneControls = new string[]{"Horizontal", "Vertical", "Fire1"};
	private string[] playerTwoControls =  new string[]{"Horizontal2", "Vertical2", "Fire2"};
	public List<string[]> defaultControls = new List<string[]>();
	CharacterController ourController;

	private Quaternion heading;
	private float headingAngle = 270;
	
	public float fps;
	private int lastIndex=0;
	private int iX=1;
	public int spriteTilesXDeath=8;
	
	private int iY=1;
	private int lastIY=1;
	public int spriteTilesX = 8;
	public int spriteTilesY = 2;
	private Vector2 scale;
	private Renderer myRenderer;
	
	public AudioClip footstepsAudio;
	public AudioClip onDeathAudio;
	public AudioClip onReviveAudio;
	private AudioSource footStepsAudioSource;
	private AudioSource onDeathAudioSource;
	private AudioSource onReviveAudioSource;
	
	public float invicibilityTime;
	public float invincibilityCountDown;
	
	private float inputVertical;
	private float inputHorizontal;
	
	public bool showContextSprite = true;
	private int contextLastIndex=0;
	private int contextIX=1;
	private int contextIY=1;
	public int contextSpriteTilesX = 3;
	public int contextSpriteTilesY = 1;
	private Vector2 contextScale;
	private Renderer contextRenderer;
	public int contextFps = 6;
	//On Death Varibles 
	
	static int playersDead; 
	public Collider playerCollider;  
	public InGameGUI inGameGUIReference;
	
	// Use this for initialization
	void Start () {
		inGameGUIReference = Camera.main.GetComponent<InGameGUI>();
		AudioSource[] audioSources = this.GetComponents<AudioSource>();
		footStepsAudioSource  = audioSources[0];
		onDeathAudioSource  = audioSources[1];
		onReviveAudioSource = audioSources[2];
		ourController = gameObject.GetComponent<CharacterController>();
		Debug.Log("Player created "+playerId);
		defaultControls.Add(playerOneControls);
		defaultControls.Add(playerTwoControls);
		state=States.Idle;
		direction = Directions.South;
		setupSprite();
		setupContextSprite();
		swapSpriteUsingHeading();

		playersDead = 0;
		
	}
	
	void FixedUpdate()
	{
		//checkForInput();
		if(state == States.Invincible)
		{
			invincibilityCountDown -= Time.deltaTime;
			if(invincibilityCountDown <= 0)
			{
				state = States.Idle;
				Color col = gameObject.renderer.material.color;
				col.a=1;
				myRenderer.material.color = col;
			}
		}
		if(state != States.Dead){
			
			movePlayer(inputHorizontal,inputVertical);
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkForInput();
		if(state != States.Dead)
		{	
			updatePlayerHeading();
			
			EmitWalkingAudio();
			displayContextSprite();
		}
		Color col = myRenderer.material.color;
		if(state == States.Invincible){
			float phi = Time.time / invincibilityCountDown * 2 * Mathf.PI;
        	float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;
			col.a=amplitude;
			myRenderer.material.color = col;
		}else if(col.a !=1){
			col.a=1;
			myRenderer.material.color = col;
		}
		//orbCountGUI();
	}
	void deathAnimation(){
		
		Vector2 offset = new Vector2(0* scale.x, 1-(scale.y*5));
		myRenderer.material.mainTextureOffset =  offset;
	
		
		/*  The animation is not in the scope anymore, and as it was getting run in the update, i've replaced it with a 
		 * one frame animation
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		iY=5;
		if(index != lastIndex)
        {
			//Debug.Log("Player: index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            iXDead++;
            Vector2 offset = new Vector2(iXDead* scale.x, 1-(scale.y*iY));
			 if(iXDead >= spriteTilesXDeath )
            {
				Debug.Log("Totally deathed out");
                animatingDeath = false;
				iXDead=1;
            }
           
			
 			Debug.Log("Player "+playerId+": deathAnim (iXDead, iY): "+iXDead+", "+iY);			
            myRenderer.material.mainTextureOffset =  offset;

        }
            lastIndex = index;*/
		
	}
	void revive(){
		iY=3;
		iX=1;
		state = States.Invincible;
		if(onReviveAudioSource.isPlaying == true)
		{
			onReviveAudioSource.Stop();
		}
		onReviveAudioSource.PlayOneShot(onReviveAudio);
		//animatingDeath = true;
		//invincibilityCountDown = invicibilityTime;
		playersDead--;
		playerCollider.enabled = false;
		swapSpriteUsingHeading();
		Debug.Log("Player "+playerId+": Is alive again!");
		}
	
	void swapSpriteUsingHeading(){
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		
		switch((int)headingAngle){
			//right
			case 180:
				iY=1;
			break;
			
			//left
			case 0:
				iY=2;
			break;
					
		    //up
			case 270:
				iY=3;	
			break;
			
	        //down
			case 90:
				iY=4;
				break;
			default:
				iY=3;
			
			break;
		}
		//force update
		if(iY != lastIY){
			lastIndex = 1;
		}
		
        if(index != lastIndex)
        {
			//Debug.Log("Player: index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            Vector2 offset = new Vector2(iX* scale.x, 1-(scale.y*iY));
			
            iX++;
            if(iX / spriteTilesX == 1)
            {
                iX=0;
            }
			
 			//Debug.Log("Player: offset :"+offset);			
            myRenderer.material.mainTextureOffset =  offset;

        }
            lastIndex = index;   
			lastIY = iY;
	}
	void setupSprite(){
		
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);
 
		Transform aTransform = transform.FindChild("SpriteHead");
		myRenderer = aTransform.GetComponentInChildren<Renderer>();
 
        if(myRenderer == null) enabled = false;
 
		Vector2 offset = new Vector2(iX* scale.x,
                                         1-(scale.y*iY));
        myRenderer.material.mainTextureScale =  scale;
        myRenderer.material.mainTextureOffset =  offset;
			
        
	}
	void setupContextSprite(){
		
		contextScale = new Vector2 (1.0f / contextSpriteTilesX ,
                             1.0f / contextSpriteTilesY);
 
		Transform aTransform = transform.FindChild("ContextSpriteHead");
		contextRenderer = aTransform.GetComponentInChildren<Renderer>();
 
        if(contextRenderer == null) enabled = false;
		
 		Vector2 contextOffset = new Vector2(iX* contextScale.x,
                                         1-(contextScale.y*iY));
        contextRenderer.material.mainTextureScale =  contextScale;
		contextRenderer.material.mainTextureOffset =  contextOffset;
		
			
        
	}
	void updatePlayerHeading(float aHeading){
		headingAngle= aHeading;
	}
	void updatePlayerHeading(){
		if(inputVertical!=0){
			//Debug.Log("Player "+playerId+": Vertical delta: "+inputVertical);
			//ourController.Move(new Vector3(0,0,(inputVertical*Time.deltaTime)*speedScale));
			if(inputVertical>0){
				headingAngle = 270;
			}else{
				headingAngle = 90;
			}
			
		}
		if(inputHorizontal!=0){
			//ourController.Move(new Vector3((inputHorizontal*Time.deltaTime)*speedScale,0,0));
			
			if(inputHorizontal>0){
				headingAngle = 180;
			}else{
				headingAngle = 0;
			}
		}
	}
	void movePlayer(float aInputHorizontal, float aInputVertical){
		//Debug.Log("Player "+playerId+", Heading Angle = "+headingAngle);
		if(inputHorizontal != 0 || inputVertical != 0){
			ourController.Move(new Vector3(aInputHorizontal, 0, aInputVertical) * speedScale * Time.deltaTime);
			swapSpriteUsingHeading();
		}
	}
	void checkForInput(){
		inputVertical = Input.GetAxis(defaultControls[playerId][(int)controls.Vertical]);
		inputHorizontal = Input.GetAxis(defaultControls[playerId][(int)controls.Horizontal]);

		if(Input.GetAxis(defaultControls[playerId][(int)controls.Fire])!=0){
			//Debug.Log("Player "+playerId+": Fire! (Waiting on the laser code)");
			
		}
		
		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			Application.LoadLevel(Application.loadedLevelName); 
		}
		
	}
	
	void OnTriggerEnter(Collider inputCollider){
		if(state != States.Dead)
		{
			if(inputCollider.tag == "Orb")
			{	
				int increase = inputCollider.gameObject.GetComponent<Orb>().Pickup();
				orbCount+= increase;
				inGameGUIReference.IncreaseEnergy(increase);
			}
		}	
		
		
	}
	void OnTriggerStay(Collider inputCollider)
	{
       if(state != States.Dead)
		{
			switch(inputCollider.tag){
	                case "Switch":
						showContextSprite = true;
	                break;
	            }
			//if(Input.GetAxis(defaultControls[playerId][(int)controls.Fire])!=0)
			if(playerId == 0 && Input.GetKeyDown(playerOneInteractKey) || playerId == 1 && Input.GetKeyDown(playerTwoInteractKey))
	        {
	            //Debug.Log("Player " + playerId + ": Switch activated");
				switch(inputCollider.tag){
	                case "Switch":
	           			Debug.Log("Player " + playerId + ": Switch activated");
						showContextSprite = true;
						inputCollider.gameObject.GetComponent<Switch>().SetToActive();
	                break;
	                case "Door":
	                    //inputCollider.gameObject.GetComponent<Door>().isActive = !inputCollider.gameObject.GetComponent<Door>().isActive;
	                    //I commented this out because for some reason you can just walk up to it and open it. 
						//inputCollider.gameObject.GetComponent<Door>().opened = true;
	                break;
	                case "Trap":
	
	                	//inputCollider.gameObject.GetComponent<Traps>().SetToActive();;
	                break;
	            }
	        }
		}

       
       
	}
	void displayContextSprite(){
		int index = (int)(Time.timeSinceLevelLoad * contextFps) % (contextSpriteTilesX * contextSpriteTilesX);
		
		//Debug.Log("iX: "+iX+", Orb ratio: "+orbRatio+", max-orbsDespenced: "+ (maxOrbs-orbsDespenced) +", orbs.Count : "+orbs.Count);
	//	if(iX != lastIX){
		int targetFrame = 0;
		if(showContextSprite){
			targetFrame = 2;
		}else{
			targetFrame = 0;
		}
		//int targetFrame = contextSpriteTilesX*(int)showContextSprite;
	        if(index != contextLastIndex)
	        {
				//Debug.Log("Context: contextIX : "+contextIX+", index = "+(int)(Time.timeSinceLevelLoad * contextFps) % (contextSpriteTilesX * contextSpriteTilesY)+" contextFps = "+contextFps);
				
	            //Vector2 offset = new Vector2(iX* scale.x,
	              //                           1-(scale.y*iY));
				Vector2 offset = new Vector2(contextIX* contextScale.x,
	                                         1-( contextScale.y*contextIY));
				
				if(contextIX<targetFrame){
					contextIX++;
				}else if(contextIX>targetFrame){
					contextIX--;	
				}
	            
	 			//Debug.Log("Context: ix :"+offset);
				
	            contextRenderer.material.mainTextureOffset =  offset;
	 
	        }
			   
	//	}
    	contextLastIndex = index;
	}
	void OnTriggerExit(Collider inputCollider)
	{
		switch(inputCollider.tag){
	                case "Switch":
						showContextSprite = false;
	                break;
	            }
		
	}
	
	void  OnControllerColliderHit(ControllerColliderHit objectHit)
	{	
		//Debug.Log("Player "+playerId+" : being hit by:" + objectHit.gameObject.tag);
		
		//Debug.Log("whos first:"  + playerId + objectHit.gameObject.GetComponent<Player>().playerId);
		switch(objectHit.gameObject.tag){
		case "Player":
			Player aPlayer =objectHit.gameObject.GetComponent<Player>(); 
			if(aPlayer.state == States.Dead)
			{
				//TODO: animation code and sound code 
				aPlayer.revive();
			}
			break;
		case "Enemy":
			if(state != States.Invincible){
				OnDeath();
			}
			break;
		}
		
	}
	
	public void EmitWalkingAudio()
	{
		if(inputHorizontal != 0 || inputVertical != 0)
		{
			if(footStepsAudioSource.isPlaying == false)
			{
				footStepsAudioSource.clip = footstepsAudio;
				footStepsAudioSource.loop = true;
				footStepsAudioSource.Play();
			}
		}
		else
		{
			footStepsAudioSource.Stop();
		}
		if(state == States.Dead){
				footStepsAudioSource.Stop();
		}
	}
	
	public void OnDeath()
	{
		if(state != States.Dead && state != States.Invincible)
		{
			Debug.Log("Player"+playerId+": is dead");
			playersDead ++;
			playerCollider.enabled = true; 
			state = States.Dead;
			invincibilityCountDown = invicibilityTime;
			if(onDeathAudioSource.isPlaying == true)
			{
				onDeathAudioSource.Stop();
			}
			if(footStepsAudioSource.isPlaying == true)
			{
				footStepsAudioSource.Stop();
			}
			onDeathAudioSource.PlayOneShot(onDeathAudio);
			iX=1;
			deathAnimation();	
			//animation code and sound 
			//
			
			if(playersDead == 2)
			{
				//StartCoroutine("GameOver");  
			}
		}
		
	}
	

		
	/*void orbCountGUI(){
			
		
		GameObject energyGUI =  GameObject.Find("EnergyGUI");
				if(orbCount == orbMax){
			
					energyGUI.renderer.material.mainTexture=energyGUIFullFrame0;
				}else if(orbCount/orbMax>=.75 ){
					energyGUI.renderer.material.mainTexture=energyGUIThreeQuartersFrame0;

				}else if(orbCount/orbMax>=.5){
					energyGUI.renderer.material.mainTexture=energyGUIHalfFrame0;

				}else if(orbCount/orbMax>=.25 ){
					energyGUI.renderer.material.mainTexture=energyGUIOneQuarterFrame0;

				}else if(orbCount >= 0 ){
				//Full Energy Animation	
				energyGUI.renderer.material.mainTexture=energyGUIEmptyFrame0;
			}
	}*/
	

}

