using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Moving_Object,IDamageable {

    public Animator knightAnimator;
	public GameObject weaponSlot;
	public float restartLvlDelay = 1f;
	public Sprite defaultWeaponSprite;

	private Animator _weaponAnimator;
	private PlayerStats _playerStats;
	private SpriteRenderer _weaponSpriteRenderer;
	private bool m_isAxisInUse = false;

       
	protected override void Start()
	{
		_weaponAnimator = weaponSlot.GetComponent<Animator>();
        _weaponSpriteRenderer = weaponSlot.GetComponent<SpriteRenderer>();
        _playerStats = GameManager.instanceGM.playerStats;
		base.Start();
	}

	private void OnDisable()
	{
        GameManager.instanceGM.playerStats = _playerStats;
	}

    private void CheckIfGameOver()
    {
        if(_playerStats.hitPoints <= 0)
        {
            Debug.Log("Game Over");
			gameObject.SetActive(false);
			GameManager.instanceGM.GameOver();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

	public Damage TakeDamage(Damage damage)
    {
        
		int finalDamage = damage.rawDamage - _playerStats.GetBlockValue();
		if(finalDamage > 0)
		{
			_playerStats.hitPoints -= finalDamage;
			knightAnimator.SetTrigger("Knight_hit");
			CheckIfGameOver();
		}      
		return null;
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if(other.tag == "exit"){
            Invoke("Restart", restartLvlDelay);
            enabled = false;
        }
        else if(other.tag == "Equipment"){
            Debug.Log("Picked up Equipment");
			Equipment equipment = other.GetComponent<PickUp>().GetPickUpEquipment();
			if (equipment.equipmentType == Equipment.Type.Weapon){
				_playerStats.weaponSlot = equipment;
				_weaponSpriteRenderer.sprite = equipment.icon;
			}
				
			else if (equipment.equipmentType == Equipment.Type.Armor)
				_playerStats.armorSlot = equipment;
			else if (equipment.equipmentType == Equipment.Type.Other)
				_playerStats.otherSlot = equipment;
            
			other.gameObject.SetActive(false);
            
        }
       
	}

	protected override void OnCantMove<T>(T component)
	{
		IDamageable hitEnemy = component as IDamageable;

		//Debug.Log(hitEnemy.gameObject.name);
        // ensure combat

		Damage counter = hitEnemy.TakeDamage(new Damage(_playerStats.weaponSlot ? _playerStats.weaponSlot.attackValue : _playerStats.defaultAttack,0));
		if(_playerStats.weaponSlot != null)
		    _playerStats.weaponSlot.durabilty -= counter.duraDamage;


        knightAnimator.SetTrigger("Knight_attack");
		_weaponAnimator.SetTrigger("Weapon_attack");
		TakeDamage(counter);
	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
        _playerStats.movementValue--;
        base.AttemptMove<T>(xDir, yDir);
        if (_playerStats.movementValue <= 0){
            GameManager.instanceGM.isPlayerTurn = false;
			_playerStats.movementValue = _playerStats.maxMovement;
        }

	}

	private void Update()
	{
		if(_playerStats.weaponSlot != null && _playerStats.weaponSlot.durabilty == 0){
			_weaponSpriteRenderer.sprite = defaultWeaponSprite;
			_playerStats.weaponSlot = null;
		}
        if (!GameManager.instanceGM.isPlayerTurn) return;
        if (isMoving) return;

        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0) vertical = 0;
		if ((horizontal != 0 || vertical != 0) && m_isAxisInUse == false)
        {
			isMoving = true;
			m_isAxisInUse = true;
			//Debug.Log("Attempt move" + isMoving);
			AttemptMove<IDamageable>(horizontal, vertical);
        }
		if(vertical == 0 && horizontal == 0){
			m_isAxisInUse = false;
		}
	}

	public PlayerStats GetStats(){
		return _playerStats;
	}
}
