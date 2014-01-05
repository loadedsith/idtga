using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour {
	
	public GameObject playerOne;
	public GameObject playerTwo;
	public LineRenderer laserOne;
	public LineRenderer laserTwo;
	
	private Vector3 playerOnePos;
	private Vector3 playerTwoPos;
	//Our Record of everything our two Raycasts hit:
	private RaycastHit[] RaycastHitsFromPlayerOneToPlayerTwo;
	private RaycastHit[] RaycastHitsFromPlayerTwoToPlayerOne;
	private Vector3 firstObstructionFromPlayerOneToPlayerTwo;
	private Vector3 firstObstructionFromPlayerTwoToPlayerOne;
	//Quick access to the number of things we hit with both our raycasts:
	private int totalRaycastHitsFromPlayerOneToPlayerTwo;
	private int totalRaycastHitsFromPlayerTwoToPlayerOne;
	//A variable for caching the game objects we will be hitting with our raycasts:
	private GameObject currentlyCheckedGameObject;
	//Our two directions for casting our raycasts:
	private Vector3 directionFromPlayerOneToPlayerTwo;
	private Vector3 directionFromPlayerTwoToPlayerOne;
	
	private bool isFiring;
	public KeyCode LaserKey;
	
	public InGameGUI referenceToCurrentEnergy;
	public float laserDecayRate;
	public float laserRechargeRate;
	
	void Start()
	{
		laserDecayRate = laserDecayRate / 10;
		laserRechargeRate = laserRechargeRate / 10;
		referenceToCurrentEnergy = Camera.main.GetComponent<InGameGUI>();
		playerOnePos = playerOne.transform.position;
		playerTwoPos = playerTwo.transform.position;
		laserOne = transform.GetChild(0).GetComponent<LineRenderer>();
		laserTwo = transform.GetChild(1).GetComponent<LineRenderer>();
	}
	
	void Update ()
	{

		
		if(Input.GetKeyDown(LaserKey))
		{
			if(referenceToCurrentEnergy.currentEnergy > 0)
			{
				if(isFiring == false)
				{
					SetLaserUp ();
					
				}
				else
				{
					ShutLaserOff ();
					
				}
			}
		}
		
		if(playerOne.GetComponent<Player>().state == Player.States.Dead
			|| playerTwo.GetComponent<Player>().state == Player.States.Dead)
		{
			ShutLaserOff ();;
		}
		
		if(referenceToCurrentEnergy.currentEnergy <= 0)
		{
			ShutLaserOff ();
		}
		
		laserOne.material.SetTextureScale("_MainTex", new Vector2(Vector3.Distance(playerOnePos, firstObstructionFromPlayerOneToPlayerTwo)/2, 1));
		laserTwo.material.SetTextureScale("_MainTex", new Vector2(Vector3.Distance(playerTwoPos, firstObstructionFromPlayerTwoToPlayerOne)/2, 1));
		
	}
	
	void FixedUpdate()
	{
		if(isFiring == true)
		{
			AimLaser ();
			FireLaser ();
		}
		else
		{
			//Recharge energy currently turned off
			//referenceToCurrentEnergy.currentEnergy += laserRechargeRate;
		}
	}

	public void AimLaser ()
	{
		//Default position of the first obstruction will be the opposite player for debug purposes.
		//This way if there are no obstructions, the Debug.DrawRays will hit the other player.
		firstObstructionFromPlayerOneToPlayerTwo = playerTwoPos;
		firstObstructionFromPlayerTwoToPlayerOne = playerOnePos;
		
		playerOnePos = playerOne.transform.position;
		playerTwoPos = playerTwo.transform.position;
		
		directionFromPlayerOneToPlayerTwo = playerTwoPos - playerOnePos;
		directionFromPlayerTwoToPlayerOne = playerOnePos - playerTwoPos;
	}

	public void FireLaser ()
	{
		//First raycast goes from player one to player two
		RaycastHitsFromPlayerOneToPlayerTwo = Physics.RaycastAll(playerOnePos, directionFromPlayerOneToPlayerTwo.normalized, directionFromPlayerOneToPlayerTwo.magnitude);
		//totalRaycastHitsFromPlayerOneToPlayerTwo = RaycastHitsFromPlayerOneToPlayerTwo.Length;
		SortLists(RaycastHitsFromPlayerOneToPlayerTwo);
		
		
		//Second raycast goes from player two to player one
		RaycastHitsFromPlayerTwoToPlayerOne = Physics.RaycastAll(playerTwoPos, directionFromPlayerTwoToPlayerOne.normalized, directionFromPlayerTwoToPlayerOne.magnitude);
		totalRaycastHitsFromPlayerTwoToPlayerOne = RaycastHitsFromPlayerTwoToPlayerOne.Length;
		SortLists(RaycastHitsFromPlayerTwoToPlayerOne);
		
		//Checking the raycast from player one to player two:
		for(int i = 0; i < RaycastHitsFromPlayerOneToPlayerTwo.Length; i++)//Searching through the first raycast's results
		{
			currentlyCheckedGameObject = (GameObject)RaycastHitsFromPlayerOneToPlayerTwo[i].transform.gameObject;
			switch(currentlyCheckedGameObject.tag)
			{
			case "Enemy":
				currentlyCheckedGameObject.GetComponent<Enemy>().EnemyDestruction();
				break;
			case "Wall":
				firstObstructionFromPlayerOneToPlayerTwo = RaycastHitsFromPlayerOneToPlayerTwo[i].point;
				i = RaycastHitsFromPlayerOneToPlayerTwo.Length;
				break;
			case "Door":
				firstObstructionFromPlayerTwoToPlayerOne = RaycastHitsFromPlayerTwoToPlayerOne[i].point;
				i = RaycastHitsFromPlayerTwoToPlayerOne.Length;
				break;
			case "GemSwitch":
				currentlyCheckedGameObject.GetComponent<GemSwitch>().isActive = true; 
				break;	
			default:
				break;
			}
		
		}
		
		referenceToCurrentEnergy.currentEnergy -= laserDecayRate;
		
		//Checking the raycast from player two to player one:
		for(int i = 0; i < totalRaycastHitsFromPlayerTwoToPlayerOne; i++)//Searching through the second raycast's results
		{
			currentlyCheckedGameObject = (GameObject)RaycastHitsFromPlayerTwoToPlayerOne[i].transform.gameObject;
			switch(currentlyCheckedGameObject.tag)
			{
			case "Enemy":
				currentlyCheckedGameObject.GetComponent<Enemy>().EnemyDestruction();
				break;
			case "Wall":
				firstObstructionFromPlayerTwoToPlayerOne = RaycastHitsFromPlayerTwoToPlayerOne[i].point;
				i = RaycastHitsFromPlayerTwoToPlayerOne.Length;
				break;
			case "Door":
				firstObstructionFromPlayerTwoToPlayerOne = RaycastHitsFromPlayerTwoToPlayerOne[i].point;
				i = RaycastHitsFromPlayerTwoToPlayerOne.Length;
				break;
			case "GemSwitch":
				currentlyCheckedGameObject.GetComponent<GemSwitch>().isActive = true; 
				break;	
			default:
				break;
			}
		}
		
		
		Debug.DrawRay(playerOnePos, firstObstructionFromPlayerOneToPlayerTwo - playerOnePos, Color.green);
		Debug.DrawRay(playerTwoPos, firstObstructionFromPlayerTwoToPlayerOne - playerTwoPos, Color.cyan);
		laserOne.SetPosition(0, playerOnePos);
		laserOne.SetPosition(1, firstObstructionFromPlayerOneToPlayerTwo);
		laserTwo.SetPosition(0, playerTwoPos);
		laserTwo.SetPosition(1, firstObstructionFromPlayerTwoToPlayerOne);
		
//		For Reference, the below is a system for dynamically adding and subtracting points along the line renderer. It works, but not well enough.
//		int laserResolution = 5;
//		float vertexCount = Vector3.Distance(playerOnePos, firstObstructionFromPlayerOneToPlayerTwo)/laserResolution;
//		vertexCount = Mathf.Clamp(vertexCount, 2, vertexCount);
//		laserOne.SetVertexCount((int)vertexCount);
//		Debug.Log("Vertex Count: " + vertexCount);
//		laserOne.SetPosition(0, playerOnePos);
//		for(float i = 1; i < vertexCount; i++)
//		{
//			laserOne.SetPosition((int)i, Vector3.Lerp(playerOnePos, firstObstructionFromPlayerOneToPlayerTwo, (i+1) * (1/vertexCount)));
//			Debug.Log("i: " + i);
//			Debug.Log(i * (1/vertexCount));
//		}
		
		


	}
	
	void SortLists(RaycastHit[] list)
	{
		for(int i = 1; i < list.Length; i++)
		{
			for(int j = i; j > 0; j--)
			{
				if(list[j].distance < list[j - 1].distance)
				{
					SwapIndexes(list, j, j-1);
				}
				else
				{
					j = -1;
				}
			}
		}
	}

	public void SetLaserUp ()
	{
		isFiring = true;
		laserOne.renderer.enabled = true;
		laserTwo.renderer.enabled = true;
		this.GetComponent<AudioSource>().Play();
	}

	public void ShutLaserOff ()
	{
		isFiring = false;
		laserOne.renderer.enabled = false;
		laserTwo.renderer.enabled = false;
		this.GetComponent<AudioSource>().Stop();
	}
	
	void SwapIndexes(RaycastHit[] list, int x, int y)
	{
		RaycastHit temp = list[x];
		list[x] = list[y];
		list[y] = temp;
	}
}
