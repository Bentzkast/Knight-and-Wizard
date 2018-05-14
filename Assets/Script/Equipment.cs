using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : MonoBehaviour{
	public enum Type {Weapon, Armor, Other}

	public Sprite icon;
    public int blockValue = 1;
    public int attackValue = 1;
	public int durabilty = 1;
	public int tier = 1;
	public Type equipmentType;
    
	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		this._spriteRenderer = GetComponent<SpriteRenderer>();
		this._spriteRenderer.sprite = icon;
	}
}

