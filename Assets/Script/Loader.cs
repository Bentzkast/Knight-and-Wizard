using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameManager game_manager;

	private void Awake()
	{
        if(GameManager.instanceGM ==null){
            Instantiate(game_manager);
        }
	}
}
