using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
	
	
	public GameObject playerOne;
	public GameObject playerTwo;
	private Vector3 playerOnePos;
	private Vector3 playerTwoPos;
	private Vector3 averagePosition;
	private Vector3 positionToMoveTowards;
	private float distanceBetPlayers;
	private float percentMaxDistance;
	
	private Camera mainCamera;
	
	public float maxDistanceBetweenPlayers;
	public float maxViewSize;
	public float minViewSize;
	public float maxPanSpeed;
	public float minPanSpeed;
	private float panSpeed;
	
	public bool showingSomething;
	public float showingSomethingTime;
	private float showingSomethingTimer;
	public List<GameObject> objectsToShow = new List<GameObject>();
	
	void Start ()
	{
		maxPanSpeed = maxPanSpeed /10;
		minPanSpeed = minPanSpeed / 10;
		UpdatePositions();
		mainCamera = Camera.main;
	}
	
	void FixedUpdate()
	{
		if(showingSomething == true)
		{
			showingSomethingTimer -= Time.deltaTime;
		}
	}

	void Update ()
	{
		if(showingSomething == true)
		{
			UpdatePositionsWhileShowingSomething();
			if(showingSomethingTimer <= 0)
			{
				showingSomething = false;
			}
		}
		else
		{
			UpdatePositions();
			
			if(Input.GetKey(KeyCode.L))
			{
				mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, maxViewSize, Time.deltaTime * 5);
			}
			else
			{
				mainCamera.orthographicSize = Mathf.Lerp(minViewSize, maxViewSize, percentMaxDistance);
			}
		}
		//mainCamera.orthographicSize = Mathf.Lerp(minViewSize, maxViewSize, percentMaxDistance);
	}

	public void UpdatePositions()
	{
		playerOnePos = playerOne.transform.position;
		playerTwoPos = playerTwo.transform.position;
		
		averagePosition = (playerOnePos + playerTwoPos) / 2;
		
		distanceBetPlayers = Vector3.Distance(playerOnePos, playerTwoPos);
		
		percentMaxDistance = distanceBetPlayers/maxDistanceBetweenPlayers;
		
		percentMaxDistance = Mathf.Clamp(percentMaxDistance, 0.0f, 1.0f);

		positionToMoveTowards = new Vector3(averagePosition.x, transform.position.y, averagePosition.z);
		
		panSpeed = maxPanSpeed * (1 - percentMaxDistance);
		
		panSpeed = Mathf.Clamp(panSpeed, minPanSpeed, maxPanSpeed);
		
		transform.position = Vector3.MoveTowards(transform.position, positionToMoveTowards, panSpeed);
	}
	
	public void UpdatePositionsWhileShowingSomething()
	{
		playerOnePos = playerOne.transform.position;
		playerTwoPos = playerTwo.transform.position;
		
		averagePosition = (playerOnePos + playerTwoPos);
		foreach(GameObject GO in objectsToShow)
		{
			averagePosition += GO.transform.position;
		}
		averagePosition = averagePosition / (objectsToShow.Count + 2);
		
		distanceBetPlayers = Vector3.Distance(playerOnePos, playerTwoPos);
		
		percentMaxDistance = distanceBetPlayers/maxDistanceBetweenPlayers;
		
		percentMaxDistance = Mathf.Clamp(percentMaxDistance, 0.0f, 1.0f);

		positionToMoveTowards = new Vector3(averagePosition.x, transform.position.y, averagePosition.z);
		
		panSpeed = maxPanSpeed * (1 - percentMaxDistance);
		
		panSpeed = Mathf.Clamp(panSpeed, minPanSpeed, maxPanSpeed);
		
		transform.position = Vector3.MoveTowards(transform.position, positionToMoveTowards, panSpeed);
	}
	
	public void ActivateOutwardZoom(List<GameObject> incomingGameobjects)
	{
		showingSomething = true;
		objectsToShow = incomingGameobjects;
		showingSomethingTimer = showingSomethingTime;
	}
	
}
