using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pop : MonoBehaviour {

	public Text popText;

	public void SetText(string text){
		popText.text = text;
	}
}
