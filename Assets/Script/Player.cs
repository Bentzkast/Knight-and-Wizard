using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Moving_Object,IDamageable {

    public Animator knightAnimator;
	public GameObject weaponSlot;
	public float restartLvlDelay = 1f;
	public Sprite defaultWeaponSprite;
	public GameObject popUpGO;

	private Animator _weaponAnimator;
	private PlayerStats _playerStats;
	private SpriteRenderer _weaponSpriteRenderer;
	private bool m_isAxisInUse = false;


	private void Awake()
	{
		_weaponAnimator = weaponSlot.GetComponent<Animator>();
        _weaponSpriteRenderer = weaponSlot.GetComponent<SpriteRenderer>();
        _playerStats = GameManager.instanceGM.playerStats;
        _playerStats.movementValue = _playerStats.maxMovement;
	}

	protected override void Start()
	{

		base.Start();
	}

	//private void OnDisable()
	//{
 //       GameManager.instanceGM.playerStats = _playerStats;
	//}

    private void CheckIfGameOver()
    {
        if(_playerStats.hitPoints <= 0)
        {
            Debug.Log("Game Over");
            
			GameManager.instanceGM.gameIsOver = true;
			gameObject.SetActive(false);
			GameManager.instanceGM.gameIsWon = false;
			GameManager.instanceGM.GameOver();
			enabled = false;
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

	public Damage TakeDamage(Damage damage)
    {
		int finalDamage = damage.rawDamage - _playerStats.GetBlockValue();
		Debug.Log("Damage : " + finalDamage + "dura : "+ damage.duraDamage);
		_playerStats.DealBlockDuraDamage(damage.duraDamage);
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

			Equipment equipment = other.GetComponent<PickUp>()._equipment;
			if (equipment.equipmentType == Equipment.Type.Weapon)
			{
				PlayerStats.Slot slot = new PlayerStats.Slot(equipment.icon,equipment.attackValue,equipment.blockValue,equipment.durabilty); 
				_playerStats.weaponSlot = slot;
				_weaponSpriteRenderer.sprite = equipment.icon;
			}
            
			else if (equipment.equipmentType == Equipment.Type.Armor)
			{
				PlayerStats.Slot slot = new PlayerStats.Slot(equipment.icon,equipment.attackValue,equipment.blockValue,equipment.durabilty); 
				_playerStats.armorSlot = slot;
			}
				
			else if (equipment.equipmentType == Equipment.Type.Other){
				PlayerStats.Slot slot = new PlayerStats.Slot(equipment.icon, equipment.attackValue, equipment.blockValue, equipment.durabilty);
				_playerStats.otherSlot = slot;
			}
			else if (equipment.equipmentType == Equipment.Type.Consumable){
				Debug.Log(equipment.heal);
				_playerStats.defaultAttack += equipment.attackValue;
				_playerStats.defaultBlock += equipment.blockValue;
				_playerStats.hitPoints += equipment.heal;
			}
			GameObject popInst = Instantiate(popUpGO, transform.position, Quaternion.identity, transform);
			popInst.GetComponent<Pop>().SetText(equipment.desc);
            Destroy(popInst, 1f);


			other.gameObject.SetActive(false);
            
        }
       
	}

	protected override void OnCantMove<T>(T component)
	{
		IDamageable hitEnemy = component as IDamageable;
        
		//Debug.Log(hitEnemy.gameObject.name);
        // ensure combat

		Damage counter = hitEnemy.TakeDamage(new Damage(_playerStats.GetAttackValue(),0));
		if(_playerStats.weaponSlot != null)
		    _playerStats.weaponSlot.durability -= counter.duraDamage;


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
		if(_playerStats != null){
			if(_playerStats.weaponSlot != null){
				_weaponSpriteRenderer.sprite = _playerStats.weaponSlot.icon;
			}
			if (_playerStats.weaponSlot != null && _playerStats.weaponSlot.durability <= 0)
            {
                _weaponSpriteRenderer.sprite = defaultWeaponSprite;
                _playerStats.weaponSlot = null;
            }
			if (_playerStats.armorSlot != null && _playerStats.armorSlot.durability <= 0)
			{
				_playerStats.armorSlot = null;
			}
			if (_playerStats.otherSlot != null && _playerStats.otherSlot.durability <= 0)
            {
				_playerStats.otherSlot = null;
			}
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
