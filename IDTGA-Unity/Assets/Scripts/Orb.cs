using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {
	public ParticleSystem particle;
	public int pointValue = 1;
	
	public float fps;
	private int lastIndex=0;
	private int iX=1;
	private int iY=1;
	private int lastIY=-1;
	public int spriteTilesX = 8;
	public int spriteTilesY = 2;
	private int targetFrame = 8;
	private Vector2 scale;
	private Renderer myRenderer;
	
	private AudioSource orbSoundSource;
	public AudioClip orbSound;
	
	// Use this for initialization
	void Start () {
		setupSprite();
		float repeatTime =  Random.value*10f;
		   InvokeRepeating("GlitterParticle", 0f,repeatTime);
		AudioSource[] audioSources = this.GetComponents<AudioSource>();
		orbSoundSource  = audioSources[0];

	}
	
	// Update is called once per frame
	void Update () {
		thereAndBackAgainSprite();
	}
	void GlitterParticle ()
	{
		particle.Play();
	}
	public int Pickup(){
		if(orbSoundSource.isPlaying == true)
		{
			orbSoundSource.Stop();
		}
		orbSoundSource.PlayOneShot(orbSound);
		//animatingDeath = true;
		
		int returnValue = pointValue;
		Debug.Log("Orb Return Value: " + returnValue);
		if(pointValue>=0){
			pointValue = 0;
				iY=2;	
		fps = 12;
		iX=0;
		}
		//Debug.Log("pointValue "+pointValue);
		return returnValue;
	}
	void setupSprite(){
		
		scale = new Vector2 (1.0f / spriteTilesX ,
                             1.0f / spriteTilesY);
 
        myRenderer = renderer;
 
        if(myRenderer == null) enabled = false;
 
        myRenderer.material.mainTextureScale =  scale;
		
	}
	void thereAndBackAgainSprite(){
		int index = (int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY);
		
		
		if(iY != lastIY){
			lastIndex = 0;
		}
		
        if(index != lastIndex)
        {
			//Debug.Log("E: index = "+(int)(Time.timeSinceLevelLoad * fps) % (spriteTilesX * spriteTilesY)+" fps = "+fps+", spriteTilesX= "+spriteTilesX+", spriteTilesY = "+spriteTilesY);
			
            Vector2 offset = new Vector2(iX* scale.x,
                                         1-(scale.y*iY));
			
            
            if(iX<targetFrame)
            {
                iX++;
            }else if(iX>targetFrame){
				iX--;	
			}else{
				if(targetFrame == spriteTilesX){
					targetFrame	=0;
				}else{
					targetFrame = spriteTilesX;
				}
			if(targetFrame == 0&&iY==2){
				Destroy(transform.gameObject);	
				}
			}
 			//Debug.Log("orb: offset :"+offset);
			
            myRenderer.material.mainTextureOffset =  offset;
 
        }
            lastIndex = index;   
			lastIY = iY;
	}
	
}
