using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleStarter : MonoBehaviour {

    public battleType[] potentialBattles;

    private bool inArea;

    public bool activateOnEnter, activateOnStay, activateOnExit;

    public float timeBetweenBattles;

    private float betweenBattleCounter;

    public bool deactivateAfterStarting;

	// Use this for initialization
	void Start () {

        //sets the battle counter to a random number
        betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f); 

	}
	
	// Update is called once per frame
	void Update () {
		
        //if in the area and can move
        if(inArea && PlayerController2.Instance.canmove)
        {
            //if a move is inputted
            if(Input.GetAxisRaw("Horizontal") !=0 || Input.GetAxisRaw("Vertical") !=0 )
            {
                //battle counter goes down
                betweenBattleCounter -= Time.deltaTime; 

            }

            //if battle counter is <= 0
            if (betweenBattleCounter <= 0)
            {
                //reset battle counter
                betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);

                //start a battle
                StartCoroutine(StartBattleCo()); 

            }

        }

	}

    //when entering
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if a player enters
        if(other.tag== "Player")
        {

            //if activate on neter is on, then start a battle right away
            if (activateOnEnter)
            {

                StartCoroutine(StartBattleCo()); 

            }

            //otherwise classify it as in area
            else
            {

                inArea = true;


            }
        }



    }

    //when someone exits the area
    private void OnTriggerExit2D(Collider2D other)
    {
        //if the person is a player
        if (other.tag == "Player")
        {
            //if activate on exit is on then start a battle
            if (activateOnExit)
            {

                StartCoroutine(StartBattleCo());

            }

            //otherwise make in area false
            else
            {

                inArea = false;


            }
        }



    }

    //starts a battle
    public IEnumerator StartBattleCo()
    {

        //ameks the screen fade black
        UIFade.Instance.fadetoBlack();

        //activates battle
        GameManager.Instance.battleActive = true;

        //sets the selected battle to any random possibility i potential battles
        int selectedBattle = Random.Range(0, potentialBattles.Length);

        //sets reward item sand exp to selected battle reward items and exp
        BattleManager.Instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.Instance.rewardEXP = potentialBattles[selectedBattle].rewardEXP;

        //makes the screen wait
        yield return new WaitForSeconds(0.5f);

        //selects enemies from the potential batttle
        BattleManager.Instance.battleStart(potentialBattles[selectedBattle].enemies);

        //makes screen fade from black
        UIFade.Instance.fadefromBlack(); 

        //if deactivate after starting is true then deactivate the zone
        if (deactivateAfterStarting)
        {

            gameObject.SetActive(false);

        }
    }


} //end of script
