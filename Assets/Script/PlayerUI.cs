using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Text playerHp;
	public Text playerAttack;
	public Text playerBlock;
	public Text move;

	public Image attackItem;
	public Image armorItem;
	public Image otherItem;

	Player _player;
	PlayerStats _stats;

	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		_stats = _player.GetStats();
	}
       
	// Update is called once per frame
	void Update()
	{
		if(_stats != null){
			move.text = "Move:" + _stats.movementValue.ToString() + "/" + _stats.maxMovement.ToString();
			playerHp.text = "Health :" + _stats.hitPoints.ToString();
			playerAttack.text = "Attack : " + _stats.GetAttackValue().ToString();
			playerBlock.text = "Block : " + _stats.GetBlockValue().ToString();
			attackItem.sprite = _stats.weaponSlot != null ? _stats.weaponSlot.icon : _player.defaultWeaponSprite;
			armorItem.sprite = _stats.armorSlot != null ? _stats.armorSlot.icon : null;
            otherItem.sprite = _stats.otherSlot != null ? _stats.otherSlot.icon : null;
		}

	}
}
