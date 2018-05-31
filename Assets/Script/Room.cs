using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

	public int xPos;
	public int yPos;
	public int roomWidth;
	public int roomHeight;
	public Direction enteringCorridor;

	public void SetupRoom(ValueRange widthRange, ValueRange heightRange, int col,int row)
	{
		roomWidth = widthRange.Random;
		roomHeight = heightRange.Random;

		xPos = Mathf.RoundToInt(col / 2f - roomWidth / 2f);
		yPos = Mathf.RoundToInt(row / 2f - roomHeight / 2f);
	}

	public void SetupRoom(ValueRange widthRange, ValueRange heightRange, int col, int row, Corridor corridor)
	{
		enteringCorridor = corridor.direction;

		roomWidth = widthRange.Random;
		roomHeight = heightRange.Random;

		switch(corridor.direction)
		{
			case Direction.North:
				roomHeight = Mathf.Clamp(roomHeight, 1, row - corridor.EndPositionY);
				yPos = corridor.EndPositionY;
				xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
				xPos = Mathf.Clamp(xPos, 0, col - roomWidth);
				break;
			case Direction.East:
				roomWidth = Mathf.Clamp(roomWidth, 1, col - corridor.EndPositionX);
				xPos = corridor.EndPositionX;
                   
				yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
				yPos = Mathf.Clamp(yPos, 0, row - roomHeight);
				break;
			case Direction.South:
				roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndPositionY);
				yPos = corridor.EndPositionY - roomHeight + 1;
				xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
				xPos = Mathf.Clamp(xPos, 0, col - roomWidth);
				break;
			case Direction.West:
				roomWidth = Mathf.Clamp(roomWidth, 1, corridor.EndPositionX);
				xPos = corridor.EndPositionX - roomWidth + 1;
				yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
				yPos = Mathf.Clamp(yPos, 0, row - roomHeight);
				break;

		}

	}
}
