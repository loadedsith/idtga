using UnityEngine;
using System.Collections;

public class InGameGUI : MonoBehaviour {
	
	
	public GameManager gameManagerReference;
	public bool graphicsAreChangeableWhilePlaying = false;
	
	public Texture playerOnePortrait;
	public Texture playerTwoPortrait;
	public Texture energyBarTexture;
	
	public float portraitWidth;
	public float portraitHeight;
	public float portraitMarginSides;
	public float portraitYPositionOffset;
	
	public float currentEnergy;
	public float maxEnergy;
	public float energyRechargeRate;
	private float currentPercentOfMaxEnergy;
	
	public float energyBarHeight;
	public float maxEnergyBarWidth;
	public float currentEnergyBarWidth;
	public float energyBarYPositionOffset;
	
	// Use this for initialization
	void Start ()
	{
		currentEnergy = maxEnergy/4; //How much energy do we want the players to start with?
		
		
		//These values will always operate in percentages of the total screen width or height
		portraitWidth = Screen.width * (portraitWidth / 100);
		portraitHeight = Screen.height * (portraitHeight / 100);
		portraitMarginSides = Screen.width * (portraitMarginSides / 100);
		portraitYPositionOffset = Screen.height * (portraitYPositionOffset / 100);
		
		energyBarHeight = Screen.height * (energyBarHeight / 100);
		maxEnergyBarWidth = Screen.width * (maxEnergyBarWidth / 100);
		currentEnergyBarWidth = maxEnergyBarWidth;
		energyBarYPositionOffset = Screen.height * (energyBarYPositionOffset / 100);
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentPercentOfMaxEnergy = currentEnergy / maxEnergy;
		currentPercentOfMaxEnergy = Mathf.Clamp(currentPercentOfMaxEnergy, 0, 1);
		//Debug.Log("currentPercentOfMaxEnergy: "+currentPercentOfMaxEnergy);
		currentEnergyBarWidth = maxEnergyBarWidth * currentPercentOfMaxEnergy;
	}
	
	void OnGUI()
	{
		//Portraits turned off
		//Graphics.DrawTexture(new Rect(portraitMarginSides, portraitYPositionOffset, portraitWidth, portraitHeight), playerOnePortrait);
		//Graphics.DrawTexture(new Rect(Screen.width - portraitMarginSides - portraitWidth, portraitYPositionOffset, portraitWidth, portraitHeight), playerTwoPortrait);
		if(currentEnergyBarWidth <= maxEnergyBarWidth)
		{
			Graphics.DrawTexture(new Rect(Screen.width/2-currentEnergyBarWidth/2, energyBarYPositionOffset, currentEnergyBarWidth, energyBarHeight), energyBarTexture);
		}
		else
		{
			Graphics.DrawTexture(new Rect(Screen.width/2-maxEnergyBarWidth/2, energyBarYPositionOffset, maxEnergyBarWidth, energyBarHeight), energyBarTexture);
		}
	}
	
	public void IncreaseEnergy(int energyIncrease)
	{
		currentEnergy+= energyIncrease;
	}
}
