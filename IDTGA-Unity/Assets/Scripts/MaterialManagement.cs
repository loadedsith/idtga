using UnityEngine;
using System.Collections;

public class MaterialManagement : MonoBehaviour {
	
	public bool materialIsChangeableWhilePlaying = false;
	public float scaleFactor = 1;
	public bool yAxisIsUp = true;
	public bool manualTileControl = false;
	public float manualXScale = 1;
	public float manualYScale = 1;
	public float manualXOffset = 0;
	public float manualYOffset = 0;
	
	void Start ()
	{
		this.renderer.material.mainTextureOffset = new Vector2(manualXOffset, manualYOffset);
		if(manualTileControl == true)
		{
			this.renderer.material.mainTextureScale = new Vector2(manualXScale, manualYScale);
		}
		else
		{
			if(yAxisIsUp == true)
			{
				this.renderer.material.mainTextureScale = new Vector2(transform.localScale.x/scaleFactor, transform.localScale.y/scaleFactor);
			}
			else
			{
				this.renderer.material.mainTextureScale = new Vector2(transform.localScale.x/scaleFactor, transform.localScale.z/scaleFactor);
			}
		}

		
	}
	

	void Update ()
	{
		if(materialIsChangeableWhilePlaying)
		{
			this.renderer.material.mainTextureOffset = new Vector2(manualXOffset, manualYOffset);
			if(manualTileControl == true)
			{
				this.renderer.material.mainTextureScale = new Vector2(manualXScale, manualYScale);
			}
			else
			{
				if(yAxisIsUp == true)
				{
					this.renderer.material.mainTextureScale = new Vector2(transform.localScale.x/scaleFactor, transform.localScale.y/scaleFactor);
				}
				else
				{
					this.renderer.material.mainTextureScale = new Vector2(transform.localScale.x/scaleFactor, transform.localScale.z/scaleFactor);
				}
			}
		}
		
	}
}
