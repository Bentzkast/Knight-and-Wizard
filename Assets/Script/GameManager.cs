using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instanceGM = null;
	public float levelStartDelay = 0.1f;
	public float turnDelay = 0.5f;
    public PlayerStats playerStats;
	public bool gameIsOver;

	private BoardCreator _boardScript;
	private List<Enemy> enemiesList;
	private bool enemiesIsMoving;
    [HideInInspector]public bool isPlayerTurn = true;

	private void Awake()
	{
        if(instanceGM == null){
            instanceGM = this;
        }
        else if(instanceGM != null){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemiesList = new List<Enemy>();
		_boardScript = GetComponent<BoardCreator>();
        InitGame();
		gameIsOver = false;
	}


	private void InitGame(){
        enemiesList.Clear();
        _boardScript.SetupGameBoard();
    }
	private void Update()
	{
        if (isPlayerTurn || enemiesIsMoving) return;
        StartCoroutine(MoveEnemies());
	}
    public void AddEnemyToList(Enemy script)
    {
        enemiesList.Add(script);
    }
    public void RemoveEnemyFromList(Enemy script){
        enemiesList.Remove(script);
    }

    public void GameOver()
    {
        enabled = false;
		gameIsOver = true;
    }
    IEnumerator MoveEnemies()
    {
        enemiesIsMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemiesList.Count == 0){
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].MoveEnemy();
            yield return new WaitForSeconds(enemiesList[i].move_time);
        }

        isPlayerTurn = true;
        enemiesIsMoving = false;
    }
}
