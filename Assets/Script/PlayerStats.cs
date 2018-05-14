using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerStats {

	public int maxSlot = 2;
    public int movementValue = 2;
    public int hitPoints = 2;
	public int defaultAttack = 1;
	public Equipment weaponSlot;
	public Equipment armorSlot;
	public Equipment otherSlot;
}
