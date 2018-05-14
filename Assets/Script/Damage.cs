using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Damage {


    public int rawDamage;
	public int duraDamage;

    public Damage(int raw,int dura){
        rawDamage = raw;
		duraDamage = dura;
    }
}
