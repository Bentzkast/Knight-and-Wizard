

using UnityEngine;

[System.Serializable]
public class Equipment{
	public enum Type {Weapon, Armor, Other, Consumable}

	public Sprite icon;
	public int Eid = 1;
    public int blockValue = 1;
    public int attackValue = 1;
	public int durabilty = 1;
	public int tier = 1;
	public int heal = 0;
	public Type equipmentType;
	public string desc;
}

