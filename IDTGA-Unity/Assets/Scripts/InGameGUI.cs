using UnityEngine;
using System.Collections;

public class InGameGUI : MonoBehaviour {
	
	
	public GameManager gameManagerReference;
	public bool graphicsAreChangeableWhilePlaying = false;
	
	public Texture playerOnePortrait;
	public Texture playerTwoPortrait;
	public Texture energyBarTexture;
	public Texture energyBarFrame;
	
	public float portraitWidth;
	public float portraitHeight;
	public float portraitMarginSides;
	public float portraitYPositionOffset;
	
	public float currentEnergy;
	public float maxEnergy;
	public float energyRechargeRate;
	private float currentPercentOfMaxEnergy;
	
	public float energyBarHeight;
	public float energyFrameHeight;
	public float maxEnergyBarWidth;
	public float energyFrameWidth;
	public float currentEnergyBarWidth;
	public float energyBarYPositionOffset;
	public float energyFrameYPositionOffset;
	
	// Use this for initialization
	void Start ()
	{
		
		//These values will always operate in percentages of the total screen width or height
		portraitWidth = Screen.width * (portraitWidth / 100);
		portraitHeight = Screen.height * (portraitHeight / 100);
		portraitMarginSides = Screen.width * (portraitMarginSides / 100);
		portraitYPositionOffset = Screen.height * (portraitYPositionOffset / 100);
		
		energyBarHeight = Screen.height * (energyBarHeight / 100);
		maxEnergyBarWidth = Screen.width * (maxEnergyBarWidth / 100);
		currentEnergyBarWidth = maxEnergyBarWidth;
		energyBarYPositionOffset = Screen.height * (energyBarYPositionOffset / 100);
		energyFrameYPositionOffset = Screen.height * (energyFrameYPositionOffset / 100);
		energyFrameHeight = Screen.height * (energyFrameHeight / 100);
		energyFrameWidth = Screen.width * (energyFrameWidth / 100);
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
		currentPercentOfMaxEnergy = currentEnergy / maxEnergy;
		currentPercentOfMaxEnergy = Mathf.Clamp(currentPercentOfMaxEnergy, 0, 1);
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
		Graphics.DrawTexture(new Rect(Screen.width/2-energyFrameWidth/2, energyFrameYPositionOffset, energyFrameWidth, energyFrameHeight), energyBarFrame);
	}
	
	public void IncreaseEnergy(int energyIncrease)
	{
		currentEnergy+= energyIncrease;
	}
}
