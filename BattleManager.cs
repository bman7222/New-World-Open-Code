﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    public static BattleManager Instance;

    public bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPos, enemyPos;

    public BattleChar[] playerPrefabs, enemyPrefabs;

    public GameObject[] playerBox;

    public int musicToPlay;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;

    public bool turnWaiting;

    public GameObject UIButtonsHolder;

    public AttackMove[] movesList;

    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;

    public Text[] playerName, playerHP, playerMP;

    public GameObject targetMenu;

    public BattleTargetButton[] targetButtons;

    public Text descriptionText;

    public GameObject enemyNameMenu, descriptionBox, itemMenu;

    public GameObject magicMenu, ppBox;

    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 80;


    // Use this for initialization
    void Start() {

        Instance = this;


        /*maybe get rid of this line???*/
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.B))
        {
            battleStart(new string[] { "Belcher", "Belcher" });
        }

        //if the battle is active 
        if (battleActive)
        {

            //if the player is waiting for a turn 
            if (turnWaiting)
            {
                //set UI buttons on
                if (activeBattlers[currentTurn].isPlayer)
                {
                    UIButtonsHolder.SetActive(true);
                }

                //otherwise have enemy attack and set buttons to false
                else
                {
                    UIButtonsHolder.SetActive(false);

                    //starts the enemies turn, makes the enemy attack and then wait then go to your turn
                    StartCoroutine(enemyMoveCo());
                }

            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                nextTurn();
            }

        } // end of battle active if 
    }


    //when the battle starts do this script
    public void battleStart(string[] enemiesToSpawn)
    {

        //sets all player boxes to false
        for (int i = 0; i < playerBox.Length; i++)
        {

            playerBox[i].SetActive(false);

        }


        //if the battle active variable isn't set to true, then set it to true, also set the gamemanager's battle active varibale to true so that the player cant move, also turns on the battle background and moves it to the correct position
        if (!battleActive)
        {
            //set battle active to true 
            battleActive = true;

            //set the gamemanager batte active to true so player cant move 
            GameManager.Instance.battleActive = true;

            //changes the position of the battle background
            transform.position = new Vector3(Camera.main.transform.position.x + 1.2f, Camera.main.transform.position.y + -3.25f, transform.position.z);

            //sets the battle background on
            battleScene.SetActive(true);


            //sets battle bgm
            AudioManager.Instance.playBGM(musicToPlay);

            //cycle through the player positions
            for (int i = 0; i < playerPos.Length; i++)
            {
                // if the player stats at i are active 
                if (GameManager.Instance.playerStats[i].gameObject.activeInHierarchy)
                {

                    //then cycle throuhg the player prefabs
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {

                        //if the player's prefab is found equal to the player stats character name
                        if (playerPrefabs[j].charName == GameManager.Instance.playerStats[i].charName)
                        {

                            //then the player with the matching char name is instantiated  and the newly spawned player's x,y,z, are linked to the player position  
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPos[i].position, playerPos[i].rotation);

                            newPlayer.transform.parent = playerPos[i];

                            //add a new player to the active battler list

                            activeBattlers.Add(newPlayer);

                            //creates "thePlayer" which holds the stats for the player[i] stats

                            CharStats thePlayer = GameManager.Instance.playerStats[i];

                            //gives the active battler player the stats of the player
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].str = thePlayer.str;
                            activeBattlers[i].def = thePlayer.def;
                            activeBattlers[i].wpnPower = thePlayer.wpn;
                            activeBattlers[i].armPower = thePlayer.armor;

                            //sets the player box in the array at the same position as the player position to true
                            playerBox[i].SetActive(true);


                        } // end of second if statement


                    } // end of j for loop
                } // end of first if statement

            } // end of i for loop

            //cycle throuhg the list of spawned enemies
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                //if the enemies name isn't null
                if (enemiesToSpawn[i] != "")
                {
                    //cycle through the enemy prefabs 
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {

                        //if the enemy name matches a name in the enemy prefabs
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            //create a new enemy based on the prefab (ex if prefab is spider then amke a spider)
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPos[i].position, enemyPos[i].rotation);

                            //set new enemy xyz to enemy xyz 
                            newEnemy.transform.parent = enemyPos[i];

                            //add the enemy to the active battlers  
                            activeBattlers.Add(newEnemy);

                        } //  end of if statement
                    } // end of j for loop

                }    // end of if statement 

            } // end of i for loop

            //sets turn waiting to true  at battle start
            turnWaiting = true;

            //sets current turn to 0
            currentTurn = 0;

            //updates the UI stats to match
            updateUIStats();

        }

    }

    //sets to next turn if the turn is over the activ battlers then 
    public void nextTurn()
    {
        //adds 1 to the turn count 
        currentTurn++;

        // if turn is greater than the number of batttlers than go back to 0
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        //waiting for turn is true
        turnWaiting = true;

        //update the battle
        updateBattle();

        //updates the UI stats to match
        updateUIStats();
    }

    //Updates the battle, checks if enemy or player is dead
    public void updateBattle()
    {
        //these are set true so that if a player does have haelth they are set to false
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        //cycle through all active battlers, to see if any are dead and check if all players dead or all enemies are dead
        for (int i = 0; i < activeBattlers.Count; i++)
        {

            //if hp is less than 0 then display 0
            if (activeBattlers[i].currentHP < 0)
            {

                activeBattlers[i].currentHP = 0;

            } //end of if hp <0 

            //if it is 0 then make battler dead
            if (activeBattlers[i].currentHP == 0)
            {
                //sets player's sprite to their assigned dead sprite
                if (activeBattlers[i].isPlayer)
                {
                    //changes it to the dead sprite (grays out player)
                    activeBattlers[i].shouldFadePlayer(); 

                }

                //other wise if they are an enemy make them disappear when dead
                else
                {
                    //makes enemy disappear
                    activeBattlers[i].shouldFadeEnemy(); 

                }
           

            } // end of if hp  = 0

            //otherwise 
            else
            {
                //if the active battler is a player than set all players dead to false
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;

                    activeBattlers[i].shouldRevive(); 

                }

                //otheriwse if its an enemy set all enemies dead to false
                else
                {
                    allEnemiesDead = false;
                }

            } // end of else

        } // end of for loop

        //if all players or enemies 
        if (allEnemiesDead || allPlayersDead)
        {
            //if all enemies are deadyou win
            if (allEnemiesDead)
            {


                //end battle victory

                StartCoroutine(EndBattleCo()); 
            }

            //otherwise all players are dead and you lose
            else
            {
                //end battle defeat
            }
        
        } // end of all enemies or all players dead if

        //else if all the players or all the enemie arent dead, then go through the active battlers and see if anyone is dead
        else
        {
            //while the active abttler's hp = 0
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                //go to next turn 
                currentTurn++;

                //if the count is higher then or equal to active battlers, reset to 0
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }

            }

        }

    } // end of update battle

    //decides the enemies attack turn
    public void enemyAttack()
    {
        //creates a list of players
        List<int> players = new List<int>();

        //cycles through all active battlers 
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            //if the active battler at position i is a player who is not KOd then add that player to the players list 
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {

                //adds player's number to the list
                players.Add(i);

            }

        }

        //makes selected target equal to any player in the entire players list
        int selectedTarget = players[Random.Range(0, players.Count)];

        // the attack selected is a random number from 0 to the length of that battle characters moves availble list 
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailible.Length);

        int movePower = 0;

        //for every move in the move lis check 
        for (int i = 0; i < movesList.Length; i++)
        {
            //this portion goes through the entire moves list to find the mathcing selected attack 
            //if the name of the moves list at positon i is equal to the current battlers moves availible (ie they have the same name) then do the attack 
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailible[selectAttack])
            {

                //create the move's effect and set movePower to the selected attack's move power
                Instantiate(movesList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }

        }

        // BETA VERSION OF SHOWING WHO IS ATTACKING Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation); 

        dealDamage(selectedTarget, movePower);

    }

    public void dealDamage(int target, int movePower)
    {

        //attackers strenght + weapon power = attack power
        float atkPwr = activeBattlers[currentTurn].str + activeBattlers[currentTurn].wpnPower;

        //the targets defense + armor power = defense power
        float defPwr = activeBattlers[target].def + activeBattlers[target].armPower;

        // attack power divided by defense power and then multiplied by move power equals damage 
        float damageCalc = atkPwr / defPwr * movePower;

        //round the damage calculation to a whole number 
        int damageToGive = Mathf.RoundToInt(damageCalc);

        //puts actions in log
        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageToGive + " damage to " + activeBattlers[target].charName);

        //makes the targets hp go down by the damage given
        activeBattlers[target].currentHP -= damageToGive;

        //creates a damage number above the target, sets the damage of the damage number to the damage to give variable
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).setDamage(damageToGive);

        //updates the UI stats to match
        updateUIStats();
    }

    //updates the stats of the players on the menu
    public void updateUIStats()
    {

        //for every name in player names 
        for (int i = 0; i < playerName.Length; i++)
        {
            //if the number of active battlers is greater than i 
            if (activeBattlers.Count > i) {
                // if the battler at i is a player
                if (activeBattlers[i].isPlayer)
                {
                    // the player data is equal to all of the stats of the active battler
                    BattleChar playerData = activeBattlers[i];

                    //set the player name to true 
                    playerName[i].gameObject.SetActive(true);

                    //set player name to the player data name
                    playerName[i].text = playerData.charName;

                    //set hp to current hp
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "";

                    //set mp to current mp
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "";

                }

                else
                {
                    //if it is an enemy set playername to false
                    playerName[i].gameObject.SetActive(false);

                }

            }

            else
            {

                //if i goes above active battlers, set playername to false 
                playerName[i].gameObject.SetActive(false);

            }

        }

    }


    //makes it so that the enemy waits between attacks and going to your turn 
    public IEnumerator enemyMoveCo()
    {
        //waiting for a turn is false
        turnWaiting = false;

        //waits for 1 second
        yield return new WaitForSeconds(1f);

        //activates enemy attack turn
        enemyAttack();

        //waits for 1 second
        yield return new WaitForSeconds(1f);

        //set to next turn
        nextTurn();

    }

    //makes player attack a targeted enemy 
    public void playerAttack(string moveName, int selectedTarget)
    {



        int movePower = 0;

        //for every move in the move list check 
        for (int i = 0; i < movesList.Length; i++)
        {
            //this portion goes through the entire moves list to find the matching selected attack 
            //if the name of the moves list at positon i is equal to the current battlers moves availible (ie they have the same name) then do the attack 
            if (movesList[i].moveName == moveName)
            {

                //create the move's effect and set movePower to the selected attack's  move power
                Instantiate(movesList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }

        }

        //goes to deal damage method, attacks a selected target with damage equal to move power
        dealDamage(selectedTarget, movePower);

        //turns off UI holder so no one can double click a button fast
        UIButtonsHolder.SetActive(false);

        //turns off the target menu
        targetMenu.SetActive(false);

        //turns off the description box
        descriptionBox.SetActive(false);

        //turns off the enemy name menu 
        enemyNameMenu.SetActive(false);

        //updates MP display 
        BattleManager.Instance.updateUIStats();

        //goes to next turn after attack
        nextTurn();

    }

    public void openTargetMenu(string moveName)
    {

        //turns the target buttons on so that an enemy can be selected to attack
        targetMenu.SetActive(true);

        //turns on the description box
        descriptionBox.SetActive(true);

        //turns on the enemy name menu 
        enemyNameMenu.SetActive(true);

        //sets the text in the description box to 'on who?' 
        descriptionText.text = "On who?";


        //list that will be used to store values of enemies
        List<int> Enemies = new List<int>();

        //for all active battlers check if they are players, if they aren't then ad them to the enemies list 
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            //if the active battler isn't a player then ad them to the enemies list
            if (!activeBattlers[i].isPlayer)
            {
                //add the active battler at i to the enemies list 
                Enemies.Add(i);

            }

        }

        //for every target button check and see if it is greater or less than the amount of enemies. If greater than deactivate it, if less activate it
        for (int i = 0; i < targetButtons.Length; i++)
        {
            //if the enemy count is greater than the current number of buttons and enemy has HP left
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP>0)
            {
                //set button to active
                targetButtons[i].gameObject.SetActive(true);

                //sets the move being used to the move name
                targetButtons[i].moveName = moveName;

                //sets the target to the enemy in the i position of the enemies list 
                targetButtons[i].activeBattlerTarget = Enemies[i];

                //sets the name text to the enemies name
                targetButtons[i].highlightName = activeBattlers[Enemies[i]].charName;

            }

            //if the number of buttons is greater than the number of enemies
            else
            {
                //set button to inactive
                targetButtons[i].gameObject.SetActive(false);

            }

        }



    }


    //opens the magic menu and sets all buttons to the correct name and amount
    public void openMagicMenu()
    {

        //magic menu active
        magicMenu.SetActive(true);

        //open pp box
        ppBox.SetActive(true);

        //for every magic buton, see if it is larger than the amoutnt of moves availible the character has to include the max number of moves
        for (int i = 0; i < magicButtons.Length; i++)
        {

            //if the moves availible si greater than i, than set that button to true, change the spell name to the correct spell name, and change the name text to the correct name text
            if (activeBattlers[currentTurn].movesAvailible.Length > i)
            {
                //activvates button 
                magicButtons[i].gameObject.SetActive(true);

                //chnages the spell used
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailible[i];

                //changes the name of the spell
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                //for every move in the battlers move list seeif there is a move with a name matching the spell name, if there is then set the move cost and the cost text to the matching move
                for (int j = 0; j < movesList.Length; j++)
                {
                    //if the move nae is the same as the spell name
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        //sets the spell cost to the move's cost
                        magicButtons[i].spellCost = movesList[j].moveCost;

                        //set the text of the magic button's cost
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();

                    }

                }

            }

            //if i is greater than moves availible then deactivate the button
            else
            {
                //deactivate the button
                magicButtons[i].gameObject.SetActive(false);

            }

        }

    }

    //flees the battle
    public void Flee()
    {
        //picks a random number from 1-100
        int fleeSuccess = Random.Range(0, 100);

        //if  the random number (flee success) is less than chance to flee, then end the battle otherwise create a notifcation saying the flee failed
        if (fleeSuccess < chanceToFlee)
        {

            //end battle 
            StartCoroutine(EndBattleCo()); 

        }

        //make a notifcation saying flee failed
        else
        {

            //go to next turn
            nextTurn();

            //set the description boxtext to fleefailed
            descriptionText.text = "You failed to flee.";

            //activate the description box
            battleNotice.Activate();

        }

    }

    //opens the item menu
    public void openItemMenu()
    {
    

    }

    public IEnumerator EndBattleCo()
    {

        //make sure UI button holder is off 
        UIButtonsHolder.SetActive(false);

        //turn off target menu
        targetMenu.SetActive(false);

        //turn off magic menu
        magicMenu.SetActive(false);

        //make battle wait for things to disappear
        yield return new WaitForSeconds(0.75f);

        //fade to black
        UIFade.Instance.fadetoBlack();

        //make screen wait again 
        yield return new WaitForSeconds(1.5f);

        //for every active battler, check to see if they are a player and finding their matching stats
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            //if they are a player then find their matchign name in the stats list 
            if (activeBattlers[i].isPlayer)
            {
               // for every player in the player stats, if they have amatching name
                for (int j=0; j< GameManager.Instance.playerStats.Length; j++)
                {
                    //if the player name matches one foudn in player stats, set the current HP and Mp to match what it should be after the battle
                    if(activeBattlers[i].charName== GameManager.Instance.playerStats[j].charName)
                    {
                        //set the players current HP and MP to what it was when they were battling
                        GameManager.Instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.Instance.playerStats[j].currentMP = activeBattlers[i].currentMP;

                    }

                }

            }

            //destroy the player
            Destroy(activeBattlers[i].gameObject); 

        }

        //go back into the scene 
        UIFade.Instance.fadefromBlack();

        //set battle scene to off
        battleScene.SetActive(false);

        // reset the active battlers list
        activeBattlers.Clear();

        //set the new current turn to 0 
        currentTurn = 0;

        //set game manager instance  battle active to false
        GameManager.Instance.battleActive = false;

        // set battle active to false
        battleActive = false;

        //play the set BGM again 
        AudioManager.Instance.playBGM(FindObjectOfType<CameraController>().musicToPlay); 

    }


} //end of script 
