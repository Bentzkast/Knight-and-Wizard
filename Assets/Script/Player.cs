using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Moving_Object {

    public Animator knightAnimator;
	public GameObject weaponSlot;
	public float restartLvlDelay = 1f;

	private Animator _weaponAnimator;
	private PlayerStats _playerStats;
	private SpriteRenderer _weaponSpriteRenderer;

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
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void TakeDamage(Damage damage)
    {
        knightAnimator.SetTrigger("Knight_hit");
        _playerStats.hitPoints -= damage.rawDamage;
        CheckIfGameOver();
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if(other.tag == "exit"){
            Invoke("Restart", restartLvlDelay);
            enabled = false;
        }
        if(other.tag == "potion"){
            Debug.Log("Picked up potion");
        }

	}

	protected override void OnCantMove<T>(T component)
	{
        Enemy hitEnemy = component as Enemy;
        hitEnemy.DamageEnemy(new Damage(1));
        knightAnimator.SetTrigger("Knight_attack");
        _weaponAnimator.SetTrigger("Weapon_attack");
	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
        _playerStats.movementValue--;
        base.AttemptMove<T>(xDir, yDir);
        Debug.Log(_playerStats.movementValue.ToString());
        if (_playerStats.movementValue <= 0){
            GameManager.instanceGM.isPlayerTurn = false;
            _playerStats.movementValue = 2;
        }
	}

	private void Update()
	{
        if (!GameManager.instanceGM.isPlayerTurn) return;
        if (isMoving) return;

        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0) vertical = 0;
        if (horizontal != 0 || vertical != 0)
        {
            Debug.Log("Attempt move");
            AttemptMove<Enemy>(horizontal, vertical);
        }
	}
}
