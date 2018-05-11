using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving_Object {

    public int hp = 3;
	public GameObject enemyChildObject;
	public GameObject slashSprite;

    //private SpriteRenderer spriteRenderer;
	private Transform _target;
	private Animator _animator;

	private void Awake()
	{
        //spriteRenderer = enemy_child_object.GetComponent<SpriteRenderer>();
        _animator = enemyChildObject.GetComponent<Animator>();
	}
    protected override void Start()
    {
        GameManager.instanceGM.AddEnemyToList(this);
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
        base.AttemptMove<T>(xDir, yDir);
	}

	public void DamageEnemy(Damage damage){
        GameObject slash_instance = Instantiate(slashSprite, gameObject.transform.position, Quaternion.identity);
        Destroy(slash_instance, .1f);
        hp -= damage.rawDamage;
        _animator.SetTrigger("Enemy_hit");

        if (hp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.instanceGM.RemoveEnemyFromList(this);
        }
            
    }

    public void MoveEnemy(){
        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs(_target.position.x - transform.position.x) > float.Epsilon)
            xDir = _target.position.x > transform.position.x ? 1 : -1;
        else
            yDir = _target.position.y > transform.position.y ? 1 : -1;
        AttemptMove<Player>(xDir, yDir);
    }

	protected override void OnCantMove<T>(T component)
	{
        Player hit_player = component as Player;
        hit_player.TakeDamage(new Damage(1));
        _animator.SetTrigger("Enemy_attack");
	}
}
