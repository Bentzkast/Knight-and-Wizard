using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerStats {

	public int maxMovement = 2;
	public int maxSlot = 2;
    public int movementValue = 2;
    public int hitPoints = 2;
	public int defaultAttack = 1;
	public Equipment weaponSlot = null;
	public Equipment armorSlot = null;
	public Equipment otherSlot = null;


	public int GetAttackValue(){
		int attack = defaultAttack;
		if (weaponSlot != null) attack += weaponSlot.attackValue;
		return attack;
	}

	public int GetBlockValue(){
		int block = 0;
		if (weaponSlot != null) block += weaponSlot.blockValue;
		if (armorSlot != null) block += armorSlot.blockValue;
		if (otherSlot != null) block += otherSlot.blockValue;
		//if(block > 0)
		//{
		//	DealBlockDuraDamage(1);
		//}

		return block;
	}

	public void DealBlockDuraDamage(int duradamage){
		if(weaponSlot != null){
			if(weaponSlot.blockValue > 0){
				if (duradamage > weaponSlot.durabilty)
				{
					duradamage -= weaponSlot.durabilty;
					weaponSlot.durabilty = 0;
				}
				else
				{
					weaponSlot.durabilty -= duradamage;
				}
			}
		}
		if (armorSlot != null)
        {
			if (armorSlot.blockValue > 0)
            {
				if (duradamage > armorSlot.durabilty)
                {
					duradamage -= armorSlot.durabilty;
					armorSlot.durabilty = 0;
                }
                else
                {
					armorSlot.durabilty -= duradamage;
                }
            }
        }
		if (otherSlot != null)
        {
			if (otherSlot.blockValue > 0)
            {
				if (duradamage > otherSlot.durabilty)
                {
					duradamage -= otherSlot.durabilty;
					otherSlot.durabilty = 0;
                }
                else
                {
					otherSlot.durabilty -= duradamage;
                }
            }
        }

	}
}
