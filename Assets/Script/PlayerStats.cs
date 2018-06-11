using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerStats {

	public class Slot {
		public Sprite icon;
		public int attackValue;
		public int blockValue;
		public int durability;

		public Slot(Sprite icon, int a, int b, int d){
			this.icon = icon;
			attackValue = a;
			blockValue = b;
			durability = d;
		}
	}
  

	public int maxMovement = 2;
	public int maxSlot = 2;
    public int movementValue = 2;
    public int hitPoints = 2;
	public int defaultAttack = 1;
	public int defaultBlock = 0;
	public Slot weaponSlot = null;
	public Slot armorSlot = null;
	public Slot otherSlot = null;


	public int GetAttackValue(){
		int attack = defaultAttack;
		if (weaponSlot != null) attack += weaponSlot.attackValue;
		return attack;
	}

	public int GetBlockValue(){
		int block = defaultBlock;
		if (weaponSlot != null) block += weaponSlot.blockValue;
		if (armorSlot != null) block += armorSlot.blockValue;
		if (otherSlot != null) block += otherSlot.blockValue;
        
		return block;
	}


	public void DealBlockDuraDamage(int duradamage){
		if (armorSlot != null)
        {
			if (armorSlot.blockValue > 0)
            {
				if (duradamage > armorSlot.durability)
                {
					duradamage -= armorSlot.durability;
					armorSlot.durability = 0;
                }
                else
                {
					armorSlot.durability -= duradamage;
                }
            }
        }
		if (otherSlot != null)
        {
			if (otherSlot.blockValue > 0)
            {
				if (duradamage > otherSlot.durability)
                {
					duradamage -= otherSlot.durability;
					otherSlot.durability = 0;
                }
                else
                {
					otherSlot.durability -= duradamage;
                }
            }
        }

	}
}
