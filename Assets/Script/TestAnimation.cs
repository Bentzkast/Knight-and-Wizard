using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {


    private Animator player_animator;
    public Animator weapon_animator;
    public GameObject slash;
    public GameObject weapon_sprite;
    public Sprite[] weapons;
	// Use this for initialization
	void Awake () {
        player_animator = GetComponent<Animator>();
        //weapon_animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)){
            player_animator.SetTrigger("Knight_Attack");
            weapon_animator.SetTrigger("Weapon_Attack");
            GameObject slash_instance = Instantiate(slash,new Vector3(1,0,0),Quaternion.identity);
            weapon_sprite.GetComponent<SpriteRenderer>().sprite = weapons[Random.Range(0, weapons.Length)];
            Destroy(slash_instance, .1f);
        }
	}
}
