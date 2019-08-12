using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*If the Blackhole detects a collision with the player or the enemy, it destroys them
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "MainPlayer" || collision.gameObject.name == "EnemyPlayer")
        {
            Destroy(collision.gameObject);
        }
    }
}
