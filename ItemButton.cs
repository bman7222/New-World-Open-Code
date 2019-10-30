using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    public Image buttonImage;

    public Text amount;

    public int buttonValue;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Press()
    {

        //if game menu is open, then carry on 
        if (GameMenu.Instance.Menu.activeInHierarchy) { 
        //if the value of the item button isn't null then the game menu will select it ad identify it usign the manager's button value to give it a name and description
        if (GameManager.Instance.itemsHeld[buttonValue] != "")
        {
            GameMenu.Instance.selectItem(GameManager.Instance.getItemDetails(GameManager.Instance.itemsHeld[buttonValue]));
        }
    }

        //if the shop is open then carry on
        if (Shop.Instance.shopMenu.activeInHierarchy)
        {

            //if the buy menu is open do this
            if (Shop.Instance.buyMenu.activeInHierarchy)
            {
                Shop.Instance.selectBuyItem(GameManager.Instance.getItemDetails(Shop.Instance.itemsForSale[buttonValue]));
            }

            //if the sell menu is open do this
            if (Shop.Instance.sellMenu.activeInHierarchy)
            {
                Shop.Instance.selectSellItem(GameManager.Instance.getItemDetails(GameManager.Instance.itemsHeld[buttonValue]));
            }

        }

        if (BattleManager.Instance.battleActive == true)
        {

            GameMenu.Instance.selectItem(GameManager.Instance.getItemDetails(GameManager.Instance.itemsHeld[buttonValue]));

        }
}

} // end of script 
