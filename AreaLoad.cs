using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaLoad : MonoBehaviour
{
    //load this
    public string areaToLoad;

    //last point
    public string areaTransitionName;

    // timer for area laod
    public float waitToLoad = 1f;

    //whetehr game should load after fade
    private bool shouldLoadAfterFade;


    private PlayerController2 Player = PlayerController2.Instance; 


    // Use this for initialization
    void Start()
    {
        

        
     

    }

    // Update is called once per frame
    void Update()
    {
        //if shouldloadafterfade is activated. when ativated go from 1-0 countdown and once belwo zero, change area and make false 
        if (shouldLoadAfterFade)
        {
           GameManager.Instance.fadingBetweenAreas = true; 
            waitToLoad -= Time.deltaTime;
            

          
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
             
            }
        }



    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
          
            //when colliding with the player activate shouldLoadAfterFade and Fade to Black
            shouldLoadAfterFade = true;
            UIFade.Instance.fadetoBlack(); 

            //set last warp point 
            PlayerController2.Instance.areaTransitionName = areaTransitionName;

        }


    }

}