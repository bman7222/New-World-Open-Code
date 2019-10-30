using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BattleTargetButton : MonoBehaviour {

    public string moveName;

    public int activeBattlerTarget;

    public Text targetName;

    public string highlightName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

    public void Press()
    {
        BattleManager.Instance.playerAttack(moveName, activeBattlerTarget); 
    }


    public void OnPointer()
    {
        Debug.Log("pointer is in"); 
        targetName.text = highlightName; 

    }

}
