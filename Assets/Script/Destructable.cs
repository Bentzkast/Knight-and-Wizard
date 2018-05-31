using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamageable {

	public int attack;
	public int hp; 
	public int armor;

	public Animator graphicAnimator;
	public GameObject slashSprite;
    
	public Damage TakeDamage(Damage damage)
	{
		hp -= damage.rawDamage;
		Debug.Log("take damage");
		if(damage.rawDamage > 0)
		{
			graphicAnimator.SetTrigger("GetHit");
			GameObject slash_instance = Instantiate(slashSprite, gameObject.transform.position, Quaternion.identity);
            Destroy(slash_instance, .1f);
		}
        if(hp <= 0)
		{
			gameObject.SetActive(false);
		}

		return new Damage(attack, armor);
	}
}
