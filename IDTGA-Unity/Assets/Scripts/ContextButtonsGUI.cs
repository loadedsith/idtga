using UnityEngine;
using System.Collections;

public class ContextButtonsGUI : MonoBehaviour {
	
	
	public GUISkin laserContextSkin;
	public bool playerIsNear; // This bool is just a substitute for OnTriggerStay, and will get deleted later
	public bool isPlayerOne; //For this sample code, this will be how we tell the players apart
	
	public float imageWidth;
	public float imageHeight;
	public float imageMarginSides;
	public float imageYPositionOffset;
	
	public enum ContextClueSet  //A list of the contexts the players can be in. For now we only have switches, right?
	{
		UseSwitch,
	}
	
	public ContextClueSet currentContext;
	
	void Start ()
	{
		//These values will always operate in percentages of the total screen width or height
		imageWidth = Screen.width * (imageWidth / 100);
		imageHeight = Screen.height * (imageHeight / 100);
		imageMarginSides = Screen.width * (imageMarginSides / 100);
		imageYPositionOffset = Screen.height * (imageYPositionOffset / 100);
	}
	
	void Update ()
	{
	
	}
	
	void OnGUI()
	{
		GUI.skin = laserContextSkin;
		if(playerIsNear)
		{
			switch(currentContext)
			{
			case ContextClueSet.UseSwitch:  //If in the context of a switch, display the code cluing the players to use the appropriate key to interact
				if(isPlayerOne)
				{
					GUI.Label(new Rect(imageMarginSides, imageYPositionOffset, imageWidth, imageHeight), "E = Switch");
				}
				else
				{
					GUI.Label(new Rect(Screen.width - imageMarginSides - imageWidth, imageYPositionOffset, imageWidth, imageHeight), "SHIFT = Switch");
				}
				break;
			}
		}

	}
}
