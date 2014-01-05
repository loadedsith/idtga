using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {
	public GUISkin laserMenuSkin;
	public float buttonWidth;
	public float buttonHeight;
	public float buttonYPositionOffset;
	public float boxHeight;
	public float boxWidth;
	
	public enum MenuScreens
	{
		Main,
		Instructions,
		Credits,
		End,
	};
	
	public MenuScreens menuState;
		
	// Use this for initialization
	void Start ()
	{
		//These values will always operate in percentages of the total screen width or height
		buttonWidth = Screen.width * (buttonWidth/100);
		buttonHeight = Screen.height * (buttonHeight/100);
		buttonYPositionOffset = Screen.height * (buttonYPositionOffset/100);
		
		boxWidth = Screen.width * (boxWidth/100);
		boxHeight = Screen.height * (boxHeight/100);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnGUI()
	{
		GUI.skin = laserMenuSkin;
		
		switch(menuState)
		{
		case MenuScreens.Main:
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 - buttonYPositionOffset * 1.5f, buttonWidth, buttonHeight), "Descend"))
			{
				Application.LoadLevel(Application.loadedLevel+1);
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 - (buttonYPositionOffset / 2), buttonWidth, buttonHeight), "Instructions"))
			{
				menuState = MenuScreens.Instructions;
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + (buttonYPositionOffset / 2), buttonWidth, buttonHeight), "Credits"))
			{
				menuState = MenuScreens.Credits;
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + buttonYPositionOffset * 1.5f, buttonWidth, buttonHeight), "Quit"))
			{
				Application.Quit();
			}
			break;
		case MenuScreens.Instructions:
			GUI.Box(new Rect(Screen.width/2-boxWidth/2,Screen.height/2-boxHeight/2,boxWidth,boxHeight),
				"\n\nHere are the instructions:\n\n" +
				"For Player One:\n" +
				"W,A,S & D for movement\n" +
				"E key to interact\n\n" +
				"For Player Two:\n" +
				"Up, Down, Left, and Right arrow keys for movement\n" +
				"Shift key to interact\n\n" +
				"Shoot your laser with the Space Bar!\n" +
				"Don't forget to revive each other by bumping into your ally!");
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + (buttonYPositionOffset * 2), buttonWidth, buttonHeight), "Back"))
			{
				menuState = MenuScreens.Main;
			}
			break;
		case MenuScreens.Credits:
			GUI.Box(new Rect(Screen.width/2-boxWidth/2,Screen.height/2-boxHeight/2,boxWidth,boxHeight),
				"\n\nCREDITS\n" +
				"In Aplhabetical Order:\n" +
				"Graham Heath: Programming, Design\n" +
				"Blair Kuhlman: Design, Visual Effects\n" +
				"Jeremy Mack: Art and Animation\n" +
				"Jacob Mooney: Design and Sound");
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + (buttonYPositionOffset * 2), buttonWidth, buttonHeight), "Back"))
			{
				menuState = MenuScreens.Main;
			}
			break;
		case MenuScreens.End:
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 - buttonYPositionOffset / 2, buttonWidth, buttonHeight), "Main Menu"))
			{
				Application.LoadLevel(0);
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + buttonYPositionOffset / 2, buttonWidth, buttonHeight), "Quit"))
			{
				Application.Quit();
			}
			break;
		default:
			GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 - buttonYPositionOffset * 1.5f, buttonWidth, buttonHeight), "Descend");
			GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 - (buttonYPositionOffset / 2), buttonWidth, buttonHeight), "Instructions");
			GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + (buttonYPositionOffset / 2), buttonWidth, buttonHeight), "Credits");
			GUI.Button(new Rect(Screen.width/2-buttonWidth/2, Screen.height/2-buttonHeight/2 + buttonYPositionOffset * 1.5f, buttonWidth, buttonHeight), "Quit");
			break;
		}
		
	}
}
