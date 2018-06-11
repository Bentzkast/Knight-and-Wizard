using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

	public SpriteRenderer spriteRenderer;
	public Equipment _equipment;

	public void Start()
	{
		spriteRenderer.sprite = _equipment.icon;
	}
}
