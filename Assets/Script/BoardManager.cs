using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    public GameObject[] top_wall_tiles;
    public GameObject[] side_wall_tiles;
    public GameObject[] floor_top_tiles;
    public GameObject[] floor_side_tiles;
    public GameObject[] floor_tiles;
    public int board_col = 8;
    public int board_row = 8;

    private Transform board_holder;
    private List<Vector3> grid_positions = new List<Vector3>();

    // Create play area
    void InitializeList(){
        grid_positions.Clear();
        for (int i = 1; i < board_col - 1; i++)
        {
            for (int j = 1; j < board_row - 1; j++)
            {
                grid_positions.Add(new Vector3(i, j, 0f));
            }
        }
    }

    // get random available position
    Vector3 randomPosition()
    {
        int random_index = Random.Range(0, grid_positions.Count);

        Vector3 random_position = grid_positions[random_index];

        grid_positions.RemoveAt(random_index);

        return random_position;
    }

    // set specified object at random
    void layoutObjectAtRandom(GameObject[] object_tile_array, int min, int max)
    {
        int object_count = Random.Range(min, max);

        for (int i = 0; i < object_count; i++)
        {
            Vector3 random_position = randomPosition();
            GameObject tile_to_instantiate = object_tile_array[Random.Range(0, object_tile_array.Length)];

            GameObject tile_instance = Instantiate(tile_to_instantiate, random_position, Quaternion.identity);
        }
    }

    void BoardSetup(){
        board_holder = new GameObject("Board").transform;

        for (int i = -1; i < board_col + 1; i++)
        {
            for (int j = -1; j < board_row + 1; j++)
            {
                GameObject tile_to_instantiate = floor_tiles[Random.Range(0, floor_tiles.Length)];

                if (i == board_col || i == -1 || j == -1)
                {
                    tile_to_instantiate = side_wall_tiles[Random.Range(0, side_wall_tiles.Length)];
                }
                else if (j == board_row)
                {
                    tile_to_instantiate = top_wall_tiles[Random.Range(0, top_wall_tiles.Length)];
                }
                else if( j == board_row -1)
                {
                    tile_to_instantiate = floor_top_tiles[Random.Range(0, floor_top_tiles.Length)];
                }


                GameObject tile_instance = Instantiate(tile_to_instantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                tile_instance.transform.SetParent(board_holder);
            }
        }
    }
    public void SetupGameBoard(){
        BoardSetup();
        InitializeList();
    }
}
