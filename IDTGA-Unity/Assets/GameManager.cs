using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private Player [] players;
	public bool gameOver =false;
	
	public int enemiesToDestroy; 
	public int gemSwitchesToHit;
	public GameObject victoryOpensLastDoor;
	
	private bool doOnce;
	private int currentLevel; 
	
	public bool level1,level2,level3,endGame,testing; 
	
	// Use this for initialization
	void Start () 
	{
		players = GameObject.FindObjectsOfType(typeof(Player))as Player[];
	}
	
	// Update is called once per frame
	void Update () {
		if(gameOver){
				StartCoroutine("GameOver");
			}
		int deadCount=0;
		foreach(Player aPlayer in players){
			if(aPlayer.state == Player.States.Dead){
				deadCount++;
			}
		}
		if(deadCount>=players.Length){
			gameOver=true;
		}else{
			gameOver=false;
		}
		
		//if testing press 'p' to achieve winning conditions
		if(testing && Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log("geeting P call");
			
			if(enemiesToDestroy > 0)
			{
				Enemy.enemyDeathCount = enemiesToDestroy;
			}
			if(gemSwitchesToHit > 0)
			{
				GemSwitch.gemSwitchesHit = gemSwitchesToHit; 
			}
			
		}
		
		//checking for vicotory conditions 
		
		//Room just need death count to pass 
		if(enemiesToDestroy > 0 && gemSwitchesToHit == 0 && Enemy.enemyDeathCount == enemiesToDestroy)
		{
			if(victoryOpensLastDoor != null)
			{
				StartCoroutine("LevelComplete"); 
				WaitingForLevelSwitch(); 
			}
			
		}
		//Room just needs switches to pass  
		if(gemSwitchesToHit > 0 && enemiesToDestroy == 0 && GemSwitch.gemSwitchesHit == gemSwitchesToHit)
		{
			if(victoryOpensLastDoor != null)
			{
				StartCoroutine("LevelComplete");  
				WaitingForLevelSwitch();
			}
		}
		//Room needs switches and death count to pass
		if(gemSwitchesToHit > 0 && enemiesToDestroy > 0 && Enemy.enemyDeathCount == enemiesToDestroy && GemSwitch.gemSwitchesHit == gemSwitchesToHit)
		{
			if(victoryOpensLastDoor != null)
			{
				StartCoroutine("LevelComplete");  
				WaitingForLevelSwitch();
			}
		}
		
	}
	
	IEnumerator LevelComplete () 
	{
		if(doOnce == false)
		{	
			doOnce = true;
				
			yield return new WaitForSeconds(1f);
				
			victoryOpensLastDoor.GetComponent<Door>().isActive = true;
			Debug.Log("Door shoud be active lvl3");
		}
		
	}
	void WaitingForLevelSwitch()
	{
	
		Debug.Log("Calling for 'Waiting for level switch'");
		
		if(victoryOpensLastDoor.GetComponentInChildren<DoorExit>().exitTrigger)
		{
			Application.LoadLevel(Application.loadedLevel + 1); 
		}
	}
	IEnumerator GameOver()
	{
		yield return new WaitForSeconds(2f);
		Application.LoadLevel(Application.loadedLevelName);
	}
	
}
