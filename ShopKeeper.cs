using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour {

    private bool canOpen;

    public string[] itemsForSale = new string [40]; 



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

        //if button is pressed and shop not open then open the shop
	void Update () {

        if(canOpen && Input.GetButtonDown("Fire1") && PlayerController2.Instance.canmove==true && !Shop.Instance.shopMenu.activeInHierarchy)
        {

            Shop.Instance.itemsForSale = itemsForSale;

            Shop.Instance.openShop();
        }
		
	}
    //makes sure player is in the zone to enter the shop

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            canOpen = true; 
        }

    }

    //makes sure shop can't be opened when out of the zone
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            canOpen = false;
        }

    }

}
