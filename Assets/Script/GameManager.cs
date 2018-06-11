using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instanceGM = null;
	public float levelStartDelay = 20f;
	public float exitLevelDelay = 2f;
	public float turnDelay = 0.5f;
    public PlayerStats playerStats;
	public bool gameIsOver;
	public bool gameIsWon = false;
	public int level = 0;
	public bool doingSetup;

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
	}
    

	private void InitGame(){
		level++;
		gameIsWon = false;
		gameIsOver = false;
		PlayerUI ui = GameObject.FindWithTag("UIManager").GetComponent<PlayerUI>();
		ui.SetExitButton(false);
		ui.SetLevelImage(level, "Eliminated All enemy to advance");
   
        enemiesList.Clear();
        _boardScript.SetupGameBoard(level);
		Invoke("UiHide", levelStartDelay);
    }
	void UiHide(){
		PlayerUI ui = GameObject.FindWithTag("UIManager").GetComponent<PlayerUI>();
        ui.HideLevelImage();
	}

	private void Update()
	{
		if (isPlayerTurn || enemiesIsMoving || doingSetup) return;
        StartCoroutine(MoveEnemies());
	}
    public void AddEnemyToList(Enemy script)
    {
        enemiesList.Add(script);
    }
    public void RemoveEnemyFromList(Enemy script){
        enemiesList.Remove(script);
		if(enemiesList.Count <= 0)
		{
			gameIsOver = true;
			gameIsWon = true;
			if(level == 5){
				Invoke("Redo", exitLevelDelay);
				return;
			}
			PlayerUI ui = GameObject.FindWithTag("UIManager").GetComponent<PlayerUI>();
			ui.SetExitButton(true);
		}
    }

	public void playerDied(){
		gameIsOver = true;
		gameIsWon = false;
		GameOver();

	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("called");

        InitGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

	public int GetEnemyCount (){
		return enemiesList.Count;
	}

	public void Restart(){

		SceneManager.LoadScene(0);
	}
	public void Redo(){
		level = 0;
        playerStats.hitPoints = 10;
		SceneManager.LoadScene(0);
	}

	public void GameOver()
    {
		if(gameIsWon)
		    Invoke("Restart", exitLevelDelay);
		else
		{         
		    Invoke("Redo", exitLevelDelay);
		}
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
