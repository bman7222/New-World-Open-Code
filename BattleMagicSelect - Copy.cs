using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{

    public string spellName;

    public int spellCost;

    public Text nameText, costText;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Press()
    {

        //when button is pressed, clsoemagic menu, pp box, make MP go down by spell cost, and open the target menu with the selected spell
        if (BattleManager.Instance.activeBattlers[BattleManager.Instance.currentTurn].currentMP >= spellCost)
        {

            //opens target menu to select an enemy to hit
            BattleManager.Instance.openTargetMenu(spellName);

            //closes the magic menu
            BattleManager.Instance.magicMenu.SetActive(false);

            //closes the ppbox
            BattleManager.Instance.ppBox.SetActive(false);

            //makes MP go down by spell cost
            BattleManager.Instance.activeBattlers[BattleManager.Instance.currentTurn].currentMP -= spellCost;

       

        }

        else
        {
            //sets the etxt of the descirpption box
            BattleManager.Instance.descriptionText.text = "Not enough MP.";

            //activates the notification so the descirption box appears 
            BattleManager.Instance.battleNotice.Activate();

            //turns off the ppbox and magic menu 
            BattleManager.Instance.magicMenu.SetActive(false);
            BattleManager.Instance.ppBox.SetActive(false);
        }

    }



}