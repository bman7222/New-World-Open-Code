using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

    //make sure camera stays in map
    public BoxCollider2D boundsBox;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    // hide map edges because unity camera height and width are 1/2 the actual
    private float halfheight;
    private float halfwidth;

    //variables for audio manager
    public int musicToPlay;

    private bool musicStarted; 



	// Use this for initialization
	void Start () {

        //set target
        // BETA VERSION target = PlayerController2.Instance.transform;
        target = FindObjectOfType<PlayerController2>().transform; 

        //camera size 
        halfheight = Camera.main.orthographicSize;
        halfwidth = halfheight * Camera.main.aspect; 

        //set bounds with camera limits
        minBounds = boundsBox.bounds.min + new Vector3(halfwidth,halfheight, 0f);
        maxBounds = boundsBox.bounds.max + new Vector3(-halfwidth, -halfheight, 0f);

        PlayerController2.Instance.setBounds(boundsBox.bounds.min, boundsBox.bounds.max); 
     


    }
	
	// Update is called once per frame
	void LateUpdate ()  {

        //lock on target
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        // keep camera in the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),transform.position.z);

        //if music hasnt stated, then start it
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.Instance.playBGM(musicToPlay); 
        }
	}
}
