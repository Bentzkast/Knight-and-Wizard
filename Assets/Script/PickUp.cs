using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {



	public Equipment GetPickUpEquipment(){
		return this.GetComponentInChildren<Equipment>();
	}
}
