using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Moving_Object {

    public Animator knight_animator;
    public GameObject weapon_slot;
    public float restart_lvl_delay = 1f;

    private Animator weapon_animator;
    private PlayerStats player_stats;
    private SpriteRenderer weapon_sprite_renderer;

	protected override void Start()
	{
        weapon_animator = weapon_slot.GetComponent<Animator>();
        weapon_sprite_renderer = weapon_slot.GetComponent<SpriteRenderer>();
        player_stats = GameManager.gm_instance.player_stats;
		base.Start();
	}

	private void OnDisable()
	{
        GameManager.gm_instance.player_stats = player_stats;
	}

    private void CheckIfGameOver()
    {
        if(player_stats.hp <= 0)
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
        knight_animator.SetTrigger("Knight_hit");
        player_stats.hp -= damage.raw_damage;
        CheckIfGameOver();
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if(other.tag == "exit"){
            Invoke("Restart", restart_lvl_delay);
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
        knight_animator.SetTrigger("Knight_attack");
        weapon_animator.SetTrigger("Weapon_attack");
	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
        player_stats.move--;
        base.AttemptMove<T>(xDir, yDir);
        Debug.Log(player_stats.move.ToString());
        if (player_stats.move <= 0){
            GameManager.gm_instance.player_turn = false;
            player_stats.move = 2;
        }
	}

	private void Update()
	{
        if (!GameManager.gm_instance.player_turn) return;
        if (moving) return;

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
