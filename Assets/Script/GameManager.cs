using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm_instance = null;
    public float level_start_delay = 0.1f;
    public float turn_delay = 0.5f;
    public PlayerStats player_stats;

    private BoardManager board_script;
    private List<Enemy> enemies;
    private bool enemies_moving;
    [HideInInspector]public bool player_turn = true;

	private void Awake()
	{
        if(gm_instance == null){
            gm_instance = this;
        }
        else if(gm_instance != null){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        board_script = GetComponent<BoardManager>();
        InitGame();
	}


	private void InitGame(){
        enemies.Clear();
        board_script.SetupGameBoard();
    }
	private void Update()
	{
        if (player_turn || enemies_moving) return;
        StartCoroutine(MoveEnemies());
	}
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    public void RemoveEnemyFromList(Enemy script){
        enemies.Remove(script);
    }

    public void GameOver()
    {
        enabled = false;
    }
    IEnumerator MoveEnemies()
    {
        enemies_moving = true;
        yield return new WaitForSeconds(turn_delay);
        if(enemies.Count == 0){
            yield return new WaitForSeconds(turn_delay);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].move_time);
        }

        player_turn = true;
        enemies_moving = false;
    }
}
