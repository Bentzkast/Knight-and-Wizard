using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving_Object {

    public int hp = 3;
    public GameObject enemy_child_object;
    public GameObject slash_sprite;

    private SpriteRenderer spriteRenderer;
    private Transform target;
    private Animator animator;

	private void Awake()
	{
        spriteRenderer = enemy_child_object.GetComponent<SpriteRenderer>();
        animator = enemy_child_object.GetComponent<Animator>();
	}
    protected override void Start()
    {
        GameManager.gm_instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
        base.AttemptMove<T>(xDir, yDir);
	}

	public void DamageEnemy(Damage damage){
        GameObject slash_instance = Instantiate(slash_sprite, gameObject.transform.position, Quaternion.identity);
        Destroy(slash_instance, .1f);
        hp -= damage.raw_damage;
        animator.SetTrigger("Enemy_hit");

        if (hp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.gm_instance.RemoveEnemyFromList(this);
        }
            
    }

    public void MoveEnemy(){
        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) > float.Epsilon)
            yDir = target.position.x > transform.position.x ? 1 : -1;
        else
            xDir = target.position.y > transform.position.y ? 1 : -1;
        AttemptMove<Player>(xDir, yDir);
    }

	protected override void OnCantMove<T>(T component)
	{
        Player hit_player = component as Player;
        hit_player.TakeDamage(new Damage(1));
        animator.SetTrigger("Enemy_attack");
	}
}
