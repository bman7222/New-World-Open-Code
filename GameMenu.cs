using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameMenu : MonoBehaviour {

    public GameObject Menu;

    private CharStats [] playerStats ;

    public Text[] nameText, HPText, MPText, LVLText, EXPText;

    public Slider [] EXPSlider;

    public Image [] charImage;

    public GameObject[] charStatHolder;

    public GameObject[] windows;

    public GameObject[] statusButtons;

    public Text statusName, statusHP, statusMP, statusSTR, statusDEF, statusWPN, statusWPNPOW, statusARM, statusARMDEF, statusEXP;

    public Image statusImage;

    public ItemButton[] itemButtons;

    public string selectedItem;

    public Item activeItem;

    public Text itemName, itemDescript, useButtonText;

    public static GameMenu Instance;

    public GameObject itemCharacterChoiceMenu;

    public Text[] characterChoiceNames;

    public Text goldText; 

    // Use this for initialization
    void Start () {

        Instance = this; 

	}
	
	// Update is called once per frame
	void Update () {

        //ifbutton is clicked open or close menu 
        if (Input.GetButtonDown("Fire2"))
        {
            if (Menu.activeInHierarchy)
            {
                //menu is closed
                // Menu.SetActive(false);
                //game manager recgonzies menu is closed so player can move again
                //  GameManager.Instance.gameMenuOpen = false; 

                closeMenu(); 
            }

            //if menu is open and the button is pressed then close menu 
           if(!Menu.activeInHierarchy && PlayerController2.Instance.canmove == true)
            {
                Menu.SetActive(true);

                updateMainStats(); 

                //game manager recgonzies menu is open so player can't move 
                GameManager.Instance.gameMenuOpen = true;

                //activate menu sound
                AudioManager.Instance.playSFX(5); 
            }
        }
		
	}

    public void updateMainStats()
    {
        //player stats are the stats
        playerStats = GameManager.Instance.playerStats; 

        //check stats
        for(int i=0; i<playerStats.Length; i++)
        {
            //check if character slot is activated, if not then leave blank
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                //turn on char slot
                charStatHolder[i].SetActive(true);

                //set the name to the name in the array 
                nameText[i].text = playerStats[i].charName;
                
                //set HP to the current HP/Max HP
                HPText[i].text= "HP: "+ playerStats[i].currentHP+"/"+playerStats[i].maxHP;

                //set MP to the current MP/Max MP
                MPText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;

                //set LVL to current LVL
                LVLText[i].text = "LVL " + playerStats[i].playerLevel;

                //Set exp to current EXP (current)/(needed)
                EXPText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].EXPtoNextLevel[playerStats[i].playerLevel];

                //set max exp of slider to max exp of array 
                EXPSlider[i].maxValue = playerStats[i].EXPtoNextLevel[playerStats[i].playerLevel];

                //set current exp of sldier to current exp of array
                EXPSlider[i].value = playerStats[i].currentEXP;

                //set image to image in array
                charImage[i].sprite = playerStats[i].charImage; 
            }
            else
            {
                //turn off char slot
                charStatHolder[i].SetActive(false);
            }


        } // end of for loop

        goldText.text = GameManager.Instance.currentGold.ToString() + " G"; 

    }

    //open button window
    public void toggleWindow(int windowNumber)  
    {

        updateMainStats();
        //check which button pressed
        for (int i=0; i<windows.Length; i++)
        {
            //open button based on its assigned number
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy); 
            }

            //close all other menus
            else
            {
                windows[i].SetActive(false);
            }

        }

        //st the item character select menu to false 
        itemCharacterChoiceMenu.SetActive(false);

    }

    //close the menu
    public void closeMenu()
    {

        //set all other menu windows to close
        for(int i=0; i<windows.Length; i++)
        {
            windows[i].SetActive(false); 
        }
        //close the menu 
        Menu.SetActive(false);

        //make sure that the game manager recgonzises menu is now clsoed
        GameManager.Instance.gameMenuOpen = false;

        //set the character chocie menu for item select to false
        itemCharacterChoiceMenu.SetActive(false); 
        
    }

    //for the stats menu buttons
    public void openStats()
    {
        //set player 0 as default
        statusCharacter(0);

        //update info shown 
        updateMainStats(); 

        //for every button set it active if the party slot is active and set the anem to the characters name
        for(int i=0; i<statusButtons.Length; i++)
        {

            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName; 
        }

    }

    //setscharacter stats
    public void statusCharacter(int selected)
    {
        //set char name
        statusName.text = playerStats[selected].charName;

        //set char HP 
        statusHP.text = ""+ playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;

        //set char MP
        statusMP.text = ""+ playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;

        //set char STR
        statusSTR.text = playerStats[selected].str.ToString();

        //set char DEF
        statusDEF.text = playerStats[selected].def.ToString();

        //if there is a weapon equipped then set the name to the weapon name otherwise display the default (none)
        if(playerStats[selected].equippedWeapon != "")
        {
            statusWPN.text = playerStats[selected].equippedWeapon;
        }

        //set weapon power
        statusWPNPOW.text = playerStats[selected].wpn.ToString();

        //if there is a armor equipped then set the name to the armor name otherwise display the default (none)
        if (playerStats[selected].equippedArmor != "")
        {
            statusARM.text = playerStats[selected].equippedArmor;
        }

        //set armor defense stat
        statusARMDEF.text = playerStats[selected].armor.ToString();

        //set exp to enxt lvl 
        statusEXP.text = (playerStats[selected].EXPtoNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();

        //set char sprite
        statusImage.sprite = playerStats[selected].charImage;



    }

    public void showItems()

    {
        GameManager.Instance.sortItems(); 

        for(int i=0; i<itemButtons.Length; i++)
        {
            //set button alue according to the array
            itemButtons[i].buttonValue = i;

            //if  item held isnt blank
            if(GameManager.Instance.itemsHeld[i] != "")
            {

                //set button active
                itemButtons[i].buttonImage.gameObject.SetActive(true);

                //call function to find item and go to the item script and grab the sprite and make the button have that sprite
                itemButtons[i].buttonImage.sprite = GameManager.Instance.getItemDetails(GameManager.Instance.itemsHeld[i]).itemSprite;

                //change amoutn to proper amount
                itemButtons[i].amount.text = GameManager.Instance.numberOfHeld[i].ToString();
            }

            else
            {
                //turn off button and get rid of text
              itemButtons[i].buttonImage.gameObject.SetActive(false);
             itemButtons[i].amount.text = ""; 
            }

        }

    }

    public void selectItem(Item newItem)
    {
        activeItem = newItem;

        //if itemset use button text to use 
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        
        //if armor or wepaon set use button text to equip
        if (activeItem.isArmor || activeItem.isWeapon){ 
        
            useButtonText.text = "Equip";
        }

        //set item name to name of activated item
        itemName.text = "Item Name: "+activeItem.itemName;

        //set item description to description of activated item
        itemDescript.text = "Item Desc: "+activeItem.description; 
    }

    //for when the discard button is pressed
    public void discardItem()
    {
        //if the item ins't null use the remove item function from the game manager
        if (activeItem != null)
        {
            GameManager.Instance.removeItem(activeItem.itemName); 
        }

    }

    //opens the characer selecter when pressing the use item button 
    public void openItemCharacterChoice()
    {

        itemCharacterChoiceMenu.SetActive(true);

        //get names of all party members and display them accordingly
        for(int i=0; i<characterChoiceNames.Length; i++)
        {
            characterChoiceNames[i].text = GameManager.Instance.playerStats[i].charName;

            characterChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.Instance.playerStats[i].gameObject.activeInHierarchy); 
        }

    }

    //close the character select from using an item
    public void closeItemCharacterChoice()
    {

        itemCharacterChoiceMenu.SetActive(false);

    }

    //use the item on the selected character and close the character choice menu
    public void useItem(int selectChar)
    {
        activeItem.Use(selectChar);

        closeItemCharacterChoice(); 
    }

    //saves game data and quest data
    public void saveGame()
    {
        GameManager.Instance.saveData();
        QuestManager.Instance.saveQuestData(); 
    }

    public void playButtonSound()
    {
        AudioManager.Instance.playSFX(4); 
    }

    public void quitGame()
    {
        Application.Quit(); 
    }

}
