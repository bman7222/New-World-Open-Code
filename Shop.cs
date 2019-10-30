using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Shop : MonoBehaviour {

    public static Shop Instance;

    public GameObject shopMenu, buyMenu,sellMenu;

    public Text goldText;

    public bool shopIsOpen;

    public string[] itemsForSale;

    public ItemButton[] buyitemButtons;

    public ItemButton[] sellitemButtons;

    public Item selectedItem;

    public Text buyItemName, buyItemDescription, buyItemValue;

    public Text sellItemName, sellItemDescription, sellItemValue; 

	// Use this for initialization
	void Start () {

        Instance = this; 

	}
	
	// Update is called once per frame
	void Update () {

        //opens the shop menu if not opened
        if(Input.GetKeyDown(KeyCode.L) && !shopMenu.activeInHierarchy)
        {
            openShop(); 
        }
		
	}

    //method that opens the shop  and sets shopisopen to true 
    public void openShop()
    {
        shopIsOpen = true;

        GameManager.Instance.shopActive = true; 

        shopMenu.SetActive(true);

        openBuyMenu(); 
         
        goldText.text = GameManager.Instance.currentGold.ToString() + " G";
    }

    //method that closes the shop menu and sets shopisopen to false
    public void closeShop()
    {
        shopIsOpen = false;

        GameManager.Instance.shopActive = false;

        shopMenu.SetActive(false);
    }

    //method that opens buy menu when pressing buy button
    public void openBuyMenu()
    {
        buyitemButtons[0].Press(); 

        sellMenu.SetActive(false);
        buyMenu.SetActive(true);
     

        //taken from game menu script, go through the buy item buttons and give them sprites based on the items for sale in the shop keeper's script
        for (int i = 0; i < buyitemButtons.Length; i++)
        {
            //set button alue according to the array
            buyitemButtons[i].buttonValue = i;

            //if  item held isnt blank
            if (itemsForSale[i] != "")
            {

                //set button active
                buyitemButtons[i].buttonImage.gameObject.SetActive(true);

                //call function to find item and go to the item script and grab the sprite and make the button have that sprite, the item being located os deter,ed by items for sale
                buyitemButtons[i].buttonImage.sprite = GameManager.Instance.getItemDetails(itemsForSale[i]).itemSprite;

                //change amount shop keeper owns to infinite
                buyitemButtons[i].amount.text = ""; 
            }

            //if there is nothing then make it blank
            else
            {
                //turn off button and get rid of text
                buyitemButtons[i].buttonImage.gameObject.SetActive(false);
                buyitemButtons[i].amount.text = "";
            }

        } //end of for loop
    } // end of buy menu

    //method that opens sell menu when pressing sell button
    public void openSellMenu()
    {
        sellitemButtons[0].Press();

        sellMenu.SetActive(true);
        buyMenu.SetActive(false);

        showSellItems(); 

     
    } // end of sell menu

    private void showSellItems()
    {
        GameManager.Instance.sortItems();

        for (int i = 0; i < sellitemButtons.Length; i++)
        {
            //set button alue according to the array
            sellitemButtons[i].buttonValue = i;

            //if  item held isnt blank
            if (GameManager.Instance.itemsHeld[i] != "")
            {

                //set button active
                sellitemButtons[i].buttonImage.gameObject.SetActive(true);

                //call function to find item and go to the item script and grab the sprite and make the button have that sprite
                sellitemButtons[i].buttonImage.sprite = GameManager.Instance.getItemDetails(GameManager.Instance.itemsHeld[i]).itemSprite;

                //change amoutn to proper amount
                sellitemButtons[i].amount.text = GameManager.Instance.numberOfHeld[i].ToString();
            }

            //if there is nothing then make it blank
            else
            {
                //turn off button and get rid of text
                sellitemButtons[i].buttonImage.gameObject.SetActive(false);
                sellitemButtons[i].amount.text = "";
            }

        } //end of for loop
    }

    //selects an item to buy
    public void selectBuyItem(Item buyItem)
    {

        //selects the item, name, description, and value
        selectedItem = buyItem;

        buyItemName.text = selectedItem.itemName;

        buyItemDescription.text = selectedItem.description;

        buyItemValue.text = "Value: " + selectedItem.value + "G"; 


    }

    //selects an item to sell
    public void selectSellItem(Item sellItem)
    {
        //selects the item, name, description, and value

        selectedItem = sellItem;

        sellItemName.text = selectedItem.itemName;

        sellItemDescription.text = selectedItem.description;

        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value*0.5f).ToString()+"G";

    }

    //purchases item from a shop
    public void buyItem()
    {
        if (selectedItem != null)
        {
            //if gold is greater than cost then subtract valu of item from the current gold and add item to inventory
            if (GameManager.Instance.currentGold >= selectedItem.value)
            {
                GameManager.Instance.currentGold -= selectedItem.value;

                GameManager.Instance.addItem(selectedItem.itemName);
            }
        }

        goldText.text = GameManager.Instance.currentGold.ToString() + "G"; 
    }

    public void sellItem()
    {

        if (selectedItem != null)
        {
            GameManager.Instance.currentGold += Mathf.FloorToInt(selectedItem.value * 0.5f);

            GameManager.Instance.removeItem(selectedItem.itemName);

        }

        goldText.text = GameManager.Instance.currentGold.ToString() + "G";

        showSellItems(); 

    }

} //end 
