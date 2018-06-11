using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamageable
{

	public int attack;
	public int hp;
	public int armor;

	[System.Serializable]
	public class Loot
	{
		public GameObject pick;
		public int dropChancePercent; // intpercent
	}

	public Animator graphicAnimator;
	public GameObject slashSprite;
	public Loot[] loots;
    
	int PickRandomItem(){
		int range = 0;
		for (int i = 0; i < loots.Length; i++){
			range += loots[i].dropChancePercent;
		}
		int rand = Random.Range(0, range);
		int top = 0;

		for (int i = 0; i < loots.Length;i++){
			top += loots[i].dropChancePercent;
			if(rand < top){
				return i;
			}
		}
        return -1;
	}
    
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
			GameObject popInst = Instantiate(loots[PickRandomItem()].pick, transform.position, Quaternion.identity) as GameObject;
			gameObject.SetActive(false);
		}

		return new Damage(attack, armor);
	}
}
