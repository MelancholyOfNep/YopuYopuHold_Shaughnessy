using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	YopuSet yopuSet;
	[SerializeField]
	GameManager manager;

	void Start()
	{
		yopuSet = GetComponent<YopuSet>();
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void Update()
	{
		if (manager.paused == false)
        {
			if (Input.GetKeyUp(KeyCode.DownArrow))
				yopuSet.dropRate = yopuSet.regularTime;
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
				yopuSet.MoveInput("Left");
			else if (Input.GetKeyDown(KeyCode.RightArrow))
				yopuSet.MoveInput("Right");
			else if (Input.GetKeyDown(KeyCode.Z))
				yopuSet.RotateInput("Left");
			else if (Input.GetKeyDown(KeyCode.X))
				yopuSet.RotateInput("Right");
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (manager.swapped == true)
					yopuSet.DisableSetHoldSwap();
				else if (manager.swapped == false)
				{
					yopuSet.DisableSetHoldSpawn();
					manager.swapped = true;
				}
			}
			if (Input.GetKey(KeyCode.DownArrow))
				yopuSet.dropRate = yopuSet.doubleTime;
		}
	}
}
