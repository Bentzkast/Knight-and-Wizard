using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour {

    public enum TileType
	{
		Wall,Floor,
	}
    
	public int col = 100;
	public int rows = 100;
	public ValueRange numRooms = new ValueRange(15, 20);
	public ValueRange roomWidth = new ValueRange(3, 10);
	public ValueRange roomHeight = new ValueRange(3, 10);
	public ValueRange corridorLength = new ValueRange(6, 10);
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] topWallTiles;
	public GameObject[] topFloorTiles;
	public GameObject[] obstacleTiles;
	public GameObject[] pickUp;
	public GameObject[] enemy;
	public GameObject[] bosses;
   
	public GameObject player;

	private bool bossSpawned = false;
	private bool enemySpawned = false;
	private TileType[][] tiles;
	private Room[] rooms;
	private Corridor[] corridors;
	private GameObject boardHolder;
	private int level = 0;

	public void SetupGameBoard(int level)
	{
		bossSpawned = true;
		enemySpawned = false;
		if(level == 5){
			bossSpawned = false;
		}
		this.level = level;
		boardHolder = new GameObject("BoardHolder");
		SetupTilesArray();
		CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
		SetTileValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();

	}

    void SetupTilesArray()
	{
		tiles = new TileType[col][];
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TileType[rows];
			for (int j = 0; j < tiles.Length;j++)
			{
				tiles[i][j] = TileType.Wall;
			}
		}
	}

	void CreateRoomsAndCorridors()
	{
		rooms = new Room[numRooms.Random];
		corridors = new Corridor[rooms.Length - 1];

		rooms[0] = new Room();
		corridors[0] = new Corridor();
		rooms[0].SetupRoom(roomWidth, roomHeight, col, rows);
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, col, rows, true);
		for (int i = 1; i < rooms.Length; i++)
		{
			rooms[i] = new Room();
			rooms[i].SetupRoom(roomWidth, roomHeight, col, rows, corridors[i - 1]);
			if (i < corridors.Length)
			{
				corridors[i] = new Corridor();
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, col, rows, false);
			}
			if (i == rooms.Length /2 )
            {
                Vector3 playerPos = new Vector3(rooms[i].xPos, rooms[i].yPos, 0);
                Instantiate(player, playerPos, Quaternion.identity);
            }
		}
	}

    void SetTilesValuesForRooms()
	{
		for (int i = 0; i < rooms.Length;i++)
		{
			Room currentRoom = rooms[i];
			for (int j = 0; j < currentRoom.roomWidth;j++)
			{
				int xCoord = currentRoom.xPos + j;
				for (int k = 0; k < currentRoom.roomHeight;k++)
				{
					int yCoord = currentRoom.yPos + k;
					tiles[xCoord][yCoord] = TileType.Floor;
				}
			}
		}
	}

    void SetTileValuesForCorridors()
	{
		for (int i = 0; i < corridors.Length;i++)
		{
			Corridor currentCorridor = corridors[i];
			for (int j = 0; j < currentCorridor.corridorLength;j++)
			{
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

				tiles[xCoord][yCoord] = TileType.Floor;
			}
		}
	}

    void InstantiateTiles()
	{
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length;j++)
			{
				if (tiles[i][j] == TileType.Wall && j > 0 && tiles[i][j - 1] == TileType.Floor)
					InstantiateFromArray(topWallTiles, i, j);
				else if (tiles[i][j] == TileType.Floor)
				{
					if (j + 1 < tiles[i].Length && tiles[i][j + 1] == TileType.Wall)
						InstantiateFromArray(topFloorTiles,i,j);
					else
					    InstantiateFromArray(floorTiles, i, j);
					float random = Random.Range(0f, 1f);

					if(!bossSpawned){
            			InstantiateFromArray(bosses, i, j);
            			bossSpawned = true;
					}
					else if(!enemySpawned){
						InstantiateFromArray(enemy, i, j);
						enemySpawned = true;
					}
					else
					{
						if (random <= .01f + (level) * .01f)
                        {
                            InstantiateFromArray(enemy, i, j);
                        }
                        else if (random <= .02f + level * .02f)
                        {
                            InstantiateFromArray(obstacleTiles, i, j);
                        }
					}               
				}

				else if (tiles[i][j] == TileType.Wall)
					InstantiateFromArray(wallTiles, i, j);
			}
		}
	}

	void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
		float rightEdgeX = col + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray(wallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
			InstantiateFromArray(wallTiles, currentX, yCoord);

            currentX++;
        }
    }

	void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
}
