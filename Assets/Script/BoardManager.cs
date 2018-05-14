using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {


	public class ValueRange
	{
		public int min;
		public int max;
		public ValueRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
	}

	public GameObject[] topWallTiles;
	public GameObject[] sideWallTiles;
	public GameObject[] floorTopTiles;
	public GameObject[] floorSideTiles;
	public GameObject[] floorTiles;
	public GameObject[] boxItem;
	public GameObject[] pickUpItem;
	public ValueRange boxAmount;
	public ValueRange pickUpAmount;
    public int boardCol = 8;
	public int boardRow = 8;

    private Transform board_holder;
    private List<Vector3> grid_positions = new List<Vector3>();

    // Create play area
    void InitializeList(){
        grid_positions.Clear();
        for (int i = 1; i < boardCol - 1; i++)
        {
            for (int j = 1; j < boardRow - 1; j++)
            {
                grid_positions.Add(new Vector3(i, j, 0f));
            }
        }
    }

    // get random available position
	private Vector3 RandomPosition()
    {
        int random_index = Random.Range(0, grid_positions.Count);

        Vector3 random_position = grid_positions[random_index];

        grid_positions.RemoveAt(random_index);

        return random_position;
    }

    // set specified object at random
	void LayoutObjectAtRandom(GameObject[] object_tile_array, int min, int max)
    {
        int object_count = Random.Range(min, max);

        for (int i = 0; i < object_count; i++)
        {
            Vector3 random_position = RandomPosition();
            GameObject tile_to_instantiate = object_tile_array[Random.Range(0, object_tile_array.Length)];

            GameObject tile_instance = Instantiate(tile_to_instantiate, random_position, Quaternion.identity);
        }
    }

    void BoardSetup(){
        board_holder = new GameObject("Board").transform;

        for (int i = -1; i < boardCol + 1; i++)
        {
            for (int j = -1; j < boardRow + 1; j++)
            {
                GameObject tile_to_instantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (i == boardCol || i == -1 || j == -1)
                {
                    tile_to_instantiate = sideWallTiles[Random.Range(0, sideWallTiles.Length)];
                }
                else if (j == boardRow)
                {
                    tile_to_instantiate = topWallTiles[Random.Range(0, topWallTiles.Length)];
                }
                else if( j == boardRow -1)
                {
                    tile_to_instantiate = floorTopTiles[Random.Range(0, floorTopTiles.Length)];
                }


                GameObject tile_instance = Instantiate(tile_to_instantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                tile_instance.transform.SetParent(board_holder);
            }
        }
    }
    public void SetupGameBoard(){
        BoardSetup();
        InitializeList();
		boxAmount = new ValueRange(2,4);
		pickUpAmount = new ValueRange(1, 2);

		LayoutObjectAtRandom(boxItem, boxAmount.min, boxAmount.max);
		LayoutObjectAtRandom(pickUpItem, pickUpAmount.min, pickUpAmount.max);

    }
}
