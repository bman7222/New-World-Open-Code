using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

    //moveplayer
    public Rigidbody2D RB;
    public float movespeed;

    //player clone task
    public static PlayerController2 Instance;
  

    //animator tasks
    private Animator anim;
    private bool PlayerMoving;
    public Vector2 LastMove;


    //area load
    public string areaTransitionName;



    //make sure the player doesn't exit the map
    private Vector3 minBounds;
    private Vector3 maxBounds;

    //if the player can or cannot move
    public bool canmove; 



    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();

        canmove = true;

        //delete clones if they exist

        if (Instance ==null)
        {
             Instance = this;
          
        }
        else
        {
            //make sure that this object isn't the instance
            if (Instance != this)
            {
                Destroy(gameObject);
            }

        }

        DontDestroyOnLoad(gameObject);

   


    }
	
	// Update is called once per frame
	void Update () {

        //if the player can move then do these things 

        if (canmove)
        {
            RB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * movespeed;

        }
        else
        {
            RB.velocity = Vector2.zero; 
        }

        //animator setter
        anim.SetFloat("Move X", RB.velocity.x);
        anim.SetFloat("Move Y", RB.velocity.y);


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {

            if (canmove)
            {
                anim.SetFloat("Last Move X", Input.GetAxisRaw("Horizontal"));
                anim.SetFloat("Last Move Y", Input.GetAxisRaw("Vertical"));
            }

            }

        
        //clamp player to bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y), transform.position.z);


    }

    //set bounds in accordance with camera script
    public void setBounds(Vector3 botLeft, Vector3 topRight)
    {
        minBounds = botLeft + new Vector3(1f,1f,0f);
        maxBounds = topRight + new Vector3(-0.5f, -1f, 0f);

    }


}
