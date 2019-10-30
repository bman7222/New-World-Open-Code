using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {

    public GameObject UIScreen;
    public GameObject Player;
 
   

	// Use this for initialization
	void Start () {
		
        if(UIFade.Instance == null)
        {
        UIFade.Instance =Instantiate(UIScreen).GetComponent<UIFade>();
        }

     

        if (PlayerController2.Instance == null)
        {
            //when essential loader enters screen 
          PlayerController2 clone =  Instantiate(Player).GetComponent<PlayerController2>();
            PlayerController2.Instance = clone; 
        }

       

     
    }
	
	// Update is called once per frame
	void Update () {
		
	}

   



}


