using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {

    public string transitionName;

    private float waitTime = 1f;

    private float elapsed; 

	// Use this for initialization
	void Start () {
       

        //Find the player and put them at this point
        if(transitionName == PlayerController2.Instance.areaTransitionName)
        {
            PlayerController2.Instance.transform.position = transform.position; 
        }

        UIFade.Instance.fadefromBlack();

        PlayerController2.Instance.canmove = true;

    }
	
	// Update is called once per frame
	void Update () {

       elapsed += Time.deltaTime;

        if (elapsed >= waitTime)
        {
            GameManager.Instance.fadingBetweenAreas = false;
            elapsed = 0f;
            
        }


	}
}
