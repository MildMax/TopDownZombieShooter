using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    GameObject player;
    Vector3 offset;

    Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Awake () {
        
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            //transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, player.transform.position.z + offset.z);

            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), 
                ref velocity, 0.1f * Time.deltaTime);
        }
    }
}
