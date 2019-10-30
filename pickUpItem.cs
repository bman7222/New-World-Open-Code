using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpItem : MonoBehaviour {

    private bool canPickUp; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (canPickUp && Input.GetButtonDown("Fire1") && PlayerController2.Instance.canmove == true)
        {
            GameManager.Instance.addItem(GetComponent<Item>().itemName);
            Destroy(gameObject); 
        }

	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {
            canPickUp = true; 
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            canPickUp = false;
        }

    }
}
