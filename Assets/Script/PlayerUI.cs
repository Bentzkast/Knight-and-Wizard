using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

	public Text playerHp;
	public Text playerAttack;
	public Text playerBlock;
	public Text move;
	public Text whosTurn;
	public Text gameOver;
	public Text enemyLeft;

	public Image attackItem;
	public Image armorItem;
	public Image otherItem;
	public Sprite empty;

	public Text attackDura;
	public Text armorDura;
	public Text otherDura;

	public GameObject levelImage;
	public Text levelText;
	public Text objectiveText;
	public Button exitLevel;

	[HideInInspector] public Player _player;
	PlayerStats _stats;


	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		gameOver.gameObject.SetActive(false);
		exitLevel.onClick.AddListener(ExitLevel);
	}

	void ExitLevel()
	{
		if (GameManager.instanceGM.gameIsWon)
			GameManager.instanceGM.playerStats = _stats;
		GameManager.instanceGM.GameOver();
	}

	public void SetLevelImage(int level, string obj)
	{
		GameManager.instanceGM.doingSetup = true;
		levelText.text = "Level" + level.ToString();
		objectiveText.text = obj;
		levelImage.SetActive(true);
	}

	public void SetExitButton(bool b)
	{
		exitLevel.gameObject.SetActive(b);
	}
    
	public void HideLevelImage(){
		exitLevel.gameObject.SetActive(false);
		levelImage.SetActive(false);
		GameManager.instanceGM.doingSetup = false;

	}

	// Update is called once per frame
	void Update()
	{
		if (_player) _stats = _player.GetStats();
		else _stats = null;

		if(_stats != null){
			
			move.text = "";
			if (GameManager.instanceGM.isPlayerTurn)
				move.text = "Move:" + _stats.movementValue.ToString() + "/" + _stats.maxMovement.ToString();
			playerHp.text = "Health :" + _stats.hitPoints.ToString();
			playerAttack.text = "Attack : " + _stats.GetAttackValue().ToString();
			playerBlock.text = "Block : " + _stats.GetBlockValue().ToString();
			if(_stats.weaponSlot != null)
			{
				attackItem.sprite = _stats.weaponSlot.icon;
				attackDura.text = _stats.weaponSlot.durability.ToString();
			}
			else{
				attackItem.sprite = _player.defaultWeaponSprite;
				attackDura.text = "\u221e";
			}
			if(_stats.armorSlot != null){
				armorItem.sprite = _stats.armorSlot.icon;
				armorDura.text = _stats.armorSlot.durability.ToString();
			}
			else{
				armorItem.sprite = empty;
				armorDura.text = "";
			}
			if(_stats.otherSlot != null){
				otherItem.sprite = _stats.otherSlot.icon;
				otherDura.text = _stats.otherSlot.durability.ToString();
			}
			else{
				otherItem.sprite = empty;
				otherDura.text = "";
			}
		}
		if(GameManager.instanceGM.isPlayerTurn){
			whosTurn.text = "Player Turn";
		}
		else{
			whosTurn.text = "Enemy Turn";
		}
		if(GameManager.instanceGM.gameIsOver){
			if (GameManager.instanceGM.gameIsWon && GameManager.instanceGM.level == 5)
			{
				gameOver.text = "Finished Demo buy full version to proceed, Congrats " + GameManager.instanceGM.level;
				gameOver.color = Color.green;
                gameOver.gameObject.SetActive(true);
			}else if(GameManager.instanceGM.gameIsWon){
				gameOver.text = "Passed LV " + GameManager.instanceGM.level;
                gameOver.color = Color.yellow;
                gameOver.gameObject.SetActive(true);
			}
			else
			{
				gameOver.text = "You died at LV " + (GameManager.instanceGM.level);
				gameOver.color = Color.red;
				gameOver.gameObject.SetActive(true);
			}
		}
		else{
			gameOver.gameObject.SetActive(false);
		}
		enemyLeft.text = GameManager.instanceGM.GetEnemyCount() + "Enemy Left";
	}
}
